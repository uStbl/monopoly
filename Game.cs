using System;
using System.Collections.Generic;
using System.Text;

namespace monopoly
{
    class Game
    {
        private const int MaxDoubleRolls = 2;

        private Random rnd;
        private BoardSpace[] boardSpaces;
        private int jailPosition;
        private List<Player> players;

        public void PlayGame()
        {
            bool gameIsOver = false;
            while (!gameIsOver) //TODO: fix
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
                    player.MoveTo(jailPosition);
                }
                else
                {
                    player.Move(rollValue1 + rollValue2);
                }
            } while (rollValue1 == rollValue2);
        }
    }
}
