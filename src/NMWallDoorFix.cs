using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    class NMWallDoorFix : NMDuckNetworkEvent
    {
        public Duck duck;
        public Vec2 position;
        public NMWallDoorFix()
        {
        }

        public NMWallDoorFix(Duck d, Vec2 pos)
        {
            this.duck = d;
            this.position = pos;
        }

        public override void Activate()
        {
            if (duck != null && position != null)
            {
                duck.immobilized = false;
                duck.sleeping = false;
                duck.position = position;
            }
            base.Activate();
        }
    }
}
