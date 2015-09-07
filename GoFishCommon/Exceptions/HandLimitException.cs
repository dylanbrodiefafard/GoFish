using System;

namespace GoFishCommon.Exceptions
{
    [Serializable]
    class HandLimitException : Exception
    {
        public HandLimitException(string message) : base(message)
        {
        }
    }
}
