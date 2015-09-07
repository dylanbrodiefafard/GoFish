using System;

namespace GoFishCommon.Exceptions
{
    [Serializable]
    class DirtyHandException : Exception
    {
        public DirtyHandException(string message) : base(message) { }
    }
}
