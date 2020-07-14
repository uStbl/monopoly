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
        private List<BoardSpace> boardSpaces;
        private List<Player> players;

        // boardSpaces must be a legal game board. passMoney must not be negative.
        public Game(List<BoardSpace> boardSpaces, List<Player> players, int passMoney)
        {
            this.boardSpaces = boardSpaces;
            this.players = players;
            this.passMoney = passMoney;
            goPosition = boardSpaces.FindIndex(space => space.GetType() == typeof(GoSpace));
            jailPosition = boardSpaces.FindIndex(space => space.GetType() == typeof(Jail));

            GoToJail goToJail = (GoToJail)boardSpaces.Find(space => space.GetType() == typeof(GoToJail));
            GoSpace goSpace = (GoSpace)boardSpaces.Find(space => space.GetType() == typeof(GoSpace));
            goToJail.SetJailPosition(jailPosition);
            goSpace.SetPassMoney(passMoney);

            ChestChance.SetGoPosition(goPosition);
            ChestChance.SetJailPosition(jailPosition);
            ChestChance.SetPlayers(players.ToArray());

            Player.SetTotalSpaces(boardSpaces.Count);
            foreach (Player p in players)
                p.MoveTo(goPosition);

            rnd = new Random();
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
                        NormalTurn(currentPlayer);

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
                    Console.WriteLine("You may build on these properties:");

                    Console.WriteLine("Enter the number of the property you would like to build on.");
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

        private void PromptForEnter()
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
                player.Move(rollValue1 + rollValue2);
                BoardSpace landedSpace = boardSpaces[player.GetPosition()];
                Console.WriteLine("You rolled {0} and {1} and broke out of jail!", rollValue1, rollValue2);
                Console.WriteLine("You moved {0} spaces forward and landed on {1}.", rollSum, landedSpace.GetName());

                if (landedSpace.GetType() != typeof(Utility))
                    landedSpace.OnPlayerLanding(player);
                else
                    ((Utility)landedSpace).OnPlayerLanding(player, rollSum);
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
        private void NormalTurn(Player player)
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
                    player.MoveTo(jailPosition);
                    player.SetRemainingJailTurns(3);
                    break;
                }
                else
                {
                    int distanceToGo; // distance to the go space
                    if (player.GetPosition() < goPosition)
                    {
                        distanceToGo = goPosition - player.GetPosition();
                    }
                    else
                    {
                        distanceToGo = goPosition + (boardSpaces.Count - player.GetPosition());
                    }

                    int rollSum = rollValue1 + rollValue2;
                    player.Move(rollSum);
                    BoardSpace landedSpace = boardSpaces[player.GetPosition()];
                    Console.WriteLine("You moved {0} spaces forward and landed on {1}.", rollSum, landedSpace.GetName());

                    if (rollSum > distanceToGo)
                    {
                        Console.WriteLine("You gained ${0} for passing go!", passMoney);
                        player.AddMoney(passMoney);
                    }

                    if (landedSpace.GetType() != typeof(Utility))
                        landedSpace.OnPlayerLanding(player);
                    else
                        ((Utility)landedSpace).OnPlayerLanding(player, rollSum);

                    if (player.HasLost()) break;

                    if (rollValue1 == rollValue2)
                        Console.WriteLine("Due to rolling doubles, you get to take another turn!");
                }
            } while (rollValue1 == rollValue2);
        }
    }
}
