using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoFishCommon
{
    public interface IMessageProcessor
    {
        void Process_DrawCard(String payload);
        void Process_HostGame(String payload);
        void Process_JoinGame(String payload);
        void Process_Connect(String payload);
        void Process_Disconnect(String payload);
    }
}
