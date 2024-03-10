using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    [EditorGroup("Random Stuff|Guns|Fire")]
    class RosaArcana : Gun
    {
        public RosaArcana(float xpos, float ypos) : base(xpos, ypos)
        {
            this.graphic = new Sprite(GetPath("rosaArcana"));
            this.ammo = 4;
            this.collisionSize = new Vec2(16f, 9f);
            this.center = new Vec2(7f, 8f);
            this.collisionOffset = new Vec2(-7f, -4f);
            this._editorName = "Rosa Arcana";
            this._barrelOffsetTL = new Vec2(15f, 4f);
        }

        public override void OnPressAction()
        {
            if (ammo > 0)
            {
                if (Network.isActive)
                {
                    this._netShootSound.Play();
                }
                SFX.Play("ignite");
                ammo--;
                base.ApplyKick();
                FireBall f = new FireBall(x,y);
                if (isServerForObject)
                {
                    Level.Add(f);
                }
                f.shooter = null;
                if (this.owner != null)
                { 
                    f.shooter = this.owner;
                }
                f.hSpeed = base.barrelVector.x * 4f;
                f.vSpeed = base.barrelVector.y * 4f;
            }
            else
            {
                this.DoAmmoClick();
            }
        }

        public override void Fire()
        { 
        }

        public StateBinding _netShootSoundBinding = new NetSoundBinding("_netShootSound");
        public NetSoundEffect _netShootSound = new NetSoundEffect(new string[]
        {
            "ignite"
        })
        {
            volume = 1f
        };
    }
}
