using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    class Club : Gun
    {
        public int framesSincePush;
        public Club(float xpos, float ypos) : base(xpos, ypos)
        {
            this.graphic = new Sprite(Mod.GetPath<RSMod>("club"));
            this.center = new Vec2(4f, 21f);
            this.collisionOffset = new Vec2(-2f, -16f);
            this.collisionSize = new Vec2(4f, 18f);
            this.weight = 0.9f;
        }

        public override void CheckIfHoldObstructed()
        {
            Duck duckOwner = this.owner as Duck;
            if (duckOwner != null)
            {
                duckOwner.holdObstructed = false;
            }
        }

        public override void Update()
        {
            base.Update();
            if (this.owner != null)
            {
                this.center = new Vec2(4f, 21f);
                this._framesSinceThrown = 0;
                if ((this.owner as Duck).action)
                {
                    this.framesSincePush++;
                }
            }
        }
    }
}
