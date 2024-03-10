using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    class DonationPopup : Thing
    {
        public SpriteMap _sprite;
        public int life;
        public DonationPopup(float xpos, float ypos) : base(xpos, ypos)
        {
            this._sprite = new SpriteMap(Mod.GetPath<RSMod>("donationPopup"), 8, 9);
            this.graphic = this._sprite;
            this._sprite.AddAnimation("Default", 0.4f, true, new int[]
            {
                0,
                1
            });
            this._sprite.CenterOrigin();
            this.depth = 3f;
        }

        public override void Initialize()
        {
            if (!(Level.current is Editor))
            {
                this._sprite.SetAnimation("Default");
            }
            base.Initialize();
        }

        public override void Update()
        {
            this.y -= 0.8f;
            if (this.life >= 60)
            {
                Level.Remove(this);
            }
            this.life++;
            base.Update();
        }

        public StateBinding _positionBinding = new StateBinding("position", -1, false, false);
    }
}
