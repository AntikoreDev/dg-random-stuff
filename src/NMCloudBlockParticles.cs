using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    class NMCloudBlockParticles : NMDuckNetworkEvent
    {
        public Thing thing;
        public NMCloudBlockParticles()
        {
        }

        public NMCloudBlockParticles(Thing t)
        {
            this.thing = t;
        }

        public override void Activate()
        {
            if (thing != null)
            {
                for (int i = 0; i < 15; i++)
                {
                    float yy = thing.bottom;
                    float xx = Rando.Float(thing.bottomLeft.x, thing.topRight.x);
                    SmallCloud sc = new SmallCloud(xx, yy);
                    Level.Add(sc);
                }
            } 
            base.Activate();
        }
    }
}
