using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    [EditorGroup("Random Stuff|Guns")]
    class Guitar : Gun
    {
        public Guitar(float xval, float yval) : base(xval, yval)
        {
            this.graphic = new Sprite(GetPath("guitar"));
            this.ammo = 1;
            this._ammoType = new ATGuitar();
            this.center = new Vec2(12f, 4f);
            this.collisionOffset = new Vec2(-12f, -4f);
            this.collisionSize = new Vec2(27f, 10f);
            this._barrelOffsetTL = new Vec2(25f, 4f);
            this._fullAuto = false;
            this._kickForce = 1.7f;
            this._fireSound = Mod.GetPath<RSMod>("SFX\\guitarShoot"); //Fire Sound
            this._editorName = "Guitar";
        }

        public override void Initialize()
        {
            if (Network.isActive)
            {
                Rando.generator = new Random(NetRand.currentSeed);
            }
            base.Initialize();
        }

        public override void OnPressAction()
        {
            if (this.ammo > 0)
            {
                if (Rando.Int(1, 10) == 10 && !this.infinite)
                {
                    if (Network.isActive)
                    {
                        this._netBreakSound.Play();
                    }
                    SFX.Play(Mod.GetPath<RSMod>("SFX\\guitarString"), 0.75f);
                    this.ammo--;
                }
                else
                {
                    this.ammo++;
                    this.Fire();
                    /*
                    this.ApplyKick();   
                    if (Network.isActive)
                    {
                        this._netShootSound.Play();
                    }
                    SFX.Play(Mod.GetPath<RSMod>("SFX\\guitarShoot"));
                    GuitarSoundwave g = new GuitarSoundwave(x, y);
                    Level.Add(g);
                    g.shooter = null;
                    if (this.owner != null)
                    {
                        g.shooter = this.owner as Duck;
                    }
                    g.angle = this.angle;
                    if (this.offDir == -1)
                    {
                        g.angleDegrees -= 180f;
                    }
                    g.hsp = base.barrelVector.x * 8f;
                    g.vsp = base.barrelVector.y * 8f;
                    */
               }
            }
            else
            {
                if (Network.isActive)
                {
                    this._netNoAmmoSound.Play();
                }
                SFX.Play("presentLand");
            }
        }

        /*
        public override void Fire()
        {
        }
        */

        public StateBinding _netShootSoundBinding = new NetSoundBinding("_netShootSound");
        public NetSoundEffect _netShootSound = new NetSoundEffect(new string[]
        {
            Mod.GetPath<RSMod>("SFX\\guitarShoot")
        })
        {
            volume = 1f
        };
        public StateBinding _netNoAmmoSoundBinding = new NetSoundBinding("_netNoAmmoSound");
        public NetSoundEffect _netNoAmmoSound = new NetSoundEffect(new string[]
        {
            "presentLand"
        })
        {
            volume = 1f
        };
        public StateBinding _netBreakSoundBinding = new NetSoundBinding("_netBreakSound");
        public NetSoundEffect _netBreakSound = new NetSoundEffect(new string[]
        {
            Mod.GetPath<RSMod>("SFX\\guitarString")
        })
        {
            volume = 0.75f
        };
    }
}
