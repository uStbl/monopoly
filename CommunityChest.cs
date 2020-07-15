using System;
using System.Collections.Generic;
using System.Text;

namespace monopoly
{
    class CommunityChest : BoardSpace
    {
        private Deck deck;

        public CommunityChest(bool isChest)
        {
            name = "Community Chest";
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
