using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    [EditorGroup("Random Stuff|Guns|Lethal")]
    class Quadastrophic : Gun
    {
        public Quadastrophic(float xval, float yval) : base(xval, yval)
        {
            this.graphic = new Sprite(GetPath("quadastrophic"));
            this.ammo = 3;
            this.loseAccuracy = 0.1f;
            this.maxAccuracyLost = 0.4f;
            this._editorName = "Quadastrophe";
            this._kickForce = 7.5f;
            this.center = new Vec2(6f, 9f);
            this.collisionOffset = new Vec2(-4f, -3f);
            this.collisionSize = new Vec2(12f, 5f);
            this._barrelOffsetTL = new Vec2(14f, 6f);
        }

        public override void Fire()
        {
        }

        public override void OnPressAction()
        {
            if (ammo > 0)
            {
                if (Network.isActive)
                {
                    this._netShootSound.Play();
                }
                SFX.Play("magShot");
                ammo--;
                base.ApplyKick();
                Quadastrophe f = new Quadastrophe(x, y);
                if (isServerForObject)
                {
                    Level.Add(f);
                }
                f.hsp = base.barrelVector.x * 12f;
                f.vsp = base.barrelVector.y * 12f;
            }
            else
            {
                this.DoAmmoClick();
            }
            base.OnPressAction();
        }

        public StateBinding _netShootSoundBinding = new NetSoundBinding("_netShootSound");
        public NetSoundEffect _netShootSound = new NetSoundEffect(new string[]
        {
            "magShot"
        })
        {
            volume = 1f
        };
    }
}
