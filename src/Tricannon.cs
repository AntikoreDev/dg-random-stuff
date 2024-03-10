using System;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using DuckGame;

namespace DuckGame.RSModDEV
{
    [EditorGroup("Random Stuff|Guns")]
    class Tricannon : Gun
    {
        private float adif = 25f;
        private bool hasShoot;
        private SpriteMap _shootLight;
        public Tricannon(float xval, float yval) : base(xval, yval)
        {
            this.ammo = 12;
            this._ammoType = new ATMag();
            this._ammoType.range = 250f;
            this._barrelAngleOffset = -adif;
            this._ammoType.accuracy = 1f;
            this.graphic = new Sprite(Mod.GetPath<RSMod>("tricannon"), 0f, 0f);
            this.center = new Vec2(8f, 10f);
            this.collisionOffset = new Vec2(-7f, -7f);
            this._kickForce = 5f;
            this.collisionSize = new Vec2(18f, 10f);
            this._shootLight = new SpriteMap(Mod.GetPath<RSMod>("shootFlare"), 9, 9, false);
            this._shootLight.AddAnimation("idle", 0.25f, false, new int[]
            {
                0,
                1,
            });
            this._shootLight.center = new Vec2(4f, 4f);
            this._barrelOffsetTL = new Vec2(18f, 6f);
            this._fireSound = "magShot";
        }

        public override void OnPressAction()
        {
            if (ammo > 0)
            {
                this.hasShoot = true;
                this._shootLight.SetAnimation("idle");
                for (int i = 0; i < 3; i++)
                {
                    this.Fire();
                    this._barrelAngleOffset += adif;
                }
                this._barrelAngleOffset = -adif;
                this.ApplyKick();
                this.ammo--;
            }
            else
            {
                this.DoAmmoClick();
            }
        }

        public override void Fire()
        {
            float num = base.angleDegrees;
            if (this.offDir < 0)
            {
                num += 180f;
                num -= this._ammoType.barrelAngleDegrees;
            }
            else
            {
                num += this._ammoType.barrelAngleDegrees;
            }
            num += this._barrelAngleOffset;
            Bullet bullet = this._ammoType.FireBullet(this.Offset(this.barrelOffset), this.owner, num, this);
            if (Network.isActive && base.isServerForObject)
            {
                this.firedBullets.Add(bullet);
                if (base.duck != null && base.duck.profile.connection != null)
                {
                    bullet.connection = base.duck.profile.connection;
                }
            }
            this.bulletFireIndex += 1;
            this._barrelHeat += 0.3f;
            this.firing = true;
            this._wait = this._fireWait;
            this.PlayFireSound();
            if (this.owner == null)
            {
                Vec2 vec3 = this.barrelVector * Rando.Float(1f, 3f);
                vec3.y += Rando.Float(2f);
                this.hSpeed -= vec3.x;
                this.vSpeed -= vec3.y;
            }
        }

        public override void Draw()
        { 
            if (hasShoot)
            {
                base.Draw(this._shootLight, new Vec2(15f, -4f), 1);
                if (this._shootLight.finished)
                {
                    this.hasShoot = false;
                }
            }
            else
            {
                this._shootLight.SetAnimation("idle");
                this._shootLight.frame = 0;
            }
            base.Draw();
        }
    }
}
