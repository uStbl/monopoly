using System;
using System.Collections.Generic;
using System.Text;

namespace monopoly
{
    abstract class BoardSpace
    {
        protected string name;
        protected Game containingGame;

        public abstract void OnPlayerLanding(Player player);

        public string GetName()
        {
            return name;
        }

        public void SetGame(Game game)
        {
            containingGame = game;
        }
    }
}
