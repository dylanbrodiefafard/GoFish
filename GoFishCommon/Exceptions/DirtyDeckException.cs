using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoFishCommon.Exceptions
{
    class DirtyDeckException : Exception
    {
        public DirtyDeckException(string message) : base(message)
        {
        }
    }
}
