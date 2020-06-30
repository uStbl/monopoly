using System;
using System.Collections.Generic;
using System.Text;

namespace monopoly
{
    abstract class BoardSpace
    {
        protected string name;
        public abstract void OnPlayerLanding(Player player);

        public string GetName()
        {
            return name;
        }
    }
}
