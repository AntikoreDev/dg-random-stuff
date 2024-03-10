using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    class DiscObject : PhysicsObject, ITeleport
    {
        public SpriteMap _sprite;
        public float vsp;
        public float hsp;
        public int fixer;
        public Duck shooter;
        public int shooterLeisure;
        public DiscObject(float xval, float yval) : base(xval, yval)
        {
            this._sprite = new SpriteMap(GetPath("disc"),16,16);
            this.graphic = _sprite;
            this.center = new Vec2(7.5f, 7.5f);
            this.collisionOffset = new Vec2(-5f, -5f);
            this.collisionSize = new Vec2(10f, 10f);
            this.fixer = 0;
            this.gravMultiplier = 0f;
            this._skipPlatforms = true;
            this.shooterLeisure = 4;
            this._sprite.AddAnimation("Default", 0.1f, true, new int[]
            {
                0,
                1
            });
        }

        public override void Initialize()
        {
            if (!(Level.current is Editor))
            {
                this._sprite.SetAnimation("Default");
            }
            base.Initialize();
        }

        public override void Update()
        {
            if (this.fixer > 0)
            {
                this.fixer--;
            }
            if (this.shooterLeisure > 0)
            {
                this.shooterLeisure--;
            }
            this.vSpeed = vsp;
            this.hSpeed = hsp;
            base.Update();
        }

        public override void OnSoftImpact(MaterialThing with, ImpactedFrom from)
        {
            if (with != null)
            {
                if (with is Duck)
                {
                    Duck d = with as Duck;
                    if (!(this.shooterLeisure > 0 && this.shooter != null && this.shooter == d))
                    {
                        if (isServerForObject && !d.dead)
                        {
                            d.Kill(new DTImpact(this));
                        }
                    }   
                }
                if (with is RagdollPart)
                {
                    RagdollPart r = with as RagdollPart;
                    Duck d = r.doll._duck as Duck;
                    if (r != null && d != null && !d.dead)
                    {
                        d.Kill(new DTImpact(this));
                    }
                }
                if (with is Holdable && !(with is Gun))
                {
                    Holdable h = with as Holdable;
                    if (h != null)
                    {
                        h._hitPoints -= 3f;
                    }
                }
                if (with is Door)
                {
                    this.Ricochet(from);
                    if (from == ImpactedFrom.Right || from == ImpactedFrom.Left)
                    {
                        Door d = with as Door;
                        if (d != null)
                        {
                            d._hitPoints -= 5f;
                        }
                    }
                }
                if (with is Window || with is FloorWindow)
                {
                    Window w = with as Window;
                    if (w != null)
                    { 
                        w.hitPoints -= 30f;
                    }
                }
                else if (with is Block || with is AutoBlock)
                {
                    this.Ricochet(from); 
                }
            }
            base.OnSoftImpact(with, from);
        }

        public void Ricochet(ImpactedFrom from)
        {
            if (fixer == 0)
            {
                if (Network.isActive)
                {
                    this._netHitSound.Play();
                }
                SFX.Play(Mod.GetPath<RSMod>("SFX\\discRicochet"), 1f, 0f, 0f, false);
                if (from == ImpactedFrom.Top || from == ImpactedFrom.Bottom)
                {
                    this.vsp = -vsp;
                }
                else if (from == ImpactedFrom.Right || from == ImpactedFrom.Left)
                {
                    this.hsp = -hsp;
                }
                this.fixer = 2;
            }
        }

        public StateBinding _netHitSoundBinding = new NetSoundBinding("_netHitSound");
        public NetSoundEffect _netHitSound = new NetSoundEffect(new string[]
        {
            Mod.GetPath<RSMod>("SFX\\discRicochet")
        })
        {
            volume = 1f
        };
        //public StateBinding vspBinding = new StateBinding("vsp", -1, false, false);
        //public StateBinding hspBinding = new StateBinding("hsp", -1, false, false);
        public StateBinding fixerBinding = new StateBinding("fixer", -1, false, false);
        public StateBinding shooterLeisureBinding = new StateBinding("shooterLeisure", -1, false, false);

    }
}
