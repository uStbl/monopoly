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

        public Player GetOwner()
        {
            return owner;
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

        public int GetHousePrice()
        {
            return housePrice;
        }

        public int[] GetHouseRents()
        {
            return houseRents;
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

                ConsoleKey input = Game.ReadYN();

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
                    Auction();
                }
            }
            else
            {
                Console.WriteLine("You do not have enough money to purchase this property. Too bad!");
                Auction();
            }
        }

        private void Auction()
        {
            Console.WriteLine("\nAn auction has begun for {0}!", name);

            bool auctionInProcess = true;
            Player[] players = containingGame.GetPlayers();
            bool[] biddedThisRound = new bool[players.Length];
            int highestBid = 0;
            Player highestBidder = null;

            while (auctionInProcess)
            {
                for (int i = 0; i < biddedThisRound.Length; i++)
                    biddedThisRound[i] = false;

                for (int i = 0; i < players.Length; i++)
                {
                    Player p = players[i];

                    if (highestBidder == p)
                    {
                        Console.WriteLine("Player {0} won the auction with a bid of ${1} and bought {2}!", highestBidder.GetId(), highestBid, name);
                        owner = highestBidder;
                        highestBidder.AddMoney(-highestBid, false);
                        Console.WriteLine("Player {0} now has ${1}.", highestBidder.GetId(), highestBidder.GetMoney());
                        auctionInProcess = false;
                        break;
                    }

                    Console.WriteLine("It is player {0}'s turn to bid.", p.GetId());
                    Console.WriteLine("Would you like to make a bid?");
                    Console.WriteLine("Y/N");
                    ConsoleKey input = Game.ReadYN();

                    if (input == ConsoleKey.Y)
                    {
                        int bidAmount = 0;
                        while (bidAmount == 0)
                        {
                            Console.Write("Enter the amount you would like to bid (enter -1 to cancel): ");
                            try
                            {
                                bidAmount = Convert.ToInt32(Console.ReadLine().Trim());
                                if (bidAmount != -1 && bidAmount <= highestBid)
                                {
                                    Console.WriteLine("You have entered an invalid value. Enter a non-negative number higher than the current highest bid of ${0}.", highestBid);
                                    throw new Exception();
                                }
                                if (p.GetMoney() < bidAmount)
                                {
                                    Console.WriteLine("You do not have that much money.");
                                    throw new Exception();
                                }
                            }
                            catch (Exception e)
                            {
                                bidAmount = 0;
                            }
                        }
                        if (bidAmount != -1)
                        {
                            highestBid = bidAmount;
                            highestBidder = p;
                            biddedThisRound[i] = true;
                        }
                    }
                    else if (input == ConsoleKey.N)
                    {
                        Console.WriteLine("You declined to make a bid.");
                    }
                    Console.WriteLine();
                }

                if (highestBidder == null)
                {
                    Console.WriteLine("Nobody wanted to buy {0}.", name);
                    auctionInProcess = false;
                }
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
            containingGame.GetColorGrouping().TryGetValue(color, out totalInGroup);

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

        public void tryBuy()
        {
            List<Property> buildables = owner.BuildableProperties();
            foreach (Property p in buildables)
                if (p.GetColor() == color && houses > p.houses)
                {
                    Console.WriteLine("You must build evenly. One or more of your other properties in this color group does not have enough houses.");
                    return;
                }

            if (houses >= 5)
                Console.WriteLine("You already have a hotel on this property.");
            else if (owner.GetMoney() < housePrice)
                Console.WriteLine("You do not have enough money to buy that many houses.");
            else
            {
                houses++;
                rent = houseRents[houses - 1];
                Console.WriteLine("You built {0}! The new rent of your property is {1}.", houses == 5 ? "a hotel" : "a house", rent);
                owner.AddMoney(-housePrice);
            }
        }

        public void trySell()
        {
            List<Property> buildables = owner.BuildableProperties();
            foreach (Property p in buildables)
                if (p.GetColor() == color && houses < p.houses)
                {
                    Console.WriteLine("You must sell evenly. One or more of your other properties in this color group has too many houses.");
                    return;
                }

            if (houses <= 0)
                Console.WriteLine("You do not have any houses on this property.");
            else
            {
                houses--;
                if (houses > 0)
                    rent = houseRents[houses - 1];
                else
                    rent = 2 * baseRent;
                Console.WriteLine("You sold {0}! The new rent of your property is {1}.", houses == 4 ? "a hotel" : "a house", rent);
                owner.AddMoney((int)(.5 * housePrice));
            }
        }
    }
}
