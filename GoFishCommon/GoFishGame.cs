using System;

namespace GoFishCommon
{
    public class GoFishGame
    {
        public readonly int Num_Players;
        public readonly String Game_Name;

        public GoFishGame(String gameName, int numPlayers)
        {
            this.Num_Players = numPlayers;
            this.Game_Name = gameName;
        }
    }
}
