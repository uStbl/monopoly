using System;
using System.Collections.Generic;
using System.Text;

namespace monopoly
{
    class Deck
    {
        private List<Card> cards;
        private Game containingGame;

        public Deck(bool isChest, Game containingGame)
        {
            this.containingGame = containingGame;
            if (isChest)
            {
                cards = new List<Card>
                {
                    new Card("Advance to GO", false, this, player => {AdvanceTo(player, "GO");}),
                    new Card("Bank error in your favor - Collect $200", false, this, player => {player.AddMoney(200);}),
                    new Card("Doctor's fees - Pay $50", false, this, player => {player.AddMoney(-50);}),
                    new Card("From sale of stock you get $50", false, this, player => {player.AddMoney(50);}),
                    new Card("Get out of jail free", true, this,
                        player => 
                        {
                            Console.WriteLine("You used a get out of jail free card and broke out of jail!");
                            player.SetRemainingJailTurns(0);
                            containingGame.NormalTurn(player);
                        }),
                    new Card("Go to jail - Do not pass GO - Do not collect $" + containingGame.GetPassMoney(), false, this,
                        player =>
                        {
                            Console.WriteLine("You were sent to jail!");
                            Console.WriteLine("You must remain in jail for {0} more turns.", Game.JailTurns);
                            player.MoveTo(containingGame.FindPosition("Jail"), false, false);
                            player.SetRemainingJailTurns(Game.JailTurns);
                        }),
                    new Card("Grand opera night - Collect $50 from every player for opening night seats", false, this,
                        player =>
                        {
                            foreach(Player p in containingGame.GetPlayers()) p.AddMoney(-50, false);
                            player.AddMoney(containingGame.GetPlayers().Length * 50);
                        }),
                    new Card("Holiday fund matures - Receive $100", false, this, player => {player.AddMoney(100);}),
                    new Card("Income tax refund - Collect $20", false, this, player => {player.AddMoney(20);}),
                    new Card("Life insurance matures - Collect $100", false, this, player => {player.AddMoney(100);}),
                    new Card("Pay hospital fees of $100", false, this, player => {player.AddMoney(-100);}),
                    new Card("Pay school fees of $150", false, this, player => {player.AddMoney(-150);}),
                    new Card("Receive $25 consultancy fee", false, this, player => {player.AddMoney(25);}),
                    new Card("You are assessed for street repairs – $40 per house – $115 per hotel", false, this,
                        player =>
                        {
                            int repairTotal = 0;
                            foreach(Property p in player.GetProperties())
                            {
                                int houseCount = p.GetHouses();
                                if (houseCount < 5)
                                    repairTotal += houseCount * 40;
                                else
                                    repairTotal += 115;
                            }
                            player.AddMoney(-repairTotal);
                        }),
                    new Card("You have won second prize in a beauty contest - Collect $10", false, this, player => {player.AddMoney(10);}),
                    new Card("You inherit $100", false, this, player => {player.AddMoney(100);}),
                };
            }
            else
            {
                cards = new List<Card>
                {
                    new Card("Advance to GO", false, this, player => {AdvanceTo(player, "GO");}),
                    new Card("Advance to Illinois Ave. - If you pass GO, collect $200", false, this, player => {AdvanceTo(player, "Illinois Avenue");}),
                    new Card("Advance to St. Charles Place - If you pass GO, collect $200", false, this, player => {AdvanceTo(player, "St. Charles Place");}),
                    new Card("Advance token to nearest utility", false, this,
                        player =>
                        {
                            int waterWorks = containingGame.FindPosition("Water Works");
                            int electricCompany = containingGame.FindPosition("Electric Company");
                            if (player.DistanceTo(waterWorks) < player.DistanceTo(electricCompany))
                                AdvanceTo(player, "Water Works");
                            else
                                AdvanceTo(player, "Electric Company");
                        }),
                    new Card("Advance token to the nearest railroad and pay owner double rent", false, this,
                        player =>
                        {
                            string[] railroadPositions =
                            {
                                "Reading Railroad",
                                "Pennsylvania Railroad",
                                "B. & O. Railroad",
                                "Short Line"
                            };

                            string nearestRailroad = "Reading Railroad";
                            foreach (string s in railroadPositions){
                                if (player.DistanceTo(containingGame.FindPosition(s)) < player.DistanceTo(containingGame.FindPosition(nearestRailroad)))
                                    nearestRailroad = s;
                            }

                            player.MoveTo(containingGame.FindPosition(nearestRailroad));
                            Railroad target = (Railroad)containingGame.BoardSpaceAt(player.GetPosition());
                            target.SetRent(2 * target.GetRent());
                            containingGame.BoardSpaceAt(player.GetPosition()).OnPlayerLanding(player);
                            target.SetRent(target.GetRent() / 2);
                        }),
                    new Card("Bank pays you dividend of $50", false, this, player => { player.AddMoney(50); }),
                    new Card("Get out of jail free", true, this,
                        player =>
                        {
                            Console.WriteLine("You used a get out of jail free card and broke out of jail!");
                            player.SetRemainingJailTurns(0);
                            containingGame.NormalTurn(player);
                        }),
                    new Card("Go back 3 spaces", false, this, player => { player.Move(-3); }),
                    new Card("Go to jail - Do not pass GO - Do not collect $" + containingGame.GetPassMoney(), false, this,
                        player =>
                        {
                            Console.WriteLine("You were sent to jail!");
                            Console.WriteLine("You must remain in jail for {0} more turns.", Game.JailTurns);
                            player.MoveTo(containingGame.FindPosition("Jail"), false, false);
                            player.SetRemainingJailTurns(Game.JailTurns);
                        }),
                    new Card("Make general repairs on all your property - $25 per house - $100 per hotel", false, this,
                        player =>
                        {
                            int repairTotal = 0;
                            foreach (Property p in player.GetProperties())
                            {
                                int houseCount = p.GetHouses();
                                if (houseCount < 5)
                                    repairTotal += houseCount * 25;
                                else
                                    repairTotal += 100;
                            }
                            player.AddMoney(-repairTotal);
                        }),
                    new Card("Pay poor tax of $15", false, this, player => { player.AddMoney(-15); }),
                    new Card("Take a walk on the Boardwalk – Advance token to Boardwalk", false, this, player => { AdvanceTo(player, "Boardwalk"); }),
                    new Card("Take a trip to Reading Railroad – If you pass GO, collect $200", false, this, player => { AdvanceTo(player, "Reading Railroad"); }),
                    new Card("You have been elected Chairman of the Board – Pay each player $50", false, this,
                    player =>
                    {
                        foreach (Player p in containingGame.GetPlayers()) p.AddMoney(50, false);
                        player.AddMoney(containingGame.GetPlayers().Length * -50);
                    }),
                    new Card("Your building and loan matures – Collect $150", false, this, player => { player.AddMoney(150); }),
                    new Card("You have won a crossword competition - Collect $100", false, this, player => { player.AddMoney(100); }),
                };
            }

            Shuffle(cards);
        }

        private void Shuffle(List<Card> deck)
        {
            Random rnd = new Random();

            for (int n = deck.Count - 1; n > 0; n--)
            {
                int k = rnd.Next(n + 1);
                Card temp = deck[k];
                deck[k] = deck[n];
                deck[n] = temp;
            }
        }

        private void AdvanceTo(Player player, string spaceName)
        {
            player.MoveTo(containingGame.FindPosition(spaceName));
            containingGame.BoardSpaceAt(player.GetPosition()).OnPlayerLanding(player);
        }

        // Return the top card in the deck.
        public Card Peek()
        {
            return cards[0];
        }

        // Move the top card to the bottom of the deck and return it.
        public Card Cycle()
        {
            Card topCard = cards[0];
            Console.WriteLine("You picked the card: {0}.", topCard.GetName());
            cards.Remove(topCard);
            cards.Add(topCard);
            return topCard;
        }

        // Remove the top card from the deck and return it.
        public Card Take()
        {
            Card topCard = cards[0];
            Console.WriteLine("You picked the card: {0}. You get to keep this card!", topCard.GetName());
            cards.Remove(topCard);
            return topCard;
        }

        // Add a card to the bottom of the deck.
        public void AddCard(Card card)
        {
            cards.Add(card);
        }
    }
}
