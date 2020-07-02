using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace monopoly
{
    class Program
    {
        static void Main(string[] args)
        {
            List<BoardSpace> boardSpaces = FromJson("board.json");
            Player player1 = new Player();
            Player player2 = new Player();
            List<Player> players = new List<Player>();
            players.Add(player1);
            players.Add(player2);

            Console.WriteLine("Welcome to Monopoly!");
            Console.WriteLine("Please enter the amount of money you would like to start with: ");
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

            Game myGame = new Game(boardSpaces, players, startingMoney);
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
                            spaces.Add(new Property(jspace.name.Value, (int)jspace.price.Value, (int)jspace.rent.Value));
                            break;
                        default:
                            Console.WriteLine("you have a space type that you did not match");
                            break;
                    }
                }
            }

            GoToJail goToJail = (GoToJail)spaces.Find(space => space.GetType() == typeof(GoToJail));
            int jailPosition = spaces.FindIndex(space => space.GetType() == typeof(Jail));
            goToJail.SetJailPosition(jailPosition);

            return spaces;
        }
    }
}
