using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    class EggCrack : PhysicsParticle
    {
        public SpriteMap _sprite;
        public EggCrack() : base(0f, 0f)
        {
            this._sprite = new SpriteMap(GetPath("eggCrack"), 3, 3);
            this.graphic = this._sprite;
            this.center = new Vec2(1.5f, 1.5f);
            this.isLocal = true;
        }

        public override void Initialize()
        {
            this._sprite.frame = Rando.Int(9);
            base.Initialize();
        }
    }
}
