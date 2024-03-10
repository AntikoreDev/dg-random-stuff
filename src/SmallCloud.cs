using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    class SmallCloud : Thing 
    {
        public SpriteMap _sprite;
        public float fallSpeed;
        public float floatSpeed;
        public SmallCloud(float xpos, float ypos) : base(xpos, ypos)
        {
            this._sprite = new SpriteMap(Mod.GetPath<RSMod>("smallCloud"), 16, 16);
            this.graphic = this._sprite;
            this._sprite.frame = 0;
            this.isLocal = true;
            this.collisionSize = new Vec2(16f, 16f);
            this.center = new Vec2(8f, 8f);
            this.collisionOffset = new Vec2(-8f, -8f);
        }

        public override void Initialize()
        {
            this.fallSpeed = Rando.Float(1f, 2f);
            float scale = Rando.Float(0.3f, 0.5f);
            this.xscale = scale;
            this.yscale = scale;
            this._sprite.frame = Rando.Int(0, 3);
            this.floatSpeed = Rando.Float(0.7f, 0.9f);
            base.Initialize();
        }

        public override void Update()
        {
            this.y += this.fallSpeed;
            this.fallSpeed *= this.floatSpeed;
            if (this.fallSpeed < 0.1f)
            {
                this.alpha -= 0.1f;
                if (this.alpha <= 0f)
                {
                    Level.Remove(this);
                }
            }
            base.Update();
        }
    }
}
