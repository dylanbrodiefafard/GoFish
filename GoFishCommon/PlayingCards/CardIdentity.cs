using System;

namespace GoFishCommon.PlayingCards
{
    public class CardIdentity : IComparable<CardIdentity>
    {
        public CardIdentity(char code, string name, int value, int order, bool isFaceCard, bool isJoker)
        {
            Code = code;
            Name = name;
            Value = value;
            Order = order;
            IsFaceCard = isFaceCard;
            IsJoker = isJoker;
        }

        public CardIdentity(char code, string name, int value, int order, bool isFaceCard)
        {
            Code = code;
            Name = name;
            Value = value;
            Order = order;
            IsFaceCard = isFaceCard;
            IsJoker = false;
        }

        public char Code { get; set; }
        public string Name { get; set; }
        public int Value { get; set; }
        public int Order { get; set; }
        public bool IsFaceCard { get; set; }
        public bool IsJoker { get; set; }

        #region IComparable<CardIdentity> Members

        public int CompareTo(CardIdentity other)
        {
            if (Order > other.Order) return 1;
            if (Order == other.Order) return 0;
            return -1;
        }

        #endregion

        public static CardIdentity Joker(int value)
        {
            return new CardIdentity(Char.Parse(value.ToString()), "Joker", value, 100, true, true);
        }

        public static CardIdentity Unknown()
        {
            return new CardIdentity('N', "None", -1, 100, false, false);
        }
    }
}