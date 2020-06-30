using System;
using System.Collections.Generic;
using System.Text;

namespace monopoly
{
    class Player
    {
        static private int totalSpaces; // The number of spaces on the game board.

        private int id; // Must be between 1 - # of players in the game.
        private bool isInGame; // If true, the player is still playing. If false, the player has lost. Cannot be set from false to true.
        private int money; // Must be >= 0.
        private int position; // Must be between 0 - (totalSpaces - 1).
        private int remainingJailTurns;

        public int GetId()
        {
            return id;
        }

        public bool HasLost()
        {
            return !isInGame;
        }

        public void LoseGame()
        {
            isInGame = false;
        }

        public int GetMoney()
        {
            return money;
        }

        public void SetMoney(int money)
        {
            this.money = money;
        }

        public int GetPosition()
        {
            return position;
        }

        public void Move(int spaces)
        {
            position += spaces;
            position %= totalSpaces;
        }

        public void MoveTo(int destination)
        {
            if (destination < 0 || destination >= totalSpaces)
            {
                throw new System.ArgumentOutOfRangeException("destination", "The destination must be between 0 and the number of board spaces - 1.");
            }
            else
                position = destination;
        }

        public int GetRemainingJailTurns()
        {
            return remainingJailTurns;
        }

        public void SetRemainingJailTurns(int turns)
        {
            remainingJailTurns = turns;
        }
    }
}
