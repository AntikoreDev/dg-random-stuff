using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    class NMThrowItem : NMDuckNetworkEvent
    {
        public Duck duck;
        public NMThrowItem()
        {
        }

        public NMThrowItem(Duck d)
        {
            this.duck = d;
        }

        public override void Activate()
        {
            if (duck != null)
            {
                duck.ThrowItem();
            }
            base.Activate();
        }
    }
}
