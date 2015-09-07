using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace GoFishCommon
{
    public class Client
    {
        public String Name;

        //events
        public event Client_Received_Handler On_Client_Received;
        public event Client_Disconnected_Handler On_Client_Disconnected;
        public event Client_Connected_Handler On_Client_Connected;

        private TcpClient socket;
        private IFormatter serializer;
        private ConcurrentQueue<String> writeQueue;
        private NetworkStream stream;
        private MessageHandler handler;

        private int disconnectCalled;

        public Client(TcpClient socket)
        {
            this.socket = socket;
            this.serializer = new BinaryFormatter();
            this.writeQueue = new ConcurrentQueue<String>();
            this.disconnectCalled = 0;
            this.handler = new MessageHandler(socket.Client.LocalEndPoint.ToString() + ".txt");
        }

        public void AuthorizeAndConnect(String name)
        {
            try
            {
                this.stream = socket.GetStream();
                this.serializer.Serialize(stream, handler.Send_ConnectMessage(name));
                String received = (String)this.serializer.Deserialize(stream);
                this.Name = handler.Recieve_ConnectMessage(received);
                this.On_Client_Connected(this, new GenericEventArgs<String>(received));
            }
            catch (Exception e)
            {
                //disconnect if the stream fails, serialization fails, casting fails.
                this.Disconnect();
                Console.WriteLine(e.Message);
            }
        }

        public void StartUp()
        {
            Thread reader = new Thread(new ThreadStart(this.read));
            Thread writer = new Thread(new ThreadStart(this.write));
            reader.IsBackground = true;
            writer.IsBackground = true;
            reader.Start();
            writer.Start();
        }
        /// <summary>
        /// Main loop designed to be run in a thread for each connected client.
        /// Continuously passes received objects to the defined event handler.
        /// Stops when client socket is closed.
        /// </summary>
        private void read()
        {
            NetworkStream clientStream = socket.GetStream();
            while (socket.Connected)
            {
                try
                {
                    String message = (String)this.serializer.Deserialize(clientStream);
                    this.On_Client_Received(this, new GenericEventArgs<String>(message));
                }
                catch (Exception e)
                {
                    //if the cast fails or the socket closes then disconnect
                    this.Disconnect();
                    Console.WriteLine(e.Message);
                }
            }
        }


        private void Disconnect()
        {
            //make sure we only call this once (could be called from read and write thread)
            if (Interlocked.Increment(ref disconnectCalled) == 1)
            {
                this.Close_Connection();
                this.On_Client_Disconnected(this, null);
            }
        }

        public void Close_Connection()
        {
            if (this.socket != null && this.socket.Connected)
            {
                this.socket.Close();
            }
        }

        public void Client_Sending(Object sender, GenericEventArgs<String> e)
        {
            this.writeQueue.Enqueue(e.GetInfo());
            this.writeAvailable.Set();
        }

        private ManualResetEvent writeAvailable = new ManualResetEvent(false);
        private void write()
        {
            while (socket.Connected)
            {
                String message;
                try
                {
                    message = this.writeQueue.Dequeue();
                    try
                    {
                        this.serializer.Serialize(stream, message);
                    }
                    catch (Exception e)
                    {
                        this.Disconnect();
                        Console.WriteLine(e.Message);
                    }
                }
                catch (InvalidOperationException e)
                {
                    this.writeAvailable.WaitOne();
                }
                //lets go to sleep until there is something to do
                this.writeAvailable.WaitOne();
            }
        }
    }

    internal class ConcurrentQueue<T> : ICollection, IEnumerable<T>
    {

            private readonly Queue<T> _queue;

            public ConcurrentQueue()
            {
                _queue = new Queue<T>();
            }

            public IEnumerator<T> GetEnumerator()
            {
                lock (SyncRoot)
                {
                    foreach (var item in _queue)
                    {
                        yield return item;
                    }
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public void CopyTo(Array array, int index)
            {
                lock (SyncRoot)
                {
                    ((ICollection)_queue).CopyTo(array, index);
                }
            }

            public int Count
            {
                get
                {
                    // Assumed to be atomic, so locking is unnecessary
                    return _queue.Count;
                }
            }

            public object SyncRoot
            {
                get { return ((ICollection)_queue).SyncRoot; }
            }

            public bool IsSynchronized
            {
                get { return true; }
            }

            public void Enqueue(T item)
            {
                lock (SyncRoot)
                {
                    _queue.Enqueue(item);
                }
            }

            public T Dequeue()
            {
                lock (SyncRoot)
                {
                    return _queue.Dequeue();
                }
            }

            public T Peek()
            {
                lock (SyncRoot)
                {
                    return _queue.Peek();
                }
            }

            public void Clear()
            {
                lock (SyncRoot)
                {
                    _queue.Clear();
                }
            }
        
    }
}
