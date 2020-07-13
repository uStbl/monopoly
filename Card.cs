using System;
using System.Collections.Generic;
using System.Text;

namespace monopoly
{
    class Card : IComparable
    {
        private string name;
        private bool holdable;
        private Action<Player> effect;

        public Card(string name, bool holdable, Action<Player> effect)
        {
            this.name = name;
            this.holdable = holdable;
            this.effect = effect;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            else
                return new Random().Next(-1, 2);
        }

        public void DoEffect(Player player)
        {
            effect(player);
        }
    }
}
