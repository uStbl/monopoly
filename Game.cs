using System;
using System.Collections.Generic;
using System.Text;

namespace monopoly
{
    class Game
    {
        public const int PassMoney = 200;
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
            foreach (Player p in players)
            {
                p.AddMoney(startingMoney);
                p.MoveTo(goPosition);
                p.SetRemainingJailTurns(0);
            }
            

            rnd = new Random();
        }

        public int GetJailPosition()
        {
            return jailPosition;
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

                if (rollsThisTurn > MaxDoubleRolls && rollValue1 == rollValue2)
                {
                    Console.WriteLine("You were sent to jail for rolling doubles 3 times.");
                    player.MoveTo(jailPosition);
                    player.SetRemainingJailTurns(3);
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
