using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    class WarpParticle : Thing
    {
        public float vsp;
        public float hsp;
        public float scaleRemoverFactor;
        public float rotationSpeed;
        public WarpParticle(float xpos, float ypos) : base(xpos, ypos)
        {
            this.graphic = new Sprite(Mod.GetPath<RSMod>("warpParticle"));
            this.center = new Vec2(2, 2);
            this.isLocal = true;
            this.collisionOffset = new Vec2(-2, -2);
            this.collisionSize = new Vec2(4, 4);
        }

        public override void Initialize()
        {
            this.vsp = Rando.Float(-1f, 1f);
            this.hsp = Rando.Float(-1f, 1f);
            float i = Rando.Float(1f, 2f);
            this.xscale = i;
            this.yscale = i;
            this.scaleRemoverFactor = Rando.Float(0.05f, 0.1f);
            this.rotationSpeed = Rando.Float(-3f, 3f);
            this.angle = Rando.Float(0f, 89f);
            base.Initialize();
        }
        public override void Update()
        {
            if (this.isServerForObject)
            {
                this.angle += this.rotationSpeed;
                this.x += this.hsp;
                this.y += this.vsp;
                this.xscale -= this.scaleRemoverFactor;
                this.yscale -= this.scaleRemoverFactor;
                if (this.xscale < 0f)
                {
                    Level.Remove(this);
                }
                base.Update();
            }
        }
    }
}
