using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    [BaggedProperty("noRandomSpawningOnline", true), EditorGroup("Random Stuff|Props")]
    class ExplosiveBarrel : Holdable, IPlatform
    {
        public int verryJot;
        public ExplosiveBarrel(float xpos, float ypos) : base(xpos, ypos)
        {
            this.collisionOffset = new Vec2(-7f, -9f);
            this.collisionSize = new Vec2(14f, 17f);
            base.depth = -0.5f;
            this.thickness = 4f;
            this.weight = 10f;
            this.buoyancy = 1f;
            this.flammable = 0.5f;
            this._hitPoints = 0.1f;
            this.physicsMaterial = PhysicsMaterial.Metal;
            base.collideSounds.Add("barrelThud");
            this.graphic = new Sprite(GetPath("explosiveBarrel"));
            this.center = new Vec2(7f, 8f);
            this._editorName = "Explosive Barrel";
        }

        public override bool Hit(Bullet bullet, Vec2 pos)
        {
            if (this._hitPoints <= 0f)
            {
                return base.Hit(bullet, pos);
            }
            if (bullet != null)
            {
                if (bullet.isLocal && this.owner == null)
                {
                    Thing.Fondle(this, DuckNetwork.localConnection);
                }
                this.Destroy(new DTShot(bullet));
            }         
            return base.Hit(bullet, pos);
        }

        protected override bool OnDestroy(DestroyType type = null)
        {
            this.Explode();
            return true;
        }

        public override void Update()
        {
            if (isServerForObject)
            {
                if (this.onFire)
                {
                    this.verryJot++;
                    if (this.verryJot > 40)
                    {
                        this.Destroy(new DTIncinerate(this));
                    }
                }
            }
            if (this._hitPoints <= 0f)
            {
                this.Destroy(new DTImpale(this));
            }
            base.Update();
        }

        public void Explode()
        {
            if (isServerForObject)
            {
                Level.Add(new GrenadeExplosion(this.x, this.y));
            }
            SFX.Play("crateDestroy", 1f, 0f, 0f, false);
            if (Network.isActive)
            {
                this._netDestroySound.Play();
            }
            Level.Remove(this);
        }

        public StateBinding _hotBinding = new StateBinding("verryJot", -1, false, false);
        public StateBinding _netDestroySoundBinding = new NetSoundBinding("_netDestroySound");
        public NetSoundEffect _netDestroySound = new NetSoundEffect(new string[]
        {
            "crateDestroy"
        })
        {
            volume = 1f
        };
    }
}
