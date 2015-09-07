namespace GoFishCommon
{
    public struct CardId
    {
        private readonly string _cardId;

        private CardId(string cardId)
        {
            _cardId = cardId;
        }

        public int Length
        {
            get { return ToString().Length; }
        }

        public override string ToString()
        {
            return _cardId;
        }

        public static implicit operator CardId(string cardId)
        {
            return new CardId(cardId);
        }

        public static bool operator ==(CardId x, CardId y)
        {
            return x.ToString() == y.ToString();
        }

        public static bool operator !=(CardId x, CardId y)
        {
            return x.ToString() != y.ToString();
        }

        public override int GetHashCode()
        {
            return (_cardId != null ? _cardId.GetHashCode() : 0);
        }

        public bool Equals(CardId other)
        {
            return Equals(other._cardId, _cardId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof (CardId)) return false;
            return Equals((CardId) obj);
        }
    }
}