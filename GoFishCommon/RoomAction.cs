using System;
using System.Collections.Generic;
using System.Diagnostics;
using RoomList = System.Collections.Generic.Dictionary<System.String, GoFishCommon.Room>;

namespace GoFishCommon
{
    /// <summary>
    /// Container class for all actions that the client may perform.
    /// Contains the user, the room name, the message if needed, and method to perform.
    /// </summary>
    [Serializable]
    public abstract class RoomAction : IPerformer
    {
        public readonly String User;
        public readonly String Room;

        public abstract Boolean Perform(RoomList rooms);

        public RoomAction(String user, String room)
        {
            this.User = user;
            this.Room = room;
        }

        /// <summary>
        /// Useful debug messages for the server console.
        /// </summary>
        [Conditional("DEBUG")]
        public void DebugMessage()
        {
            Console.WriteLine("[{0}][{1}][{2}][{3}]", DateTime.Now, this.GetType(), this.User, this.Room);
        }

        /// <summary>
        /// Action to create a room.
        /// Pre: room name is unique.
        /// </summary>
        [Serializable]
        public class CreateRoom : RoomAction
        {
            public CreateRoom(String user, String room) : base(user, room) { }

            public override Boolean Perform(RoomList rooms)
            {
                lock (rooms)
                {
                    if (!rooms.ContainsKey(this.Room))
                    {
                        DebugMessage();
                        rooms.Add(this.Room, new Room(this.Room, this.User));
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Action to delete a room.
        /// Pre: room name exists, no users in room, and this user is the owner of the room.
        /// </summary>
        [Serializable]
        public class DeleteRoom : RoomAction
        {
            public DeleteRoom(String user, String room) : base(user, room) { }

            public override Boolean Perform(RoomList rooms)
            {
                lock (rooms)
                {
                    Room toDelete = null;
                    if (rooms.TryGetValue(this.Room, out toDelete) && toDelete != null)
                    {
                        if (toDelete.Users.Count == 0 && toDelete.Owner.Equals(this.User))
                        {
                            DebugMessage();
                            rooms.Remove(this.Room);
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Action for a user to join a room.
        /// Pre: room exists, user is not already in the room.
        /// </summary>
        [Serializable]
        public class JoinRoom : RoomAction
        {
            public JoinRoom(String user, String room) : base(user, room) { }

            public override Boolean Perform(RoomList rooms)
            {
                lock (rooms)
                {
                    Room toJoin = null;
                    if (rooms.TryGetValue(this.Room, out toJoin) && toJoin != null)
                    {
                        if (!toJoin.Users.Contains(this.User))
                        {
                            DebugMessage();
                            rooms[this.Room].Join(this.User);
                            return true;
                        }
                    }
                    return false;
                }
            }
        }

        /// <summary>
        /// Action for a user to leave a room.
        /// Pre: room exists, user is actually in the room.
        /// </summary>
        [Serializable]
        public class LeaveRoom : RoomAction
        {
            public LeaveRoom(String user, String room) : base(user, room) { }

            public override Boolean Perform(RoomList rooms)
            {
                lock (rooms)
                {
                    Room toLeave = null;
                    if (rooms.TryGetValue(this.Room, out toLeave) && toLeave != null)
                    {
                        if (toLeave.Users.Contains(this.User))
                        {
                            DebugMessage();
                            rooms[this.Room].Leave(this.User);
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Action to send a message to a room.
        /// Pre: room exists, user is in the room.
        /// </summary>
        [Serializable]
        public class SendMessage : RoomAction
        {
            public readonly String Message;
            public SendMessage(String user, String room, String message) : base(user, room) { this.Message = message; }

            public override Boolean Perform(RoomList rooms)
            {
                lock (rooms)
                {
                    Room toSend = null;
                    if (rooms.TryGetValue(this.Room, out toSend) && toSend != null)
                    {
                        if (toSend.Users.Contains(this.User))
                        {
                            DebugMessage();
                            String msg;
                            if (this.Message == "gimmie card")
                            {
                                msg = toSend.DrawCard(this.User);
                                toSend.PostMessage(this.User, msg);
                            }
                            else
                            {
                                toSend.PostMessage(this.User, this.Message);
                            }
                            
                            return true;
                        }
                    }
                }
                return false;
            }
        }
    }
}
