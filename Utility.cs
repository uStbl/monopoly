using System;
using System.Collections.Generic;
using System.Text;

namespace monopoly
{
    class Utility : Property
    {
        private int rentMultiplier;

        public Utility(string name, int price, int rentMultiplier) : base(name, "", price, 0, 0, new int[0])
        {
            this.rentMultiplier = rentMultiplier;
        }

        public override void OnPlayerLanding(Player player)
        {
            if (owner == null)
            {
                PromptToBuy(player);
            }
            else if (owner != player)
            {
                Console.WriteLine("This property is owned by player {0}!", owner.GetId());
                Console.WriteLine("Roll dice to determine the rent you owe. Rent will be ten times the amount thrown.");
                Game.PromptForEnter();
                Random rnd = new Random();
                int diceRoll = rnd.Next(1, 7) + rnd.Next(1, 7);
                rentMultiplier = 10;
                CollectRent(player, diceRoll);
                updateRent();
            }
            else
            {
                PassBy();
            }
        }

        public void OnPlayerLanding(Player player, int diceRoll)
        {
            if (owner == null)
            {
                PromptToBuy(player);
            }
            else if (owner != player)
            {
                Console.WriteLine("This property is owned by player {0}!", owner.GetId());
                CollectRent(player, diceRoll);
            }
            else
            {
                PassBy();
            }
        }

        protected override void PromptToBuy(Player player)
        {
            int playerMoney = player.GetMoney();
            if (playerMoney >= price)
            {
                owner = player;
                player.AddProperty(this);
                updateRent();

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
                        owner = null;
                        player.RemoveProperty(this);
                        updateRent();
                    }
                } while (!(input == ConsoleKey.Y || input == ConsoleKey.N));
            }
            else
            {
                Console.WriteLine("You do not have enough money to purchase this property. Too bad!");
            }
        }

        private void CollectRent(Player player, int diceRoll)
        {
            int rent = diceRoll * rentMultiplier;
            Console.WriteLine("You paid player {0} ${1} ({2} x {3}).", owner.GetId(), rent, diceRoll, rentMultiplier);
            player.AddMoney(-rent);
            owner.AddMoney(rent, false);
            Console.WriteLine("Player {0} now has ${1}.", owner.GetId(), owner.GetMoney());
        }

        protected override void updateRent()
        {
            List<Utility> utilities = new List<Utility>();
            foreach (Property p in owner.GetProperties())
            {
                if (p.GetType() == typeof(Utility))
                    utilities.Add((Utility)p);
            }

            int newMultiplier = 0;

            switch (utilities.Count)
            {
                case 1:
                    newMultiplier = 4;
                    break;

                case 2:
                    newMultiplier = 10;
                    break;
            }

            foreach (Utility u in utilities)
            {
                u.rentMultiplier = newMultiplier;
            }
        }
    }
}
