using System;

namespace GoFishCommon.PlayingCards
{
    public class CardSuit : IComparable<CardSuit>
    {
        public CardSuit(char code, string name, int order)
        {
            Code = code;
            Name = name;
            Order = order;
        }

        public char Code { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }

        #region IComparable<CardSuit> Members

        public int CompareTo(CardSuit other)
        {
            if (Order > other.Order) return 1;
            if (Order == other.Order) return 0;
            return -1;
        }

        #endregion

        public static CardSuit NoSuit()
        {
            return new CardSuit('N', "None", 100);
        }

        public static CardSuit Joker()
        {
            return new CardSuit('J', "Joker", 100);
        }
    }
}