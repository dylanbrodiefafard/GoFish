using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoFishCommon
{
    class Player : IPlayer
    {
        private List<ICard> hand;
        private int handLimit;
        private DeckBase<ICard> deck;

        public Player(String name)
        {
            PlayerID = Guid.NewGuid();
            Name = name;
            hand = null;
            handLimit = 0;
        }

        ~Player()
        {
            if(this.deck != null) this.deck.ReturnCards(this.hand);
        }

        public string Name
        {
            get; private set;
        }

        public int Order
        {
            get; set;
        }

        public Guid PlayerID
        {
            get; private set;
        }

        public void RegisterDeck(DeckBase<ICard> d)
        {
            if (this.deck != null) throw new Exceptions.DirtyDeckException("You must unregister a player's deck before registering a new one.");
            this.deck = d;
        }

        public void UnregisterDeck()
        {
            this.deck = null;
        }

        public void CreateHand(int hl)
        {
            if (hand != null) throw new Exceptions.DirtyHandException("Unable to create a new hand while Player already has a hand. Cleanup hand first.");
            handLimit = hl;
            hand = new List<ICard>(handLimit);
        }

        public void CleanupHand()
        {
            if (hand.Count > 0) throw new Exceptions.DirtyHandException("Unable to cleanup hand. Return all cards to deck first.");
            hand = null;
            handLimit = 0;
        }

        public void AddCardToHand(ICard card)
        {
            if (hand.Count >= handLimit) throw new Exceptions.HandLimitException("Hand limit exceeded.");
            hand.Add(card);
        }

        public int GetHandSize()
        {
            return hand.Count;
        }

        public bool HasCard(ICard card)
        {
            return hand.Contains(card);
        }
    }
}
