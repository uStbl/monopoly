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
        private List<Property> properties;
        private List<Card> cards;
        private int money;
        private int position; // Must be between 0 - (totalSpaces - 1).
        private int remainingJailTurns;

        public Player(int money)
        {
            isInGame = true;
            playerCount++;
            id = playerCount;
            properties = new List<Property>();
            this.money = money;
            remainingJailTurns = 0;
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

        public Property[] GetProperties()
        {
            return properties.ToArray();
        }

        public void AddProperty(Property p)
        {
            properties.Add(p);
        }

        public void AddCard(Card c)
        {
            cards.Add(c);
        }

        public int GetMoney()
        {
            return money;
        }

        public void AddMoney(int money, bool log = true)
        {
            this.money += money;
            if (log)
                Console.WriteLine("You now have {0}.", this.money);
            if (this.money < 0)
                OnBankrupt();
        }

        private void OnBankrupt()
        {
            Console.WriteLine("Player {0} lost from going bankrupt! Better luck next time!", id);
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

        public List<Property> BuildableProperties()
        {
            List<Property> buildable = new List<Property>();

            foreach (Property p in properties)
            {
                int totalInGroup;
                Program.propertyGrouping.TryGetValue(p.GetColor(), out totalInGroup);

                if (AmountOfColor(p.GetColor()) >= totalInGroup)
                    buildable.Add(p);
            }

            return buildable;
        }

        private int AmountOfColor(string color)
        {
            int count = 0;
            foreach (Property p in properties)
            {
                if (p.GetColor().Equals(color))
                    count++;
            }

            return count;
        }
    }
}
