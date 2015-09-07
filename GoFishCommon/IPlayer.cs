using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
