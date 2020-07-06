using System;
using System.Collections.Generic;
using System.Text;

namespace monopoly
{
    class Utility : Property
    {
        private int rentMultiplier;

        public Utility(string name, int price, int rentMultiplier) : base(name, price, 0)
        {
            this.rentMultiplier = rentMultiplier;
        }

        protected override void PromptToBuy(Player player)
        {
            int playerMoney = player.GetMoney();
            if (playerMoney >= price)
            {
                Console.WriteLine("Would you like to purchase this property?");
                Console.WriteLine("Your money: {0}", playerMoney);
                Console.WriteLine("Cost of property: {0}", price);
                Console.WriteLine("Rent amount: {0} x dice roll (multiplier increases with each utility you own)", rentMultiplier);
                Console.WriteLine("Y/N");

                ConsoleKey input;
                do
                {
                    input = Console.ReadKey(true).Key;

                    if (input == ConsoleKey.Y)
                    {
                        Console.WriteLine("Congratulations! You have bought {0}.", name);
                        owner = player;
                        player.AddMoney(-price);
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
    }
}
