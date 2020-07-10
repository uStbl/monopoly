using System;
using System.Collections.Generic;
using System.Text;

namespace monopoly
{
    class GoSpace : BoardSpace
    {
        private int passMoney;

        public GoSpace()
        {
            name = "GO";
        }

        public void SetPassMoney(int passMoney)
        {
            this.passMoney = passMoney;
        }

        public override void OnPlayerLanding(Player player)
        {
            int total = Game.LandingMultiplier * passMoney;
            Console.WriteLine("You gained {0} for landing directly on Go!", total);
            player.AddMoney(total);
        }

        public void OnPlayerPassing(Player player)
        {
            player.AddMoney(passMoney);
        }
    }
}
