using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoFishCommon
{
    public interface IMessageProcessor
    {
        void Process_drawcard(String payload);
        void Process_hostgame(String payload);
        void Process_joingame(String payload);
    }
}
