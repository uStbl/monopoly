using System;
using System.Collections.Generic;
using System.Text;

namespace monopoly
{
    class Railroad : Property
    {
        public Railroad(string name, int price, int rent) : base(name, price, rent)
        {
        }

        protected override void PromptToBuy(Player player)
        {
            int playerMoney = player.GetMoney();
            if (playerMoney >= price)
            {
                Console.WriteLine("Would you like to purchase this property?");
                Console.WriteLine("Your money: {0}", playerMoney);
                Console.WriteLine("Cost of property: {0}", price);
                Console.WriteLine("Rent amount: {0} (increases with each railroad you own)", rent);
                Console.WriteLine("Y/N");

                ConsoleKey input;
                do
                {
                    input = Console.ReadKey(true).Key;

                    if (input == ConsoleKey.Y)
                    {
                        Console.WriteLine("Congratulations! You have bought {0}.", name);
                        owner = player;
                        player.GetProperties().Add(this);
                        player.AddMoney(-price);
                        updateRent();
                    }
                    else if (input == ConsoleKey.N)
                    {
                        Console.WriteLine("You declined to purchase the property.");
                    }
                } while (!(input == ConsoleKey.Y || input == ConsoleKey.N));
            }
            else
            {
                Console.WriteLine("You do not have enough money to purchase this property. Too bad!");
            }
        }

        private void updateRent()
        {
            List<Railroad> railroads = new List<Railroad>();
            foreach (Property p in owner.GetProperties())
            {
                if (p.GetType() == typeof(Railroad))
                    railroads.Add((Railroad)p);
            }

            int newRent = 0;

            switch (railroads.Count)
            {
                case 1:
                    newRent = 25;
                    break;

                case 2:
                    newRent = 50;
                    break;

                case 3:
                    newRent = 100;
                    break;

                case 4:
                    newRent = 200;
                    break;
            }

            foreach (Railroad r in railroads)
            {
                r.rent = newRent;
            }
        }
    }
}
