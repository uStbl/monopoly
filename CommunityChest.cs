using System;
using System.Collections.Generic;
using System.Text;

namespace monopoly
{
    class CommunityChest : BoardSpace
    {
        private int goPosition;
        private int jailPosition;
        private Player[] players;
        private Card[] cards;
        private int deckIncrement;

        public CommunityChest()
        {
            cards = new Card[]
            {
                new Card("Advance to GO", false, player => {player.MoveTo(goPosition);}),
                new Card("Bank error in your favor - Collect $200", false, player => {player.AddMoney(200);}),
                new Card("Doctor's fees - Pay $50", false, player => {player.AddMoney(-50);}),
                new Card("From sale of stock you get $50", false, player => {player.AddMoney(50);}),
                new Card("Get out of jail free", true, player => {}),
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

            deckIncrement = 0;
            Array.Sort(cards);
        }

        public void setGoPosition(int goPosition)
        {
            this.goPosition = goPosition;
        }

        public void setJailPosition(int jailPosition)
        {
            this.jailPosition = jailPosition;
        }

        public void setPlayers(Player[] players)
        {
            this.players = players;
        }

        public override void OnPlayerLanding(Player player)
        {
            cards[deckIncrement].DoEffect(player);
            if (deckIncrement < cards.Length - 1)
                deckIncrement++;
            else
                deckIncrement = 0;
        }
    }
}
