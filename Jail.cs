using System;
using System.Collections.Generic;
using System.Text;

namespace monopoly
{
    class Jail : BoardSpace
    {
        public Jail()
        {
            name = "Jail";
        }

        public override void OnPlayerLanding(Player player)
        {
            Console.WriteLine("...but you're just passing by.");
        }
    }
}
