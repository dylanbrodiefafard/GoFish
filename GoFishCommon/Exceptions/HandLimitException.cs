using System;

namespace GoFishCommon.Exceptions
{
    class HandLimitException : Exception
    {
        public HandLimitException(string message) : base(message)
        {
        }
    }
}
