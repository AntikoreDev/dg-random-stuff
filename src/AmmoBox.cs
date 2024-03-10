using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    [BaggedProperty("canSpawn", false), EditorGroup("Random Stuff|Misc")]
    class AmmoBox : Holdable
    {
        public AmmoBox(float xpos, float ypos) : base(xpos, ypos)
        {
            this.graphic = new Sprite(GetPath("ammoBox"));
            this.center = new Vec2(8f, 11f);
            this.collisionSize = new Vec2(14f, 10f);
            this.collisionOffset = new Vec2(-7f, -5f);
            this.canPickUp = false;
            this.gravMultiplier = 1f;
            this.depth = -0.4f;
            this._editorName = "Ammo Box";
        }

        public override void Update()
        {
            List<Duck> ducks = Level.CheckRectAll<Duck>(new Vec2(this.topLeft.x + 2f, this.topLeft.y + 2f), new Vec2(this.bottomRight.x - 2f, this.bottomRight.y - 2f)).ToList<Duck>();
            if (ducks.Count > 0)
            {
                foreach (Duck d in ducks)
                {
                    if (d != null && d.holdObject != null && d.holdObject is Gun)
                    {
                        Gun g = d.holdObject as Gun;
                        if (!g.infinite)
                        {
                            this.Reload(d, g);
                            continue;
                        }
                    }
                }
            }
            base.Update();
        }

        public void Reload(Duck d, Gun g)
        {  
            Gun gun = Activator.CreateInstance(g.GetType(), Editor.GetConstructorParameters(g.GetType())) as Gun;
            int ammo = gun.ammo;
            if (g.ammo != ammo)
            {
                g.ammo = ammo;
                gun.position = new Vec2(-9999f, -9999f);
                Level.Remove(gun);
                SFX.Play(Mod.GetPath<RSMod>("SFX\\ammoBox"));
                this.AmmoParticles(d.offDir);
                if (Network.isActive)
                {
                    Send.Message(new NMAmmoBox(d.offDir, this));
                }
                Level.Remove(this);
                base._destroyed = true;
            }
        }

        public void AmmoParticles(int dir)
        {
            AmmoPopUp popup = new AmmoPopUp(base.x, base.y - 8f);
            Level.Add(popup);
            int i;
            for (i = 0;i < 10;i += 1)
            {
                Level.Add(new MagnumShell(x, y)
                {
                    hSpeed = -dir * (1.5f + Rando.Float(1f))
                });
            }
        }

        public StateBinding netPickupSoundBinding = new NetSoundBinding("netPickupSound");
        public NetSoundEffect netPickupSound = new NetSoundEffect(new string[]
        {
            Mod.GetPath<RSMod>("SFX\\ammoBox")
        });
    }
}
