﻿using System;
using System.Collections.Generic;
using System.Text;

namespace monopoly
{
    class Game
    {
        public const int PassMoney = 200;
        public const int LandingMoney = 400;
        public const int JailTurns = 3;

        private const int MaxDoubleRolls = 2;

        private Random rnd;
        private int goPosition;
        private int jailPosition;
        private List<BoardSpace> boardSpaces;
        private List<Player> players;

        // boardSpaces must be a legal game board. startingMoney must not be negative.
        public Game(List<BoardSpace> boardSpaces, List<Player> players, int startingMoney)
        {
            this.boardSpaces = boardSpaces;
            this.players = players;
            goPosition = boardSpaces.FindIndex(space => space.GetType() == typeof(GoSpace));
            jailPosition = boardSpaces.FindIndex(space => space.GetType() == typeof(Jail));
            GoToJail.SetJailPosition(jailPosition);
            Player.SetTotalSpaces(boardSpaces.Count);
            foreach (Player p in players)
            {
                p.AddMoney(startingMoney, false);
                p.MoveTo(goPosition);
                p.SetRemainingJailTurns(0);
            }

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
            if (rollValue1 == rollValue2)
            {
                player.SetRemainingJailTurns(0);
                player.Move(rollValue1 + rollValue2);
                BoardSpace landedSpace = boardSpaces[player.GetPosition()];
                Console.WriteLine("You rolled {0} and {1} and broke out of jail!", rollValue1, rollValue2);
                Console.WriteLine("You moved {0} spaces forward and landed on {1}.", rollValue1 + rollValue2, landedSpace.GetName());
                landedSpace.OnPlayerLanding(player);
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

                    if (rollSum > distanceToGo) {
                        Console.WriteLine("You gained ${0} for passing go!", PassMoney);
                        player.AddMoney(PassMoney);
                    }

                    landedSpace.OnPlayerLanding(player);

                    if (player.HasLost()) break;

                    if (rollValue1 == rollValue2)
                        Console.WriteLine("Due to rolling doubles, you get to take another turn!");
                }
            } while (rollValue1 == rollValue2);
        }
    }
}
