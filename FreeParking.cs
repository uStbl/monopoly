using System;
using System.Collections.Generic;
using System.Text;

namespace monopoly
{
    class FreeParking : BoardSpace
    {
        public override void onPlayerLanding(Player player)
        {
            Console.WriteLine("You landed on free parking!");
        }
    }
}
