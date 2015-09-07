using System;

namespace GoFishCommon.PlayingCards
{
    public class PlayingCard : ICard
    {
        public PlayingCard(CardSuit cardSuit, CardIdentity cardIdentity, Guid deckId)
        {
            Suit = cardSuit;
            Identity = cardIdentity;
            FromDeckId = deckId;
        }

        public CardSuit Suit { get; set; }
        public CardIdentity Identity { get; set; }

        #region ICard Members

        /// <summary>
        ///     Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get { return Identity.Name + " of " + Suit.Name; }
        }

        public Guid FromDeckId { get; set; }
        public ICardSet AssignedTo { get; set; }
        public int StackOrder { get; set; }

        /// <summary>
        ///     Gets the card code.
        /// </summary>
        /// <value>The card code.</value>
        public string Code
        {
            get { return string.Concat(Identity.Code, Suit.Code); }
        }

        #endregion


        /// <summary>
        ///     Gets the specified card from a deck.
        /// </summary>
        /// <param name = "id"></param>
        /// <param name = "deckId">The deck id.</param>
        /// <returns></returns>
        /// <summary>
        ///     Determines whether the specified <see cref = "System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name = "obj">The <see cref = "System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///     <c>true</c> if the specified <see cref = "System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref = "T:System.NullReferenceException">
        ///     The <paramref name = "obj" /> parameter is null.
        /// </exception>
        public override bool Equals(object obj)
        {
            return obj is PlayingCard ? ((PlayingCard) obj).Code == Code : false;
        }

        /// <summary>
        ///     Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        ///     A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return Code.GetHashCode();
        }

        /// <summary>
        ///     Generates a card from the specified code.
        /// </summary>
        /// <param name = "id">The id.</param>
        /// <returns></returns>
        /// <summary>
        ///     Returns a <see cref = "System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///     A <see cref = "System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var result = Code + " - " + Identity.Name + " - " + Description;
            if (AssignedTo != null) result += " - Assigned: " + AssignedTo.CardSetId;
            return result;
        }

        /// <summary>
        ///     Determines whether card is from the specified deck.
        /// </summary>
        /// <param name = "deckId">The deck id.</param>
        /// <returns>
        ///     <c>true</c> if is from deck otherwise, <c>false</c>.
        /// </returns>
        public bool IsFromDeck(Guid deckId)
        {
            return FromDeckId == deckId;
        }

        public static PlayingCard UnknownCard()
        {
            return new PlayingCard(CardSuit.NoSuit(), CardIdentity.Unknown(), Guid.Empty);
        }

        public static PlayingCard Joker(int number, Guid deckId)
        {
            return new PlayingCard(CardSuit.Joker(), CardIdentity.Joker(number), deckId);
        }
    }
}