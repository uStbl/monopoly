using System;
using System.Collections.Generic;
using System.Text;

namespace monopoly
{
    class Card
    {
        private string name;
        private bool holdable;
        private Deck containingDeck;
        private Action<Player> effect;
        private Random rnd;

        public Card(string name, bool holdable, Deck containingDeck, Action<Player> effect)
        {
            this.name = name;
            this.holdable = holdable;
            this.containingDeck = containingDeck;
            this.effect = effect;
            rnd = new Random();
        }

        public bool IsHoldable()
        {
            return holdable;
        }

        public string GetName()
        {
            return name;
        }

        public void DoEffect(Player player)
        {
            effect(player);
        }

        public Deck GetDeck()
        {
            return containingDeck;
        }
    }
}
