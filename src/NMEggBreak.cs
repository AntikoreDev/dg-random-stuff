using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{ 
    class NMEggBreak : NMDuckNetworkEvent
    {
        public float x;
        public float y;
        public NMEggBreak()
        {
        }

        public NMEggBreak(float xx, float yy)
        {
            this.x = xx;
            this.y = yy;
        }

        public override void Activate()
        {
            for (int i = 0; i < 15; i++)
            {
                EggCrack e = new EggCrack();
                e.x = this.x;
                e.y = this.y;
                e.vSpeed = Rando.Float(-1f, 1f);
                e.hSpeed = Rando.Float(-1f, 1f);
                Level.Add(e);
            }
            base.Activate();
        }


    }
}
