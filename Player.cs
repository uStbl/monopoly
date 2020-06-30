using System;
using System.Collections.Generic;
using System.Text;

namespace monopoly
{
    class Player
    {
        private int id; // Must be between 1 - # of players in the game
        private int money; // Must be >= 0
        private int position; // TODO define bounds

        public int getId() {
            return id;
        }

        public int getMoney()
        {
            return money;
        }

        public void setMoney(int money)
        {
            this.money = money;
        }
    }
}
