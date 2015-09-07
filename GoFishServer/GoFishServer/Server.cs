using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Timers;
using GoFishCommon;
using RoomList = System.Collections.Generic.Dictionary<System.String, GoFishCommon.Room>;

namespace GoFishServer
{
    /// <summary>
    /// This class handles the communication between and management of clients.
    /// Maximum #of clients connected at once is specified in the App.config.
    /// </summary>
    public class Server : IClientHandler
    {
        public event Client_Sending_Handler On_Client_Sending;
        private ManualResetEvent listening = new ManualResetEvent(false);
        private RoomList rooms;
        private readonly BinaryFormatter serializer;
        private int connectedClients;
        private int guestNumber = 0;
        private readonly int maxClients = 4; // Int32.Parse(ConfigurationManager.AppSettings["maxClients"]);
        private readonly int port = 6657; // Int32.Parse(ConfigurationManager.AppSettings["port"]);

        public Server()
        {
            this.serializer = new BinaryFormatter();
            this.rooms = new RoomList();
            this.connectedClients = 0;
            this.On_Client_Sending += delegate { }; //dummy client to get past checking for null (no clients connected)
        }

        public static int Main(String[] args)
        {
            Server server = new Server();
            server.listen();
            return 0;
        }

        /// <summary>
        /// Main running loop of the server.
        /// Uses a thread signal to listen for a new client before proceeding.
        /// </summary>
        public void listen()
        {
            //output server information to console.
            IPHostEntry server = Dns.GetHostEntry(Dns.GetHostName());
            Console.WriteLine("Listening on port {0}", this.port);
            Console.WriteLine(server.HostName);
            foreach (IPAddress IP in server.AddressList)
            {
                Console.WriteLine(IP.ToString());
            }

            //setup maintenance timer
            int minutes = 60; // Int32.Parse(ConfigurationManager.AppSettings["maintenanceInterval"]);
            double interval = TimeSpan.FromMinutes(minutes).TotalMilliseconds;
            System.Timers.Timer maintenanceTimer = new System.Timers.Timer(interval);
            maintenanceTimer.Elapsed += maintenanceTimer_Elapsed;
            maintenanceTimer.Enabled = true;

            TcpListener listener = new TcpListener(IPAddress.Any, this.port);
            listener.Start();
            //main loop
            while (listener.Server.IsBound)
            {
                this.listening.Reset();
                listener.BeginAcceptTcpClient(Client_Accepted, listener);
                this.listening.WaitOne();
            }
        }

        /// <summary>
        /// Callback event for the async listening method. (from main loop)
        /// If the server is not full then it
        /// sends the client the proper handshake sequence and then starts a clientReader thread.
        /// </summary>
        /// <param name="ar"></param>
        public void Client_Accepted(IAsyncResult ar)
        {
            this.listening.Set(); //start listening for a new client on the main thread
            TcpClient clientSocket = ((TcpListener)ar.AsyncState).EndAcceptTcpClient(ar);
            Client client = new Client(clientSocket);
            //subscribe to published events
            client.On_Client_Received += this.Client_Received;
            client.On_Client_Connected += this.Client_Connected;
            client.On_Client_Disconnected += this.Client_Disconnected;
            client.AuthorizeAndConnect("Guest_" + Interlocked.Increment(ref this.guestNumber));
        }

        /// <summary>
        /// Event handler for the maintenance timer.
        /// Runs maintenance on the server. (removes old rooms)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void maintenanceTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (this.rooms)
            {
                Console.WriteLine("=== Running Maintenance...");
                foreach (Room room in this.rooms.Values)
                {
                    double daysSinceUsed = DateTime.Now.Subtract(room.LastUsed).TotalDays;
                    double daysLimit = 7; // TimeSpan.FromDays(Double.Parse(ConfigurationManager.AppSettings["roomTTL"])).TotalDays;
                    //Delete any rooms older than the limit (default is 7 days). Also should be empty. 
                    if (daysSinceUsed > daysLimit && room.Users.Count == 0)
                    {
                        Console.WriteLine("Deleting {0}. Room is {1} days old", room.Name, daysSinceUsed);
                        RoomAction action = new RoomAction.DeleteRoom(room.Owner, room.Name);
                        action.Perform(this.rooms);
                        this.On_Client_Sending(this, new GenericEventArgs<RoomAction>(action));
                    }
                }
                Console.WriteLine("=== Maintenance finished...");
            }
        }

        /// <summary>
        /// Event handler for client disconnecting.
        /// Will unsubscribe the proper events.
        /// and removes the client from all rooms that he was in.
        /// </summary>
        /// <param name="sender">Client that disconnected</param>
        /// <param name="e">nothing</param>
        public void Client_Disconnected(Client sender, EventArgs e)
        {
            this.On_Client_Sending -= sender.Client_Sending;
            Interlocked.Decrement(ref this.connectedClients);
            //send a leave room action for each room the client was in.
            lock (this.rooms)
            {
                foreach (Room room in this.rooms.Values)
                {
                    if (room.Users.Contains(sender.Name))
                    {
                        RoomAction action = new RoomAction.LeaveRoom(sender.Name, room.Name);
                        action.Perform(this.rooms);
                        this.On_Client_Sending(this, new GenericEventArgs<RoomAction>(action));
                    }
                }
            }

            Console.WriteLine("Client disconnected {0}", sender.Name);
        }

        /// <summary>
        /// Event handler for when a client connects.
        /// Makes sure there is enough room on the server before allowing it to proceed.
        /// if successful it starts the client read/write threads.
        /// </summary>
        /// <param name="sender">Client that is connecting</param>
        /// <param name="e">the handshake (contains its name)</param>
        public void Client_Connected(Client sender, GenericEventArgs<Handshake> e)
        {
            sender.Name = e.GetInfo().identifier;
            if (!Interlocked.Equals(this.connectedClients, this.maxClients))
            {
                Interlocked.Increment(ref this.connectedClients);
                sender.SendRooms(rooms); //send all the rooms to the client
                this.On_Client_Sending += sender.Client_Sending; //subscribe to the send to all event
                sender.StartUp();
                Console.WriteLine("Client connected: {0}", sender.Name);
            }
            else
            {
                sender.Close_Connection();
                Console.WriteLine("Client refused, server full: {0}", sender.Name);
            }

        }

        /// <summary>
        /// Event handler for Successfull read of an object sent from a client.
        /// </summary>
        /// <param name="sender">ClientReader Thread</param>
        /// <param name="e">RoomAction (hopefully)</param>
        public void Client_Received(Client sender, GenericEventArgs<RoomAction> e)
        {
            RoomAction action = e.GetInfo();
            //if the action was succesfully validated and performed then send it to all currently connected clients.
            if (action.Perform(this.rooms))
            {
                this.On_Client_Sending(this, new GenericEventArgs<RoomAction>(action));
            }
        }
    }
}
