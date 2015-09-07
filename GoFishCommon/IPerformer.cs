using System;
using System.Collections.Generic;
using RoomList = System.Collections.Generic.Dictionary<System.String, GoFishCommon.Room>;

namespace GoFishCommon
{
    /// <summary>
    /// Simple interface to guarantee that all actions have a perform method.
    /// The server implicitly expects a perform method from the client.
    /// </summary>
    interface IPerformer
    {
        Boolean Perform(RoomList rooms);
    }
}
