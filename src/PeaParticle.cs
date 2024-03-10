using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    class PeaParticle : PhysicsParticle
    {
        public PeaParticle() : base(0f, 0f)
        {
            this.graphic = new Sprite(Mod.GetPath<RSMod>("peaParticle"));
            this.center = new Vec2(1f, 1f);
        }
    }
}
