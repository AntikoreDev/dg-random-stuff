using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{ 
    class DWBullet : PhysicsObject, ITeleport
    {
        public Sprite _sprite;
        public Duck shooter;
        public int _timer;
        public DWBullet(float xval, float yval) : base(xval, yval)
        {
            this._sprite = new Sprite(GetPath("dwBullet"));
            this.graphic = _sprite;
            this.center = new Vec2(7.5f, 7.5f);
            this.collisionOffset = new Vec2(-5f, -5f);
            this.collisionSize = new Vec2(10f, 10f);
            this.xscale = 0.75f;
            this.yscale = 0.75f;
            this.gravMultiplier = 1f;
        }

        public override void Update()
        {
            this._timer++;
            if (this.vSpeed <= 0 && this._timer >= 10)
            {
                this.Explode();
            }
            base.Update();
        }

        public override void OnSoftImpact(MaterialThing with, ImpactedFrom from)
        {
            if (with != null)
            {
                if (with is IAmADuck)
                {
                    Duck duck = null;
                    if (with is RagdollPart)
                    {
                        RagdollPart r = with as RagdollPart;
                        Duck d = r.doll._duck;
                        duck = d;
                    }
                    else if (with is TrappedDuck)
                    {
                        TrappedDuck t = with as TrappedDuck;
                        Duck d = t.captureDuck;
                        duck = d;
                    }
                    else if (with is Duck)
                    {
                        duck = with as Duck;
                    }
                    if (duck != null)
                    {
                        if (!(this.shooter != null && this.shooter == duck))
                        {
                            MaterialThing mt = (MaterialThing)with;
                            mt.Destroy(new DTImpale(this));
                            this.Explode();
                        }
                    }
                    
                }
                else if (with is Block || (with is IPlatform && !(with is Holdable)) || with is AutoBlock)
                {
                    if (with != null)
                    { 
                        this.Explode();
                        Block b = with as Block;
                        if (b is ItemBox)
                        {
                            ItemBox pb = b as ItemBox;
                            pb.OnSoftImpact(this.shooter, ImpactedFrom.Bottom);
                        }
                        else if (b is Door)
                        {
                            b._hitPoints -= 5f;
                        }
                        else if (b is Gravel)
                        {
                            Gravel g = b as Gravel;
                            g._timer = 45;
                            g.touched = true;
                            g._sprite.frame = 1;
                            g.alpha = 1f;
                            
                        }
                        else if (b is Block)
                        {
                            Block s = b as Block;
                            try
                            {
                                s.OnSoftImpact(this.shooter, ImpactedFrom.Bottom);
                            }
                            catch
                            {
                            }
                        }
                    }
                }
                else if (with is Holdable && !(with is Gun) && !(with is RagdollPart) && !(with is Key))
                {
                    Holdable h = with as Holdable;
                    h._hitPoints -= 5f;
                    this.Explode();
                }     
            }
            base.OnSoftImpact(with, from);
        }

        public void Explode()
        {
            ExplosionPart ex = new ExplosionPart(base.x, base.y, true);
            ex.xscale *= 0.7f;
            ex.yscale *= 0.7f;
            Level.Add(ex);
            if (Network.isActive)
            {
                this._netHitSound.Play();
            }
            SFX.Play("magPop", 0.7f, Rando.Float(-0.5f, -0.3f), 0f, false);
            Level.Remove(this);
        }

        public StateBinding _shooterBinding = new StateBinding("shooter", -1, false, false);
        public StateBinding _netHitSoundBinding = new NetSoundBinding("_netHitSound");
        public NetSoundEffect _netHitSound = new NetSoundEffect(new string[]
        {
            "magPop"
        })
        {
            volume = 1f
        };
    }
}
