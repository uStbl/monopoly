using System;
using System.Collections.Generic;
using System.Text;

namespace monopoly
{
    class GoSpace : BoardSpace
    {
        public GoSpace() {
            name = "Go";
        }

        public override void OnPlayerLanding(Player player)
        {
            int total = Game.LandingMultiplier * Game.PassMoney;
            Console.WriteLine("You gained {0} for landing directly on Go!", total);
            player.AddMoney(total);
        }

        public void OnPlayerPassing(Player player)
        {
            player.AddMoney(Game.PassMoney);
        }
    }
}
