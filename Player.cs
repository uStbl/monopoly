using System;
using System.Collections.Generic;
using System.Text;

namespace monopoly
{
    class Player
    {
        static private int totalSpaces; // The number of spaces on the game board.
        static private int playerCount = 0; // The number of players that have been created.

        private int id; // Must be between 1 - # of players in the game.
        private bool isInGame; // If true, this player is still playing. If false, this player has lost.
        private int money;
        private int position; // Must be between 0 - (totalSpaces - 1).
        private int remainingJailTurns;

        public Player()
        {
            isInGame = true;
            playerCount++;
            id = playerCount;
        }

        public static void SetTotalSpaces(int spaces)
        {
            totalSpaces = spaces;
        }

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

        public void AddMoney(int money)
        {
            this.money += money;
            Console.WriteLine("You now have {0}.", this.money);
            if (this.money < 0)
                OnBankrupt();
        }

        private void OnBankrupt()
        {
            Console.WriteLine("You lost from going bankrupt! Better luck next time!");
            isInGame = false;
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
