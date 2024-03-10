using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    class AmmoPopUp : Thing
    {
        public int _timer;
        public AmmoPopUp(float xpos, float ypos) : base(xpos, ypos)
        {
            this.graphic = new Sprite(GetPath("Ammo"));
            this.center = new Vec2(11f, 4f);
            this.depth = 1f;
            this.isLocal = true;
            this.vSpeed = -0.8f;
        }

        public override void Update()
        {
            base.y += this.vSpeed;
            this._timer++;
            if (this._timer >= 30)
            {
                Level.Remove(this);
            }
        }
    }
}