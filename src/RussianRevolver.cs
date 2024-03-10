using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    [EditorGroup("Random Stuff|Guns")]
    class RussianRevolver : Gun
    {
        private SpriteMap clickPuff;
        private bool _doPuff;
        public RussianRevolver(float xval, float yval) : base(xval, yval)
		{
            this.ammo = 1;
            this._ammoType = new ATMagnum();
            this._type = "gun";
            this.graphic = new Sprite(GetPath("russianRevolver"), 0f, 0f);
            this.center = new Vec2(6f, 7f);
            this.collisionOffset = new Vec2(-5f, -7f);
            this.collisionSize = new Vec2(18f, 11f);
            this._barrelOffsetTL = new Vec2(21f, 3f);
            this._fireSound = "magnum";
            this._kickForce = 4f;
            this._holdOffset = new Vec2(-2f, 1f);
            this._editorName = "Russian Revolver";
            this.clickPuff = new SpriteMap("clickPuff", 16, 16, false);
            this.clickPuff.AddAnimation("puff", 0.3f, false, new int[]
            {
                0,
                1,
                2,
                3
            });
            this.clickPuff.center = new Vec2(0f, 12f);
        }

        public override void OnPressAction()
        {
            if (ammo > 0)
            {
                if (Rando.Int(1,4) == 4)
                {
                    base.Fire();
                }
                else
                {
                    this.FailShoot();
                }
            }
            else
            {
                this.DoAmmoClick();
            }
            base.OnPressAction();
        }

        public override void Initialize()
        {
            if (Network.isActive)
            {
                Rando.generator = new Random(NetRand.currentSeed);
            }
            base.Initialize();
        }

        public override void Fire()
        {
        }

        public void FailShoot()
        {
            this._doPuff = true;
            this.clickPuff.frame = 0;
            this.clickPuff.SetAnimation("puff");
            if (Network.isActive)
            {
                this._netFailSound.Play();
            }
            SFX.Play("littleGun");
        }

        public override void Draw()
        {
            base.Draw();
            if (this._doPuff)
            {
                this.clickPuff.alpha = 0.6f;
                this.clickPuff.angle = this.angle + this._smokeAngle;
                this.clickPuff.flipH = (this.offDir < 0);
                base.Draw(this.clickPuff, this.barrelOffset, 1);
            }
        }

        public override void Update()
        {
            base.Update();
            if (this.clickPuff.finished)
            {
                this._doPuff = false;
            }
        }

        public StateBinding _netFailSoundBinding = new NetSoundBinding("_netFailSound");
        public NetSoundEffect _netFailSound = new NetSoundEffect(new string[]
        {
            "littleGun"
        })
        {
            volume = 1f
        };
    }
}
