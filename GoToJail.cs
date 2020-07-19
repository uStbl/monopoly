using System;
using System.Collections.Generic;
using System.Text;

namespace monopoly
{
    class GoToJail : BoardSpace
    {
        public GoToJail()
        {
            name = "Go to Jail";
        }

        public override void OnPlayerLanding(Player player)
        {
            Console.WriteLine("You were sent to jail.");
            Console.WriteLine("You must remain in jail for {0} more turns.", Game.JailTurns);
            player.MoveTo(containingGame.FindPosition("Jail"), false, false);
            player.SetRemainingJailTurns(Game.JailTurns);
        }
    }
}
