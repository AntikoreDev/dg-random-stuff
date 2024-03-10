using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using DuckGame;

namespace DuckGame.RSModDEV
{
    class DelayBlockHitter : Block, IPathNodeBlocker
    {
        public Thing _blockOwner;
        public DelayBlockHitter(float xpos, float ypos, Thing blockOwner) : base(xpos, ypos)
        {
            this.graphic = new Sprite(GetPath("none"));
            this._blockOwner = blockOwner;
            this.center = new Vec2(8f, 8f);
            this.collisionSize = new Vec2(16f, 16f);
            this.collisionOffset = new Vec2(-8f, -8f);
        }

        public override void Update()
        {
            if (_blockOwner == null || Level.current is Editor)
            {
                Level.Remove(this);
            }
        }

        public void Kill()
        {
            if (this.isServerForObject)
            {
                this.solid = false;
                this._aboveList = Level.CheckRectAll<PhysicsObject>(base.topLeft + new Vec2(-1f, -4f), base.bottomRight + new Vec2(1f, -12f)).ToList<PhysicsObject>();
                this._aboveList2 = Level.CheckRectAll<PhysicsParticle>(base.topLeft + new Vec2(-1f, -4f), base.bottomRight + new Vec2(1f, -12f)).ToList<PhysicsParticle>();
                foreach (PhysicsObject p in this._aboveList)
                {
                    if (p.isServerForObject)
                    {
                        p.sleeping = false;
                    }
                }
                foreach (PhysicsParticle p in _aboveList2)
                {
                    if (p.isServerForObject)
                    {
                        typeof(PhysicsParticle).GetField("_grounded", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).SetValue(p, false);
                    }
                }
                Level.Remove(this);
            }
        }
        protected List<PhysicsObject> _aboveList = new List<PhysicsObject>();
        protected List<PhysicsParticle> _aboveList2 = new List<PhysicsParticle>();
    }
}
