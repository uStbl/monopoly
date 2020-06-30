using System;
using System.Collections.Generic;
using System.Text;

namespace monopoly
{
    abstract class BoardSpace
    {
        protected string name;
        public abstract void onPlayerLanding(Player player);

        public string getName()
        {
            return name;
        }
    }
}
