using System;
using System.Collections.Generic;
using System.Text;

namespace monopoly
{
    class Game
    {
        public const int LandingMultiplier = 2;
        public const int JailTurns = 3;

        private const int MaxDoubleRolls = 2;

        private Random rnd;
        private int goPosition;
        private int jailPosition;
        private int passMoney;
        private Dictionary<string, int> colorGrouping;
        private List<BoardSpace> boardSpaces;
        private List<Player> players;

        // boardSpaces must be a legal game board. passMoney must not be negative.
        public Game(List<BoardSpace> boardSpaces, List<Player> players, int passMoney)
        {
            this.boardSpaces = boardSpaces;
            this.players = players;
            this.passMoney = passMoney;
            colorGrouping = PropertiesPerGroup();
            goPosition = boardSpaces.FindIndex(space => space.GetType() == typeof(GoSpace));
            jailPosition = boardSpaces.FindIndex(space => space.GetType() == typeof(Jail));

            Deck chestDeck = new Deck(true, this);
            Deck chanceDeck = new Deck(false, this);
            foreach (BoardSpace b in boardSpaces)
            {
                b.SetGame(this);

                if (b.GetType() == typeof(CommunityChest))
                    ((CommunityChest)b).SetDeck(chestDeck);

                if (b.GetType() == typeof(Chance))
                    ((Chance)b).SetDeck(chanceDeck);
            }

            foreach (Player p in players)
            {
                p.SetGame(this);
                p.MoveTo(goPosition, false, false);
            }

            rnd = new Random();
        }

        private Dictionary<string, int> PropertiesPerGroup()
        {
            Dictionary<string, int> grouping = new Dictionary<string, int>();

            foreach (BoardSpace space in boardSpaces)
            {
                if (space.GetType() == typeof(Property))
                {
                    Property property = (Property)space;
                    string color = property.GetColor();

                    if (grouping.ContainsKey(color))
                    {
                        int incremented;
                        grouping.Remove(color, out incremented);
                        incremented++;
                        grouping.Add(color, incremented);
                    }
                    else
                    {
                        grouping.Add(color, 1);
                    }
                }
            }
            return grouping;
        }

        public Dictionary<string, int> GetColorGrouping()
        {
            return colorGrouping;
        }

        public int FindPosition(string name)
        {
            return boardSpaces.FindIndex(space => space.GetName() == name);
        }

        public BoardSpace BoardSpaceAt(int position)
        {
            return boardSpaces[position];
        }

        public int GetPassMoney()
        {
            return passMoney;
        }

        public Player[] GetPlayers()
        {
            return players.ToArray();
        }

        public int GetTotalSpaces()
        {
            return boardSpaces.Count;
        }

        public void PlayGame()
        {
            bool gameIsOver = false;
            while (!gameIsOver)
            {
                foreach (Player currentPlayer in players)
                {
                    Console.WriteLine("It is player {0}'s turn.", currentPlayer.GetId());

                    List<Property> buildables = currentPlayer.BuildableProperties();

                    if (buildables.Count > 0)
                        BuySellHouses(buildables);

                    if (currentPlayer.GetRemainingJailTurns() > 0)
                        JailTurn(currentPlayer);
                    else
                        DevTurn(currentPlayer);

                    Console.WriteLine("-------------------------------------------------");

                    if (currentPlayer.HasLost())
                    {
                        players.RemoveAt(currentPlayer.GetId() - 1);
                        if (players.Count <= 1)
                        {
                            gameIsOver = true;
                            break;
                        }
                    }
                }
            }

            Console.WriteLine("Player {0} has won the game! Congratulations!", players[0].GetId());
        }

        private void BuySellHouses(List<Property> properties)
        {
            Console.WriteLine("Would you like to buy or sell houses or hotels?");
            Console.WriteLine("Y/N");
            ConsoleKey input;
            do
            {
                input = Console.ReadKey(true).Key;
            } while (!(input == ConsoleKey.Y || input == ConsoleKey.N));
            while (input != ConsoleKey.N)
            {
                if (input == ConsoleKey.Y)
                {
                    Console.WriteLine("You may develop these properties:\n");
                    PrintProperties(properties);
                    Console.WriteLine("Enter the number of the property you would like to develop.");
                    Console.Write("> ");
                    int propertyNumber = -1;

                    while (propertyNumber == -1)
                    {
                        try
                        {
                            propertyNumber = Convert.ToInt32(Console.ReadLine().Trim());
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("You have entered an invalid value. Please enter the number of a property.");
                        }

                        if (propertyNumber < 1 || propertyNumber > properties.Count)
                        {
                            Console.WriteLine("You have entered an invalid value. Please enter the number of a property.");
                            propertyNumber = -1;
                        }
                    }

                    BuildOnProperty(properties[propertyNumber - 1]);

                    Console.WriteLine("Would you like to buy or sell on any other properties?");
                    Console.WriteLine("Y/N");
                    do
                    {
                        input = Console.ReadKey(true).Key;
                    } while (!(input == ConsoleKey.Y || input == ConsoleKey.N));

                    if (input == ConsoleKey.N)
                        Console.WriteLine("You declined to buy or sell houses or hotels.");
                }
                else if (input == ConsoleKey.N)
                {
                    Console.WriteLine("You declined to buy or sell houses or hotels.");
                }
            }
        }

        private void PrintProperties(List<Property> properties)
        {
            for (int i = 0; i < properties.Count; i++)
            {
                Property p = properties[i];
                Console.WriteLine($"{i + 1}. {p.GetName()}");
                Console.WriteLine($"Current rent: {p.GetRent()}");
                Console.WriteLine($"Houses built: {(p.GetHouses() == 5 ? 0 : p.GetHouses())}");
                Console.WriteLine($"Hotel built: {(p.GetHouses() == 5 ? "Yes" : "No")}");
                Console.WriteLine();
            }
        }

        private void BuildOnProperty(Property property)
        {
            Console.WriteLine($"You selected {property.GetName()}.");
            Console.WriteLine("Would you like to buy or sell houses?");
            Console.WriteLine("Press B to buy, S to sell, or C to cancel.");

            ConsoleKey input;
            do
            {
                input = Console.ReadKey(true).Key;
            } while (!(input == ConsoleKey.B || input == ConsoleKey.S || input == ConsoleKey.C));

            if (input == ConsoleKey.B)
            {
                Console.WriteLine("How many houses would you like to buy?");
                Console.WriteLine($"Your money: {property.GetOwner().GetMoney()}");
                Console.WriteLine($"Price per house: {property.GetHousePrice()}");
                Console.Write("> ");

                int buyAmount = -1;
                while (!(1 <= buyAmount && buyAmount <= 5))
                {
                    string inputLine = Console.ReadLine().Trim();
                    try
                    {
                        buyAmount = Int32.Parse(inputLine);
                        if (!(1 <= buyAmount && buyAmount <= 5))
                            throw new System.ArgumentOutOfRangeException();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("You have entered an invalid value. Please enter a number between 1 and 5.");
                    }
                }

                property.tryBuy(buyAmount);
            }
            else if (input == ConsoleKey.S)
            {
                Console.WriteLine("How many houses would you like to sell?");
                Console.WriteLine($"Your money: {property.GetOwner().GetMoney()}");
                Console.WriteLine($"Price per house: {property.GetHousePrice()}");
                Console.Write("> ");

                int sellAmount = -1;
                while (!(1 <= sellAmount && sellAmount <= 5))
                {
                    string inputLine = Console.ReadLine().Trim();
                    try
                    {
                        sellAmount = Int32.Parse(inputLine);
                        if (!(1 <= sellAmount && sellAmount <= 5))
                            throw new System.ArgumentOutOfRangeException();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("You have entered an invalid value. Please enter a number between 1 and 5.");
                    }
                }

                property.trySell(sellAmount);
            }
            else if (input == ConsoleKey.C)
            {
                Console.WriteLine("You decided not to buy or sell on this property.");
            }
        }

        public void PromptForEnter()
        {
            Console.WriteLine("Press enter to roll dice.");
            ConsoleKey input;
            do
            {
                input = Console.ReadKey(true).Key;
            } while (input != ConsoleKey.Enter);
        }

        private void JailTurn(Player player)
        {
            Console.WriteLine("You are in jail. If you roll doubles, you will break out of jail and move forward.");
            PromptForEnter();
            int rollValue1 = rnd.Next(1, 7);
            int rollValue2 = rnd.Next(1, 7);
            int rollSum = rollValue1 + rollValue2;
            if (rollValue1 == rollValue2)
            {
                player.SetRemainingJailTurns(0);
                Console.WriteLine("You rolled {0} and {1} and broke out of jail!", rollValue1, rollValue2);
                MoveAndLand(player, rollSum);
            }
            else
            {
                int newRemainingTurns = player.GetRemainingJailTurns() - 1;
                player.SetRemainingJailTurns(newRemainingTurns);
                Console.WriteLine("Unfortunately, you rolled {0} and {1}.", rollValue1, rollValue2);
                Console.WriteLine("You must remain in jail for {0} more turns.", newRemainingTurns);
            }
        }

        // Roll dice and move [player].
        public void NormalTurn(Player player)
        {
            int rollValue1;
            int rollValue2;
            int rollsThisTurn = 0;
            do
            {
                PromptForEnter();
                rollValue1 = rnd.Next(1, 7);
                rollValue2 = rnd.Next(1, 7);
                rollsThisTurn++;
                Console.WriteLine("You rolled {0} and {1}.", rollValue1, rollValue2);

                if (rollsThisTurn > MaxDoubleRolls && rollValue1 == rollValue2)
                {
                    Console.WriteLine("You were sent to jail for rolling doubles 3 times.");
                    player.MoveTo(jailPosition, false);
                    player.SetRemainingJailTurns(3);
                    break;
                }
                else
                {
                    int rollSum = rollValue1 + rollValue2;

                    MoveAndLand(player, rollSum);

                    if (player.HasLost()) break;

                    if (rollValue1 == rollValue2)
                        Console.WriteLine("Due to rolling doubles, you get to take another turn!");
                }
            } while (rollValue1 == rollValue2);
        }

        private void MoveAndLand(Player player, int spaces)
        {
            player.Move(spaces);
            BoardSpace landedSpace = boardSpaces[player.GetPosition()];

            if (landedSpace.GetType() != typeof(Utility))
                landedSpace.OnPlayerLanding(player);
            else
                ((Utility)landedSpace).OnPlayerLanding(player, spaces);
        }

        // Move [player] by a chosen number of spaces. For development purposes.
        private void DevTurn(Player player)
        {
            Console.WriteLine("How many spaces do you want to move?");
            int input = Convert.ToInt32(Console.ReadLine());

            MoveAndLand(player, input);
        }
    }
}
