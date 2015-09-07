using System;

namespace GoFishCommon
{
    /// <summary>
    /// Interface defining a player
    /// </summary>
    interface IPlayer
    {
        int Order { get; set; }
        string Name { get; }
        Guid PlayerID { get; }
    }
}
