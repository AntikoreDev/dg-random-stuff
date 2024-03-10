using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    [EditorGroup("Random Stuff|Misc")]
    class BowlingBall : Holdable
    {
        public float requiredImpact;
        public float requiredImpact2;
        public MaterialThing lastOwner;
        public int timer;
        public BowlingBall(float xpos, float ypos) : base(xpos, ypos)
		{
            this.graphic = new Sprite(Mod.GetPath<RSMod>("bowlingBall"));
            this.center = new Vec2(8f, 8f);
            this.collisionOffset = new Vec2(-8f, -8f);
            this.collisionSize = new Vec2(15f, 15f);
            base.depth = -0.4f;
            this.thickness = 1f;
            this.weight = 5f;
            this.requiredImpact = 1f;
            this.requiredImpact2 = 2f;
            this.physicsMaterial = PhysicsMaterial.Metal;
            this._bouncy = 0.6f;
            this.friction = 0.03f;
            this._impactThreshold = 0.1f;
            this._holdOffset = new Vec2(6f, 0f);
            this.handOffset = new Vec2(0f, 0f);
        }

        public override void Update()
        {
            this.angleDegrees += this.hSpeed * 3.5f;
            if (this.owner != null)
            {
                this.timer = 10;
                this.lastOwner = this.owner as Duck;
            }
            else if (this.timer > 0)
            {
                this.timer--;
            }
            base.Update();
        }

        public override void OnSolidImpact(MaterialThing with, ImpactedFrom from)
        {
            if (with != null && base.isServerForObject)
            {
                MaterialThing p = with;
                if (p is Block && (this.hSpeed > this.requiredImpact || this.hSpeed < -this.requiredImpact || this.vSpeed > this.requiredImpact || this.vSpeed < -this.requiredImpact))
                {
                    SFX.Play("barrelThud");
                    if (Network.isActive)
                    {
                        this._netCollidingSound.Play();
                    }
                }
                if (p is Block && !(p is AutoBlock || p is Door || p is Window || p is PurpleBlock || p is GoldenBlock))
                {
                    if (this.hSpeed > this.requiredImpact || this.hSpeed < -this.requiredImpact)
                    {
                        if (from != ImpactedFrom.Top)
                        {
                            if (this.lastOwner != null)
                            {
                                p.OnSoftImpact(this.lastOwner, ImpactedFrom.Bottom);
                            }
                            else
                            {
                                p.OnSoftImpact(this, ImpactedFrom.Bottom);
                            }
                        }
                    }
                }
            }
            base.OnSolidImpact(with, from);
        }

        public override void OnSoftImpact(MaterialThing with, ImpactedFrom from)
        {
            if (with != null && base.isServerForObject)
            {
                if (with is IAmADuck)
                {
                    if (this.hSpeed > this.requiredImpact2 || this.hSpeed < -this.requiredImpact2 && from != ImpactedFrom.Bottom && !(with == this.lastOwner && this.timer > 0))
                    {
                        SFX.Play("barrelThud");
                        if (Network.isActive)
                        {
                            this._netCollidingSound.Play();
                        }
                        MaterialThing mt = (MaterialThing)with;
                        mt.Destroy(new DTImpale(this));
                        this.hSpeed = -this.hSpeed;
                    }
                }
            }
            base.OnSoftImpact(with, from);
        }
        public StateBinding _lastOwnerBinding = new StateBinding("lastOwner", -1, false, false);
        public StateBinding _netCollidingSoundBinding = new NetSoundBinding("_netCollidingSound");
        public NetSoundEffect _netCollidingSound = new NetSoundEffect(new string[]
        {
            "barrelThud"
        });
    }
}
