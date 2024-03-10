using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    class NMGravelFall : NMDuckNetworkEvent
    {
        public Gravel gravel;
        public NMGravelFall()
        {
        }

        public NMGravelFall(Gravel g)
        {
            this.gravel = g;
        }
        public override void Activate()
        {
            if (gravel != null)
            {
                gravel._sprite.frame = 1;
                gravel.touched = true;
                gravel.alpha = 1f;
            }
            base.Activate();
        }
    }
}
