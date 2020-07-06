using System;
using System.Collections.Generic;
using System.Text;

namespace monopoly
{
    class GoToJail : BoardSpace
    {
        private const int jailTurns = 3;
        private int jailPosition;

        public GoToJail()
        {
            name = "Go to Jail";
        }

        public override void OnPlayerLanding(Player player)
        {
            Console.WriteLine("You were sent to jail.");
            Console.WriteLine("You must remain in jail for {0} more turns.", jailTurns);
            player.MoveTo(jailPosition);
            player.SetRemainingJailTurns(jailTurns);
        }

        public void SetJailPosition(int jailPosition)
        {
            this.jailPosition = jailPosition;
        }
    }
}
