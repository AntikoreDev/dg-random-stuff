using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    [BaggedProperty("isFatal", false), EditorGroup("Random Stuff|Guns")]
    class BasquePistol : Gun
    {
        public BasquePistol(float xval, float yval) : base(xval, yval)
        {
            this.graphic = new Sprite(GetPath("basquePistol"));
            this.ammo = 10;
            this.center = new Vec2(16f, 16f);
            this.collisionOffset = new Vec2(-8f, -4f);
            this.collisionSize = new Vec2(16f, 9f);
            this._barrelOffsetTL = new Vec2(27f, 14f);
            this._fullAuto = false;
            this._kickForce = 3f;
            this._editorName = "Basque Pistol";
            this._barrelSteam = new SpriteMap("steamPuff", 16, 16, false);
            this._barrelSteam.center = new Vec2(0f, 14f);
            this._barrelSteam.AddAnimation("puff", 0.4f, false, new int[]
            {
                0, 
                1,
                2,
                3, 
                4,
                5,
                6,
                7
            });
            this._barrelSteam.SetAnimation("puff");
            this._barrelSteam.speed = 0f;
        }

        public override void Update()
        {
            if (this._barrelSteam.speed > 0f && this._barrelSteam.finished)
            {
                this._barrelSteam.speed = 0f;
            }
            base.Update();
        }

        public override void Draw()
        {
            base.Draw();
            if (this._barrelSteam.speed > 0f)
            {
                this._barrelSteam.alpha = 0.6f;
                base.Draw(this._barrelSteam, new Vec2(9f, 1f), 1);
            }
        }

        public override void OnPressAction()
        {
            if (this.ammo > 0)
            {
                this.ammo--;
                this._barrelSteam.speed = 1f;
                this._barrelSteam.frame = 0;
                base.ApplyKick();
                Vec2 pos = this.Offset(base.barrelOffset);
                if (Network.isActive)
                {
                    this._netShootSound.Play();
                }
                SFX.Play("netGunFire", 1f, 0f, 0f, false);
                if (isServerForObject)
                {
                    Rock rock = new Rock(pos.x, pos.y - 2f);
                    rock.vSpeed = this.barrelVector.y * 7.5f;
                    rock.hSpeed = this.barrelVector.x * 7.5f;
                    Level.Add((Thing)rock);
                }
            }
            else
            {
                base.DoAmmoClick();
            }
        }

        public override void Fire()
        {
        }

        public StateBinding _netShootSoundBinding = new NetSoundBinding("_netShootSound");
        public NetSoundEffect _netShootSound = new NetSoundEffect(new string[]
        {
            "netGunFire"
        })
        {
            volume = 1f
        };

        private SpriteMap _barrelSteam;
    }
}
