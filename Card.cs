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
        private Random rnd;

        public Card(string name, bool holdable, Action<Player> effect)
        {
            this.name = name;
            this.holdable = holdable;
            this.effect = effect;
            rnd = new Random();
        }

        public bool GetHoldable()
        {
            return holdable;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            else
                return rnd.Next(-1, 2);
        }

        public void DoEffect(Player player)
        {
            Console.WriteLine("You picked the card: {0}", name);
            effect(player);
        }
    }
}
