using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    class BlockBase : Block, IPathNodeBlocker
    {
        public bool canBounce
        {
            get
            {
                return this._canBounce;
            }
        }

        public BlockBase(float xpos, float ypos) : base(xpos, ypos)
		{
            base.graphic = null;
            this.center = new Vec2(8f, 8f);
            this.collisionSize = new Vec2(16f, 16f);
            this.collisionOffset = new Vec2(-8f, -8f);
            base.depth = 0.5f;
            this.timesUsed = 0;
            this._canFlip = false;
        }

        public void Pop(MaterialThing with)
        {
            this.Bounce(with);
            if (!this._hit && (this.timesUsed < this.maxUses || (float)this.maxUses == -1f))
            {
                this.Activate(with);
            }
        }

        public void Bounce(MaterialThing with)
        {
            if (!this._canBounce)
            {
                return;
            }
            this.bounceAmount = 8f;
            this._canBounce = false;
            if (Network.isActive)
            {
                this.netDisarmIndex += 1;
                return;
            }
            this._aboveList = Level.CheckRectAll<PhysicsObject>(base.topLeft + new Vec2(1f, -4f), base.bottomRight + new Vec2(-1f, -12f)).ToList<PhysicsObject>();
            foreach (PhysicsObject physicsObject in this._aboveList)
            {
                if (physicsObject.grounded || physicsObject.vSpeed >= 0f)
                {
                    base.Fondle(physicsObject);
                    physicsObject.y -= 2f;
                    physicsObject.vSpeed = -3f;
                    Duck duck = physicsObject as Duck;
                    if (duck != null)
                    {
                        duck.Disarm(this);
                    }
                }
            }
        }

        public virtual void Activate(MaterialThing with)
        {
            this.timesUsed++;
            this._netHitSound.Play(1f, 0f);
            this.charging = this.rechargeTime;
            this._hit = true;
        }

        public override void OnSoftImpact(MaterialThing with, ImpactedFrom from)
        {
            if (from != ImpactedFrom.Bottom || !with.isServerForObject)
            {
                return;
            }
            with.Fondle(this);
            this.Pop(with);
        }

        public virtual void UpdateCharging()
        {
            if (!base.isServerForObject || (this.maxUses != -1 && this.timesUsed == this.maxUses))
            {
                return;
            }
            if (this.charging > 0)
            {
                this.charging--;
                return;
            }
            this.charging = 0;
            this._hit = false;
        }

        public override void Update()
        {
            this._aboveList.Clear();
            if ((double)this.startY < -9999.0)
            {
                this.startY = base.y;
            }
            this.sprite.frame = ((this._hit || this.timesUsed == this.maxUses) ? 1 : 0);
            if (this.netDisarmIndex != this.localNetDisarm)
            {
                this.localNetDisarm = this.netDisarmIndex;
                this._aboveList = Level.CheckRectAll<PhysicsObject>(base.topLeft + new Vec2(1f, -4f), base.bottomRight + new Vec2(-1f, -12f)).ToList<PhysicsObject>();
                foreach (PhysicsObject physicsObject in this._aboveList)
                {
                    if (base.isServerForObject && physicsObject.owner == null)
                    {
                        base.Fondle(physicsObject);
                    }
                    if (physicsObject.isServerForObject && (physicsObject.grounded || physicsObject.vSpeed >= 0f))
                    {
                        physicsObject.y -= 2f;
                        physicsObject.vSpeed = -3f;
                        Duck duck = physicsObject as Duck;
                        if (duck != null)
                        {
                            duck.Disarm(this);
                        }
                    }
                }
            }
            this.UpdateCharging();
            if (this.bounceAmount > 0f)
            {
                this.bounceAmount -= 0.8f;
            }
            else
            {
                this.bounceAmount = 0f;
            }
            base.y -= this.bounceAmount;
            if (this._canBounce)
            {
                return;
            }
            if (base.y < this.startY)
            {
                base.y += 0.8f + Math.Abs(base.y - this.startY) * 0.4f;
            }
            if (base.y > this.startY)
            {
                base.y -= 0.8f - Math.Abs(base.y - this.startY) * 0.4f;
            }
            if ((double)Math.Abs(base.y - this.startY) >= 0.8)
            {
                return;
            }
            this._canBounce = true;
            base.y = this.startY;
        }

        public StateBinding _positionBinding = new StateBinding("position", -1, false);
        public StateBinding _boxStateBinding = new StateBinding("_hit", -1, false);
        public StateBinding _chargingBinding = new StateBinding("charging", 9, false);
        public StateBinding _netDisarmIndexBinding = new StateBinding("netDisarmIndex", -1, false);
        public StateBinding _netHitSoundBinding = new NetSoundBinding("_netHitSound");
        public NetSoundEffect _netHitSound = new NetSoundEffect(new string[]
        {
            "hitBox"
        })
        {
            volume = 1f
        };
        public EditorProperty<int> maxUses = new EditorProperty<int>(-1, null, -1f, 100f, 1f, "INF", false, false);
        public EditorProperty<int> rechargeTime = new EditorProperty<int>(500, null, 0f, 3000f, 10f, null, false, false);
        public float startY = -99999f;
        protected List<PhysicsObject> _aboveList = new List<PhysicsObject>();
        public bool _canBounce = true;
        public byte netDisarmIndex;
        public byte localNetDisarm;
        public float bounceAmount;
        public bool _hit;
        public int charging;
        protected int timesUsed;
        protected SpriteMap sprite;
    }
}
