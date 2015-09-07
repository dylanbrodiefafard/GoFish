using System;
using System.Collections.Generic;
using System.Linq;

namespace GoFishCommon
{
    /// <summary>
    /// The generic chatroom. Contains the owner, room name, when it was created.
    /// List of users, and message.
    /// </summary>
    [Serializable]
    public class Room
    {
        private String owner;
        private String roomName;
        private DateTime lastUsed;
        private Queue<String> messages;
        private Dictionary<String, DateTime> users;
        private DeckBase<ICard> deck;

        public Room(String roomName, String owner)
        {
            this.roomName = roomName;
            this.owner = owner;
            this.lastUsed = DateTime.Now;
            this.messages = new Queue<String>();
            this.users = new Dictionary<String, DateTime>();
            //this.deck = new DeckBase<ICard>();
        }

        public string DrawCard(String user)
        {
            return "Test";
        }

        public void PostMessage(String user, String message)
        {
            lastUsed = DateTime.Now;
            this.messages.Enqueue(message);
        }

        public void Leave(String user)
        {
            this.users.Remove(user);
        }

        public void Join(String user)
        {
            this.users.Add(user, DateTime.Now);
        }

        public DateTime LastUsed
        {
            get { return this.lastUsed; }
        }

        public List<String> Messages
        {
            get { return this.messages.ToList<string>(); }
        }

        public List<String> Users
        {
            get { return this.users.Keys.ToList<String>(); }
        }

        public String Owner
        {
            get { return this.owner; }
        }

        public String Name
        {
            get { return this.roomName; }
        }
    }
}
