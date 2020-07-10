using System;
using System.Collections.Generic;
using System.Text;

namespace monopoly
{
    class Property : BoardSpace
    {
        protected Player owner;
        private string color;
        protected int price;
        protected int rent;
        private int baseRent;
        private int houses;
        private int housePrice;
        private int[] houseRents;

        public Property(string name, string color, int price, int rent, int housePrice, int[] houseRents)
        {
            this.name = name;
            this.color = color;
            this.price = price;
            this.rent = rent;
            this.baseRent = rent;
            houses = 0;
            this.housePrice = housePrice;
            this.houseRents = houseRents;
        }

        public string GetColor()
        {
            return color;
        }

        public int GetRent()
        {
            return rent;
        }

        public int GetHouses()
        {
            return houses;
        }

        public override void OnPlayerLanding(Player player)
        {
            if (owner == null)
            {
                PromptToBuy(player);
            }
            else if (owner != player)
            {
                CollectRent(player);
            }
            else
            {
                PassBy();
            }
        }

        protected virtual void PromptToBuy(Player player)
        {
            int playerMoney = player.GetMoney();
            if (playerMoney >= price)
            {
                Console.WriteLine("Would you like to purchase this property?");
                Console.WriteLine("Your money: {0}", playerMoney);
                Console.WriteLine("Cost of property: {0}", price);
                Console.WriteLine("Property group: {0}", color);
                Console.WriteLine("Rent amount: {0}", rent);
                Console.WriteLine("Y/N");

                ConsoleKey input;
                do
                {
                    input = Console.ReadKey(true).Key;
                } while (!(input == ConsoleKey.Y || input == ConsoleKey.N));

                if (input == ConsoleKey.Y)
                {
                    Console.WriteLine("Congratulations! You have bought {0}.", name);
                    owner = player;
                    player.AddProperty(this);
                    player.AddMoney(-price);
                    updateRent();
                }
                else if (input == ConsoleKey.N)
                {
                    Console.WriteLine("You declined to purchase the property.");
                }
            }
            else
            {
                Console.WriteLine("You do not have enough money to purchase this property. Too bad!");
            }
        }

        protected void CollectRent(Player player)
        {
            Console.WriteLine("This property is owned by player {0}!", owner.GetId());
            Console.WriteLine("You paid player {0} ${1}.", owner.GetId(), rent);
            player.AddMoney(-rent);
            owner.AddMoney(rent, false);
            Console.WriteLine("Player {0} now has ${1}.", owner.GetId(), owner.GetMoney());
        }

        protected void PassBy()
        {
            Console.WriteLine("You own this property. You admire it as you pass by.");
        }

        protected virtual void updateRent()
        {
            List<Property> sameGroup = new List<Property>();
            foreach (Property p in owner.GetProperties())
            {
                if (p.color.Equals(color))
                    sameGroup.Add(p);
            }

            bool hasMonopoly = false;
            int totalInGroup;
            Program.propertyGrouping.TryGetValue(color, out totalInGroup);

            if (sameGroup.Count >= totalInGroup)
                hasMonopoly = true;

            if (hasMonopoly)
            {
                foreach (Property p in sameGroup)
                {
                    p.rent *= 2;
                }
                Console.WriteLine("You have a monopoly on {0} properties! Rents have been doubled.", color);
            }
        }

        public void tryBuy(int amount)
        {
            if (houses + amount > 5)
                Console.WriteLine("You cannot build that many houses on this property.");
            else if (owner.GetMoney() < amount * housePrice)
                Console.WriteLine("You do not have enough money to buy that many houses.");
            else
            {
                houses += amount;
                rent = houseRents[houses - 1];
                Console.WriteLine("You built {0}! The new rent of your property is {1}.", houses == 5 ? "a hotel" : (amount == 1 ? "a house" : amount + "houses"), rent);
                owner.AddMoney(-amount * housePrice);
            }
        }

        public void trySell(int amount)
        {
            if (houses - amount < 0)
                Console.WriteLine("You do not have that many houses on this property.");
            else
            {
                houses += amount;
                if (houses > 0)
                    rent = houseRents[houses - 1];
                else
                    rent = 2 * baseRent;
                Console.WriteLine("You built {0}! The new rent of your property is {1}.", houses == 5 ? "a hotel" : (amount == 1 ? "a house" : amount + " houses"), rent);
                owner.AddMoney((int)(amount * .5 * housePrice));
            }
        }
    }
}
