using System;
using System.Collections.Generic;
using System.Text;

namespace monopoly
{
    class Player
    {
        static private int playerCount = 0; // The number of players that have been created.

        private Game containingGame; // The game that this player is in.
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
            cards = new List<Card>();
            this.money = money;
            remainingJailTurns = 0;
        }

        public void SetGame(Game game)
        {
            containingGame = game;
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
                Console.WriteLine("You now have ${0}.", this.money);
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
            CalculatePassMoney(spaces);

            position += spaces;
            position %= containingGame.GetTotalSpaces();

            while (position < 0)
                position += containingGame.GetTotalSpaces();

            Console.WriteLine("You moved {0} spaces forward and landed on {1}.", spaces, containingGame.BoardSpaceAt(position).GetName());
        }

        public void MoveTo(int destination, bool collectPassMoney = true, bool log = true)
        {
            int spaces = DistanceTo(destination);

            if (collectPassMoney)
                CalculatePassMoney(spaces);

            if (destination < 0 || destination >= containingGame.GetTotalSpaces())
            {
                throw new System.ArgumentOutOfRangeException("destination", "The destination must be between 0 and the number of board spaces - 1.");
            }
            else
                position = destination;

            if (log)
                Console.WriteLine("You moved {0} spaces forward and landed on {1}.", spaces, containingGame.BoardSpaceAt(position).GetName());
        }

        // Add money if moving [spaces] will move the player past GO.
        private void CalculatePassMoney(int spaces)
        {
            int distanceToGo; // distance to the go space
            int goPosition = containingGame.FindPosition("GO");
            distanceToGo = DistanceTo(goPosition);

            if (spaces > distanceToGo)
            {
                Console.WriteLine("You gained ${0} for passing go!", containingGame.GetPassMoney());
                money += containingGame.GetPassMoney();
            }
        }

        public int DistanceTo(int destination)
        {
            if (destination > position)
                return destination - position;
            else
                return destination + (containingGame.GetTotalSpaces() - position);
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
                if (p.GetType() == typeof(Property))
                {
                    int totalInGroup;
                    containingGame.GetColorGrouping().TryGetValue(p.GetColor(), out totalInGroup);

                    if (AmountOfColor(p.GetColor()) >= totalInGroup)
                        buildable.Add(p);
                }
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
