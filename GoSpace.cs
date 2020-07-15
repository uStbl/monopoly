using System;
using System.Collections.Generic;
using System.Text;

namespace monopoly
{
    class GoSpace : BoardSpace
    {
        public GoSpace()
        {
            name = "GO";
        }

        public override void OnPlayerLanding(Player player)
        {
            int total = Game.LandingMultiplier * containingGame.GetPassMoney();
            Console.WriteLine("You gained {0} for landing directly on Go!", total);
            player.AddMoney(total);
        }

        public void OnPlayerPassing(Player player)
        {
            player.AddMoney(containingGame.GetPassMoney());
        }
    }
}
