using System;
using System.Collections.Generic;
using System.Text;

namespace monopoly
{
    class Property : BoardSpace
    {
        private Player owner;
        private int price;
        private int rent;

        public Property(string name, int price, int rent)
        {
            this.name = name;
            this.price = price;
            this.rent = rent;
        }
        public override void OnPlayerLanding(Player player)
        {

            if (owner == null)
            {
                PromptToBuy(player);
            }
            else
            {
                CollectRent(player);
            }
        }

        private void PromptToBuy(Player player)
        {
            int playerMoney = player.GetMoney();
            if (playerMoney >= price)
            {
                Console.WriteLine("Would you like to purchase this property?");
                ConsoleKey input;
                Console.WriteLine("Your money: {0}", playerMoney);
                Console.WriteLine("Cost of property: {0}", price);
                Console.WriteLine("Rent amount: {0}", rent);
                Console.WriteLine("Y/N");

                do
                {
                    input = Console.ReadKey(true).Key;

                    if (input == ConsoleKey.Y)
                    {
                        owner = player;
                        player.SetMoney(playerMoney - price);
                        Console.WriteLine("Congratulations! You have bought {0}.", name);
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

        private void CollectRent(Player player)
        {
            Console.WriteLine("This property is owned by player {0}!" + owner.GetId());
            Console.WriteLine("You paid player {0} ${1}.", owner.GetId(), rent);
        }
    }
}
