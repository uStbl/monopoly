using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace monopoly
{
    class Program
    {
        private const string path = "../../../board.json";

        static void Main(string[] args)
        {
            List<BoardSpace> boardSpaces = FromJson(path);
            List<Player> players = new List<Player>();

            Console.WriteLine("Welcome to Monopoly!");
            Console.Write("Please enter the number of players: ");
            int playerCount = -1;
            while (playerCount < 2)
            {
                string input = Console.ReadLine().Trim();
                try
                {
                    playerCount = Int32.Parse(input);
                    if (playerCount < 2)
                        throw new System.ArgumentOutOfRangeException();
                }
                catch (Exception e)
                {
                    Console.WriteLine("You have entered an invalid value. Please enter a number equal or greater than 2.");
                }
            }

            Console.Write("Please enter the amount of money you would like to start with: ");
            int startingMoney = -1;
            while (startingMoney < 0)
            {
                string input = Console.ReadLine().Trim();
                try
                {
                    startingMoney = Int32.Parse(input);
                    if (startingMoney < 0)
                        throw new System.ArgumentOutOfRangeException();
                }
                catch (Exception e)
                {
                    Console.WriteLine("You have entered an invalid value. Please enter a non-negative number.");
                }
            }

            Console.Write("Please enter the amount of money to collect when passing GO: ");
            int passMoney = -1;
            while (passMoney < 0)
            {
                string input = Console.ReadLine().Trim();
                try
                {
                    passMoney = Int32.Parse(input);
                    if (passMoney < 0)
                        throw new System.ArgumentOutOfRangeException();
                }
                catch (Exception e)
                {
                    Console.WriteLine("You have entered an invalid value. Please enter a non-negative number.");
                }
            }

            for (int i = 0; i < playerCount; i++)
            {
                players.Add(new Player(startingMoney));
            }

            Game myGame = new Game(boardSpaces, players, passMoney);
            Console.WriteLine("You have started the game!");

            myGame.PlayGame();
        }


        private static List<BoardSpace> FromJson(string path)
        {
            List<BoardSpace> spaces = new List<BoardSpace>();

            using (StreamReader reader = new StreamReader(path))
            {
                string json = reader.ReadToEnd();
                dynamic jspaces = JsonConvert.DeserializeObject(json);
                foreach (var jspace in jspaces)
                {
                    switch (jspace.type.Value)
                    {
                        case "go":
                            spaces.Add(new GoSpace());
                            break;
                        case "free parking":
                            spaces.Add(new FreeParking());
                            break;
                        case "jail":
                            spaces.Add(new Jail());
                            break;
                        case "go to jail":
                            spaces.Add(new GoToJail());
                            break;
                        case "tax":
                            spaces.Add(new TaxSpace(jspace.name.Value, (int)jspace.GetValue("tax amount").Value));
                            break;
                        case "property":
                            spaces.Add(new Property(jspace.name.Value, jspace.color.Value, (int)jspace.price.Value,
                                (int)jspace.rent.Value, (int)jspace.GetValue("house price").Value, jspace.GetValue("house rents").ToObject(typeof(int[]))));
                            break;
                        case "railroad":
                            spaces.Add(new Railroad(jspace.name.Value, (int)jspace.price.Value, (int)jspace.rent.Value));
                            break;
                        case "utility":
                            spaces.Add(new Utility(jspace.name.Value, (int)jspace.price.Value, (int)jspace.multiplier.Value));
                            break;
                        case "community chest":
                            spaces.Add(new CommunityChest());
                            break;
                        case "chance":
                            spaces.Add(new Chance());
                            break;
                        default:
                            Console.WriteLine("you have a space type that you did not match");
                            break;
                    }
                }
            }

            return spaces;
        }
    }
}
