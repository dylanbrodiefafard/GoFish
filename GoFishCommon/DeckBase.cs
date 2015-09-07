using System;
using System.Collections.Generic;
using System.Linq;

namespace GoFishCommon
{
    public abstract class DeckBase<TCard> : List<TCard> where TCard : ICard
    {
        protected DeckBase()
        {
            DeckId = Guid.NewGuid();
        }

        public Guid DeckId { get; private set; }

        /// <summary>
        ///     Gets the count of unassigned cards in this deck.
        /// </summary>
        /// <value>The unassigned card count.</value>
        public int UnassignedCardCount
        {
            get { return this.Where(x => x.AssignedTo == null).Count(); }
        }

        /// <summary>
        ///     Assigns X number of cards to the specified card set.
        /// </summary>
        /// <param name = "numberOfCards">The number of cards.</param>
        /// <param name = "cardSet">The card set.</param>
        /// <returns></returns>
        public IEnumerable<TCard> AssignSet(int numberOfCards, ICardSet cardSet)
        {
            if (this.Where(x => x.AssignedTo == null).Count() < numberOfCards)
                throw new Exception("Insufficient cards remain to assign.");

            foreach (var card in this.Where(x => x.AssignedTo == null).Take(numberOfCards))
                card.AssignedTo = cardSet;

            return CardsAssignedTo(cardSet);
        }

        /// <summary>
        ///     Returns list of cards assigned to a card set.
        /// </summary>
        /// <param name = "cardSet">The card set.</param>
        /// <returns></returns>
        public IEnumerable<TCard> CardsAssignedTo(ICardSet cardSet)
        {
            return this.Where(x => x.AssignedTo != null && x.AssignedTo == cardSet);
        }

        /// <summary>
        ///     Shuffles this deck instance.
        /// </summary>
        public void Shuffle()
        {
            // Note: this is a better way
            // this.OrderBy(a => Guid.NewGuid());

            var rand = new Random();
            for (var i = Count - 1; i > 0; i--)
            {
                var n = rand.Next(i + 1);
                var temp = this[i];
                this[i] = this[n];
                this[n] = temp;
            }
        }

        /// <summary>
        ///     Shuffles the specified cards.
        /// </summary>
        /// <param name = "cards">The cards.</param>
        public void Shuffle(IEnumerable<TCard> cards)
        {
            throw new NotImplementedException("Shuffle set of cards has not yet been implemented.");
        }

        /// <summary>
        ///     Shuffles the specified deck a number of times.
        /// </summary>
        /// <param name = "shuffles">Number of shuffles.</param>
        public void Shuffle(int shuffles)
        {
            if (shuffles > 0 && shuffles <= 1000)
            {
                for (var z = 0; z < shuffles; z++)
                    Shuffle();
            }
            else
                Shuffle();
        }

        /// <summary>
        ///     Shuffles the set.
        /// </summary>
        /// <param name = "shuffles">The shuffles.</param>
        /// <param name = "cardSet">The card set.</param>
        public void ShuffleSet(int shuffles, ICardSet cardSet)
        {
            throw new NotImplementedException("ShuffleSet has not yet been implemented.");
        }

        /// <summary>
        ///     Reassigns the card.
        /// </summary>
        /// <param name = "card">The card.</param>
        /// <param name = "newCardSet">The new card set.</param>
        /// <param name = "stackPosition">The stack position.</param>
        public void ReassignCard(TCard card, ICardSet newCardSet, int stackPosition)
        {
            ReassignCard(card, newCardSet);
            card.StackOrder = stackPosition;
        }

        public void ReassignCard(TCard card, ICardSet newCardSet)
        {
            if (card.FromDeckId != DeckId) throw new Exception("Card is not from this deck.");

            card.AssignedTo = newCardSet;
        }

        /// <summary>
        ///     Reassigns the cards.
        /// </summary>
        /// <param name = "cards">The cards.</param>
        /// <param name = "cardSet">The target card set.</param>
        public void ReassignCards(IEnumerable<TCard> cards, ICardSet cardSet)
        {
            foreach (var card in cards)
                ReassignCard(card, cardSet);
        }

        /// <summary>
        ///     Returns the card to the deck (deassigns the card).
        /// </summary>
        /// <param name = "playingCard">The card.</param>
        public void ReturnCard(TCard playingCard)
        {
            if (playingCard.FromDeckId == DeckId)
                playingCard.AssignedTo = null;
            else
                throw new Exception("Supplied card is not from this deck");
        }

        /// <summary>
        ///     Returns the cards to the deck (deassigns the cards).
        /// </summary>
        /// <param name = "cards">The cards.</param>
        public void ReturnCards(IEnumerable<TCard> cards)
        {
            foreach (var card in cards)
                ReturnCard(card);
        }

        /// <summary>
        ///     Unassigns all cards.
        /// </summary>
        public void ReturnAllCards()
        {
            foreach (var card in this)
                ReturnCard(card);
        }

        /// <summary>
        ///     Reutns a card from the specified code.
        /// </summary>
        /// <param name = "code">The card id.</param>
        /// <returns></returns>
        public TCard Card(string code)
        {
            var r = this.SingleOrDefault(x => x.Code == code);

            if (r == null) throw new Exception("Invalid card id, did not recognise: " + code);

            return r;
        }

        /// <summary>
        ///     Gets a list of cards from a list of card codes
        /// </summary>
        /// <param name = "cardCodes">The card ids.</param>
        /// <returns></returns>
        public List<TCard> Cards(string[] cardCodes)
        {
            return cardCodes.Select(Card).ToList();
        }
    }
}