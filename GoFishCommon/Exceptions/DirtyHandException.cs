using System;

namespace GoFishCommon.Exceptions
{
    class DirtyHandException : Exception
    {
        public DirtyHandException(string message) : base(message) { }
    }
}
