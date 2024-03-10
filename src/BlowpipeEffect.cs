using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    class BlowpipeEffect : Thing
    {
        public Duck duckOwner;
        public int framesAlive;
        public float dx;
        public float dy;
        public BlowpipeEffect(float xpos, float ypos, Duck duck) : base(xpos, ypos)
        {
            this.duckOwner = duck;
        }

        public override void Update()
        {
            base.Update();
            this.framesAlive++;
            if (this.duckOwner == null || !(this.duckOwner is Duck))
            {
                Level.Remove(this);
                return;
            }
            this.ShakeDuck();
            if (this.framesAlive > 60)
            {
                this.duckOwner.vSpeed = -3f;
                this.duckOwner.hSpeed = Rando.Float(2f, -2f);
                (this.duckOwner as Duck).GoRagdoll();
                (this.duckOwner as Duck).immobilized = false;
                Level.Remove(this);
            }
        }
        public void ShakeDuck()
        {
            if (isServerForObject)
            {
                this.duckOwner.position = new Vec2(this.dx + Rando.Int(-2, 2), this.dy + Rando.Int(-2, 2));
            }
        }
    }
}
