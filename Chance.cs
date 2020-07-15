using System;
using System.Collections.Generic;
using System.Text;

namespace monopoly
{
    class Chance : BoardSpace
    {
        private Deck deck;

        public Chance(bool isChest)
        {
            name = "Chance";
        }

        public void SetDeck(Deck deck)
        {
            this.deck = deck;
        }

        public override void OnPlayerLanding(Player player)
        {
            if (deck.Peek().IsHoldable())
                player.AddCard(deck.Take());
            else
                deck.Cycle().DoEffect(player);
        }
    }
}
