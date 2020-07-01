using System;
using System.Collections.Generic;
using System.Text;

namespace monopoly
{
    class Game
    {
        private const int MaxDoubleRolls = 2;

        private Random rnd;
        private List<BoardSpace> boardSpaces;
        private int jailPosition;
        private List<Player> players;

        public Game(List<BoardSpace> boardSpaces, List<Player> players)
        {
            // TODO assign args to fields, set jailPosition based on boardSpaces
        }

        public void PlayGame()
        {
            bool gameIsOver = false;
            while (!gameIsOver)
            {
                foreach (Player currentPlayer in players)
                {
                    if (currentPlayer.GetRemainingJailTurns() > 0)
                    {
                        JailDiceRoll(currentPlayer);
                    }
                    else
                    {
                        DiceRoll(currentPlayer);
                        BoardSpace landedSpace = boardSpaces[currentPlayer.GetPosition()];
                        Console.WriteLine("You have landed on {0}.", landedSpace.GetName());
                        landedSpace.OnPlayerLanding(currentPlayer);
                    }

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
        }

        private void JailDiceRoll(Player player)
        {
            int rollValue1 = rnd.Next(1, 7);
            int rollValue2 = rnd.Next(1, 7);
            if (rollValue1 == rollValue2)
            {
                player.SetRemainingJailTurns(0);
                player.Move(rollValue1 + rollValue2);
            }
            else
            {
                player.SetRemainingJailTurns(player.GetRemainingJailTurns() - 1);
            }
        }

        // Roll dice and move [player].
        // TODO print roll values and prompt player to roll each time
        private void DiceRoll(Player player)
        {
            int rollValue1;
            int rollValue2;
            int rollsThisTurn = 0;
            do
            {
                rollValue1 = rnd.Next(1, 7);
                rollValue2 = rnd.Next(1, 7);
                rollsThisTurn++;

                if (rollsThisTurn >= 3 && rollValue1 == rollValue2)
                {
                    Console.WriteLine("You were sent to jail for rolling doubles 3 times.");
                    player.MoveTo(jailPosition);
                    break;
                }
                else
                {
                    player.Move(rollValue1 + rollValue2);
                }
            } while (rollValue1 == rollValue2);
        }
    }
}
