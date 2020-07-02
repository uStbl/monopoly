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
            return;
        }

        public void OnPlayerPassing(Player player)
        {
            player.AddMoney(Game.PassMoney);
        }
    }
}
