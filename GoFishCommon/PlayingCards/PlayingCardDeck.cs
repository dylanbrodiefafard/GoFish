using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace GoFishCommon.PlayingCards
{
    public class PlayingCardDeck : DeckBase<PlayingCard>
    {
        private CardIdentities _cardIdentities;
        private CardSuits _cardSuits;

        public PlayingCardDeck()
        {
            Initialize(CardSuits.Default(), CardIdentities.AceLow(), 0);
        }

        public PlayingCardDeck(int numberOfJokers)
        {
            Initialize(CardSuits.Default(), CardIdentities.AceLow(), numberOfJokers);
        }

        public PlayingCardDeck(CardSuits cardSuits, CardIdentities cardIdentities, int numberOfJokers)
        {
            Initialize(cardSuits, cardIdentities, numberOfJokers);
        }

        public ReadOnlyCollection<CardSuit> Suits
        {
            get { return new ReadOnlyCollection<CardSuit>(_cardSuits); }
        }

        public ReadOnlyCollection<CardIdentity> Numbers
        {
            get { return new ReadOnlyCollection<CardIdentity>(_cardIdentities); }
        }

        /// <summary>
        ///     Initializes the deck, creates cards, adds jokers etc.
        /// </summary>
        /// <param name = "suits">The suits.</param>
        /// <param name = "identities">The numbers.</param>
        /// <param name = "numberOfJokers">The number of jokers.</param>
        private void Initialize(CardSuits suits, CardIdentities identities, int numberOfJokers)
        {
            _cardSuits = suits;
            _cardIdentities = identities;

            foreach (var cardSuit in suits)
                foreach (var cardNumber in identities)
                    Add(new PlayingCard(cardSuit, cardNumber, DeckId));

            for (var i = 0; i < numberOfJokers; i++)
                Add(PlayingCard.Joker(i, DeckId));
        }


        /// <summary>
        ///     Gets a card by suit and number.
        /// </summary>
        /// <param name = "number">The number.</param>
        /// <param name = "suit">The suit.</param>
        /// <returns></returns>
        public PlayingCard Card(int number, CardSuit suit)
        {
            return this.FirstOrDefault(x => x.Identity.Order == number && x.Suit == suit);
        }

        /// <summary>
        ///     Gets all cards from a specified suit
        /// </summary>
        /// <param name = "suit">The suit.</param>
        /// <returns></returns>
        public List<PlayingCard> SuitCards(CardSuit suit)
        {
            return Enumerable.Range(1, 12).Select(x => Card(x, suit)).ToList();
        }
    }
}