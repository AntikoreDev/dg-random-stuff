using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    [BaggedProperty("isFatal", true), EditorGroup("Random Stuff|Guns")]
    class DiscGun : Gun
    {
        public DiscGun(float xval, float yval) : base(xval, yval)
        {
            this.graphic = new Sprite(GetPath("discGun"));
            this.ammo = 3;
            this.center = new Vec2(16f, 16f);
            this.collisionOffset = new Vec2(-8f, -4f);
            this.collisionSize = new Vec2(16f, 9f);
            this._barrelOffsetTL = new Vec2(27f, 14f);
            this._fullAuto = false;
            this._kickForce = 1.7f;
            this._editorName = "Disc Gun";
        }

        public override void OnPressAction()
        {
            if (this.ammo > 0)
            {
                this.ammo--;
                base.ApplyKick();
                Vec2 pos = this.Offset(base.barrelOffset);
                if (Network.isActive)
                {
                    this._netShootSound.Play();
                }
                SFX.Play("barrelThud", 1.5f, 0f, 0f);
                if (isServerForObject)
                {     
                    DiscObject disc = new DiscObject(pos.x, pos.y - 2f);
                    disc.vsp = this.barrelVector.y * 4f;
                    disc.hsp = this.barrelVector.x * 4f;
                    disc.shooter = this.owner as Duck;
                    Level.Add(disc);
                    foreach (MaterialThing mt in Level.CheckRectAll<MaterialThing>(disc.topLeft, disc.topRight).ToList<MaterialThing>())
                    {
                        disc.clip.Add(mt);
                    }
                        
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
            "barrelThud"
        })
        {
            volume = 1f
        };
    }
}
