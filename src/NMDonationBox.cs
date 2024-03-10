using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    class NMDonationBox : NMDuckNetworkEvent
    {
        public Coin coin;
        public NMDonationBox()
        {
        }

        public NMDonationBox(Coin c)
        {
            this.coin = c;
        }

        public override void Activate()
        {
            if (coin != null)
            {
                Level.Remove(coin);
            }
            base.Activate();
        }
    }
}
