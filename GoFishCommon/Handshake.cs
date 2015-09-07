using System;

namespace GoFishCommon
{
    /// <summary>
    /// Simple handshake object to pass names between server/client.
    /// Also crucial to the client authentication process.
    /// </summary>
    [Serializable]
    public class Handshake
    {
        public readonly String identifier;
        public Handshake(String id)
        {
            identifier = id;
        }
    }
}
