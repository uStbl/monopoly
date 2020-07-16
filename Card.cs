using System;
using System.Collections.Generic;
using System.Text;

namespace monopoly
{
    class Card
    {
        private string name;
        private bool holdable;
        private Action<Player> effect;
        private Random rnd;

        public Card(string name, bool holdable, Action<Player> effect)
        {
            this.name = name;
            this.holdable = holdable;
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
    }
}
