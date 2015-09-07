using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoFishCommon.Exceptions
{
    class DirtyHandException : Exception
    {
        public DirtyHandException(string message) : base(message) { }
    }
}
