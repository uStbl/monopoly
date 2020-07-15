using System;
using System.Collections.Generic;
using System.Text;

namespace monopoly
{
    class ChestChance : BoardSpace
    {
        private bool isChest;
        private static Player[] players;
        private static List<Card> chestCards;
        private static List<Card> chanceCards;
        private static int chestDeckPosition;
        private static int chanceDeckPosition;

        public ChestChance(bool isChest)
        {
            if (isChest)
                name = "Community Chest";
            else
                name = "Chance";

            this.isChest = isChest;
        }

        public static void InitializeDecks()
        {
            chestCards = new List<Card>
            {
                new Card("Advance to GO", false, player => {player.MoveTo(containingGame.FindPosition("GO"));}),
                new Card("Bank error in your favor - Collect $200", false, player => {player.AddMoney(200);}),
                new Card("Doctor's fees - Pay $50", false, player => {player.AddMoney(-50);}),
                new Card("From sale of stock you get $50", false, player => {player.AddMoney(50);}),
                new Card("Get out of jail free", true, player => {player.SetRemainingJailTurns(0);}),
                new Card("Go to jail", false, player => {player.MoveTo(jailPosition);}),
                new Card("Grand opera night - Collect $50 from every player for opening night seats", false,
                player =>
                {
                    foreach(Player p in players) p.AddMoney(-50, false);
                    player.AddMoney(players.Length * 50);
                }),
                new Card("Holiday fund matures - Receive $100", false, player => {player.AddMoney(100);}),
                new Card("Income tax refund - Collect $20", false, player => {player.AddMoney(20);}),
                new Card("Life insurance matures - Collect $100", false, player => {player.AddMoney(100);}),
                new Card("Pay hospital fees of $100", false, player => {player.AddMoney(-100);}),
                new Card("Pay school fees of $150", false, player => {player.AddMoney(-150);}),
                new Card("Receive $25 consultancy fee", false, player => {player.AddMoney(25);}),
                new Card("You are assessed for street repairs – $40 per house – $115 per hotel", false,
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
                new Card("You have won second prize in a beauty contest - Collect $10", false, player => {player.AddMoney(10);}),
                new Card("You inherit $100", false, player => {player.AddMoney(100);}),
            };

            chanceCards = new List<Card>
            {
                new Card("Advance to GO", false, player => {player.MoveTo(goPosition);}),
                new Card("Advance to Illinois Ave. - If you pass GO, collect $200", false, player => {player.MoveTo();}),
                new Card("Advance to St. Charles Place - If you pass GO, collect $200", false, player => {player.MoveTo();}),
                new Card("Advance token to nearest utility", false, player => {player.MoveTo();}),
                new Card("Advance token to the nearest railroad and pay owner double rent", false, player => {player.MoveTo();}),
                new Card("Bank pays you dividend of $50", false, player => {player.AddMoney(50);}),
                new Card("Get out of jail free", true, player => {player.MoveTo();}),
                new Card("Go back 3 spaces", false, player => {player.Move(-3);}),
                new Card("Go to jail", false, player => {player.MoveTo(jailPosition);}),
                new Card("Make general repairs on all your property - $25 per house - $100 per hotel", false, player => {player.MoveTo();}),
                new Card("Pay poor tax of $15", false, player => {player.AddMoney(-15);}),
                new Card("Take a walk on the Boardwalk – Advance token to Boardwalk", false, player => {player.MoveTo();}),
                new Card("Take a trip to Reading Railroad – If you pass GO, collect $200", false, player => {player.MoveTo();}),
                new Card("You have been elected Chairman of the Board – Pay each player $50", false,
                player =>
                {
                    foreach(Player p in players) p.AddMoney(50, false);
                    player.AddMoney(players.Length * -50);
                }),
                new Card("Your building and loan matures – Collect $150", false, player => {player.AddMoney(150);}),
                new Card("You have won a crossword competition - Collect $100", false, player => {player.AddMoney(100);}),
            };

            chestCards.Sort();
            chanceCards.Sort();
            chestDeckPosition = 0;
            chanceDeckPosition = 0;
        }

        public static void InitializeFields(Player[] players, int goPosition, int jailPosition, ) { 
        
        }

        public static void SetGoPosition(int goPosition)
        {
            ChestChance.goPosition = goPosition;
        }

        public static void SetJailPosition(int jailPosition)
        {
            ChestChance.jailPosition = jailPosition;
        }

        public static void SetPlayers(Player[] players)
        {
            ChestChance.players = players;
        }

        public override void OnPlayerLanding(Player player)
        {
            if (isChance)
            {
                Card currentCard = chanceCards[chanceDeckPosition];
                if (currentCard.GetHoldable())
                {
                    player.AddCard(currentCard);
                    chanceCards.Remove(currentCard);
                    if (chanceDeckPosition >= chanceCards.Count)
                        chanceDeckPosition = 0;
                }
                else
                {
                    currentCard.DoEffect(player);
                    if (chanceDeckPosition < chanceCards.Count - 1)
                        chanceDeckPosition++;
                    else
                        chanceDeckPosition = 0;
                }
            }
            else
            {
                Card currentCard = chestCards[chestDeckPosition];
                if (currentCard.GetHoldable())
                {
                    player.AddCard(currentCard);
                    chestCards.Remove(currentCard);
                    if (chestDeckPosition >= chestCards.Count)
                        chestDeckPosition = 0;
                }
                else
                {
                    currentCard.DoEffect(player);
                    if (chestDeckPosition < chestCards.Count - 1)
                        chestDeckPosition++;
                    else
                        chestDeckPosition = 0;
                }
            }
        }
    }
}
