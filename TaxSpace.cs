using System;
using System.Collections.Generic;
using System.Text;

namespace monopoly
{
    class TaxSpace : BoardSpace
    {
        private int taxAmount;

        public override void OnPlayerLanding(Player player)
        {
            Console.WriteLine("You had to pay {0}.", player.GetMoney());
            player.AddMoney(-taxAmount);
        }
    }
}
