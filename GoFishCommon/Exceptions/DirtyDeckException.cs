using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoFishCommon.Exceptions
{
    [Serializable]
    class DirtyDeckException : Exception
    {
        public DirtyDeckException(string message) : base(message)
        {
        }
    }
}
