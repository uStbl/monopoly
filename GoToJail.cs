using System;
using System.Collections.Generic;
using System.Text;

namespace monopoly
{
    class GoToJail : BoardSpace
    {
        private static int jailPosition;

        public GoToJail()
        {
            name = "Go to Jail";
        }

        public override void OnPlayerLanding(Player player)
        {
            Console.WriteLine("You were sent to jail.");
            Console.WriteLine("You must remain in jail for {0} more turns.", Game.JailTurns);
            player.MoveTo(jailPosition);
            player.SetRemainingJailTurns(Game.JailTurns);
        }

        public static void SetJailPosition(int jailPosition)
        {
            GoToJail.jailPosition = jailPosition;
        }
    }
}
