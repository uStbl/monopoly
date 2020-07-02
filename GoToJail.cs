using System;
using System.Collections.Generic;
using System.Text;

namespace monopoly
{
    class GoToJail : BoardSpace
    {
        private int jailPosition;

        public GoToJail()
        {
            name = "Go to Jail";
        }

        public override void OnPlayerLanding(Player player)
        {
            player.MoveTo(jailPosition);
            player.SetRemainingJailTurns(3);
        }

        public void SetJailPosition(int jailPosition)
        {
            this.jailPosition = jailPosition;
        }
    }
}
