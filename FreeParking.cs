﻿using System;
using System.Collections.Generic;
using System.Text;

namespace monopoly
{
    class FreeParking : BoardSpace
    {
        public FreeParking()
        {
            name = "Free Parking";
        }

        public override void OnPlayerLanding(Player player)
        {
            return;
        }
    }
}
