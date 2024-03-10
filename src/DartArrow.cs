using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    class DartArrow : PhysicsObject, ITeleport
    {
        public Vec2 travel;
        public float gravForce;
        public List<Vec2> Trail = new List<Vec2>();
        public int life;
        public Duck responsibleDuck;
        public bool teleported;
        public DartArrow(float xval, float yval) : base(xval, yval)
        {
            this.graphic = new Sprite(Mod.GetPath<RSMod>("arrow"));
            this.center = new Vec2(6.5f, 3.5f);
            this.collisionOffset = new Vec2(-6.5f, -3.5f);
            this.collisionSize = new Vec2(13f, 7f);
            this.friction = 0f;
            this.gravForce = 0f;
            this.gravMultiplier = 0f;
            this.weight = 4f;
        }

        public override void OnSoftImpact(MaterialThing with, ImpactedFrom from)
        {
            if (with != null)
            {
                if (with is Duck || with is TrappedDuck || with is RagdollPart)
                {
                    this.Collide(with);
                    this.Break(true);
                    Level.Remove(this);
                }
                if (with is Block || (with is IPlatform && from == ImpactedFrom.Top))
                {
                    SFX.Play("woodHit", 1f, 0f, 0f, false);
                    this.Break();
                    Level.Remove(this);
                }
            }
            base.OnSoftImpact(with, from);
        }

        public override void Update()
        {
            if (this.grounded && this.life > 3)
            {
                SFX.Play("woodHit", 1f, 0f, 0f, false);
                this.Break();
                Level.Remove(this);
            }
            this.life++;
            if (this.life > 20)
            {
                this.gravForce = Maths.Clamp(this.gravForce + 0.1f, 0f, 1f);
            }
            this.gravMultiplier = this.gravForce;
            this.travel = new Vec2(hSpeed, vSpeed);
            this.angleDegrees = -Maths.PointDirection(Vec2.Zero, travel);
            base.Update();
        }

        public void Collide(MaterialThing with)
        {
            if (with is TrappedDuck)
            {
                Level.Remove(this);
            }
            if (with is Duck)
            {
                Duck d = with as Duck;
                this.SpawnEffect(d);
            }
            else if (with is RagdollPart)
            {
                RagdollPart r = with as RagdollPart;
                Duck d = r.doll._duck as Duck;
                r.doll.Unragdoll();
                this.SpawnEffect(d);
            }
        }

        public void SpawnEffect(Duck d)
        {
            if ((this.responsibleDuck == d && this.life < 15) && !this.teleported)
            {
                return;
            }
            bool flag1 = false;
            foreach (BlowpipeEffect bp in Level.current.things[typeof(BlowpipeEffect)])
            {
                if (bp.duckOwner == d)
                {
                    flag1 = true;
                }
            }
            if (!flag1)
            {
                d.immobilized = true;
                BlowpipeEffect bpe = new BlowpipeEffect(0, 0, d);
                bpe.dx = d.x;
                bpe.dy = d.y;
                Level.Add(bpe);
                Thing.Fondle(bpe, d.connection);
                Level.Remove(this);
            }     
        }

        public override void Draw()
        {
            base.Draw();
            this.Trail.Add(this.position);
            if (this.Trail.Count > 32)
            {
                this.Trail.RemoveAt(0);
            }
            if (this.Trail.Count > 1)
            {
                for (int i = 1; i < this.Trail.Count; i++)
                {
                    Graphics.DrawLine(this.Trail[i - 1], this.Trail[i], Color.White * ((float)i / (float)this.Trail.Count) * 0.5f, 2f, -1f);
                }
            }
        }

        public override void OnTeleport()
        {
            this.teleported = true;
            base.OnTeleport();
        }

        public void Break(bool isDuck = false)
        {
            if (isDuck)
            {
                return;
            }
            for (int i = 0; i < 6; i++)
            {
                Vec2 travelDir = new Vec2(hSpeed, vSpeed);
                Thing ins = WoodDebris.New(base.x - 8f + Rando.Float(16f), base.y - 8f + Rando.Float(16f));
                ins.hSpeed = ((Rando.Float(1f) > 0.5f) ? 1f : -1f) * Rando.Float(3f) + (float)Math.Sign(travelDir.x) * 0.5f;
                ins.vSpeed = -Rando.Float(1f);
                Level.Add(ins);
            }
        }
    }

}
