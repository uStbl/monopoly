using System;
using System.Collections.Generic;
using System.Text;

namespace monopoly
{
    class GoToJail : BoardSpace
    {
        private int jailPosition;

        public override void OnPlayerLanding(Player player)
        {
            player.MoveTo(jailPosition);
        }
    }
}
