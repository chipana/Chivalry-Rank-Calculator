using System;

namespace ChivalryRank
{
    class Operations
    {
        public static double ExpToKills(double xp)
        {
            double kills = Math.Floor(Convert.ToSingle(24000 * (Math.Pow(2, xp / 6) - 1) / Math.Log(2)) / 200);
            return kills;
        }
        public static float GetRankByExp(int xpValue)
        {
            float rank = Convert.ToSingle(6 * Math.Log((xpValue * Math.Log(2) / 24000) + 1) / Math.Log(2));
            return rank;
        }

    }
}
