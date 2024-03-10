using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    class NMWarpParticles : NMDuckNetworkEvent
    {
        public float xx;
        public float yy;
        public NMWarpParticles()
        {
        }

        public NMWarpParticles(float x, float y)
        {
            this.xx = x;
            this.yy = y;
        }

        public override void Activate()
        {
            for (int i = 0; i < 15; i++)
            {
                WarpParticle wp = new WarpParticle(this.xx, this.yy);
                Level.Add(wp);
            }
            base.Activate();
        }
    }
}
