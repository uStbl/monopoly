using System;
using System.Collections.Generic;
using System.Text;

namespace monopoly
{
    class GoSpace : BoardSpace
    {
        public const int PassMoney = 200;
        public override void OnPlayerLanding(Player player)
        {
            return;
        }

        public void OnPlayerPassing(Player player)
        {
            player.AddMoney(PassMoney);
        }
    }
}
