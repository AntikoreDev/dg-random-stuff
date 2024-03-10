using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    class GuitarSoundwave : Thing, ITeleport
    {
        public float hsp;
        public float vsp;
        public float fsize;
        public int safePeriod;
        public Duck shooter;
        public GuitarSoundwave(float xval, float yval) : base(xval, yval)
        {
            this.graphic = new Sprite(GetPath("guitarWave"));
            this.visible = true;
            this.collisionOffset = new Vec2(-3f, -5f);
            this.collisionSize = new Vec2(6f, 10f);
            this.center = new Vec2(3f, 5f);
            base.layer = Layer.Blocks;
            base.depth = 0.95f;
            this.vsp = 0f;
            this.hsp = 0f;
            this.safePeriod = 16;
            this.xscale = 0f;
            this.yscale = 0f;
            this.fsize = 0.65f;
        }

        public override void Update()
        {
            if (hsp == 0 && vsp == 0)
            {
                Level.Remove(this);
                return;
            }
            if (base.isServerForObject && (base.x > Level.current.bottomRight.x + 200f || base.x < Level.current.topLeft.x - 200f))
            {
                Level.Remove(this);
                return;
            }
            if (this.safePeriod > 0)
            {
                this.safePeriod--;
            }
            else
            {
                this.shooter = null;
            }
            this.x += hsp;
            this.y += vsp;
            if (this.xscale < this.fsize)
            {
                this.xscale += 0.1f;
                this.yscale += 0.1f;
            }
            else if (this.xscale > this.fsize)
            {
                this.xscale = this.fsize;
                this.yscale = this.fsize;
            }
            this.CheckCollision();
            base.Update();
        }

        public void CheckCollision()
        {
            foreach (MaterialThing mt in Level.CheckRectAll<MaterialThing>(base.topLeft, base.bottomRight))
            {
                if (!this.iTouched.Contains(mt))
                {
                    if (!(mt is IAmADuck && (this.shooter != null && this.shooter == mt)))
                    {
                        if (mt is Holdable && !(mt is Gun))
                        {
                            if (!(mt is BlueBarrel || mt is YellowBarrel || mt is LavaBarrel))
                            {
                                this.iTouched.Add(mt);
                                mt._hitPoints -= Rando.Float(2f, 5f);
                                if (mt is Crate || mt is ECrate)
                                {
                                    SFX.Play("woodHit", 1f, 0f, 0f, false);
                                }
                            }
                        }
                        else if (mt is Door)
                        {
                            mt._hitPoints -= 5f;
                        }
                        else
                        {
                            mt.Destroy(new DTImpale(this));
                        }
                    }
                }
            }
        }

        public override void OnTeleport()
        {
            this.iTouched.Clear();
            base.OnTeleport();
        }

        public List<MaterialThing> iTouched = new List<MaterialThing>();
        public StateBinding safeBinding = new StateBinding("safePeriod", -1, false, false);
        public StateBinding _xscaleBinding = new StateBinding("xscale", -1, false, false);
        public StateBinding _yscaleBinding = new StateBinding("yscale", -1, false, false);
    }
}
