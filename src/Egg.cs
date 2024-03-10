using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    class Egg : PhysicsObject
    {
        public SpriteMap _sprite;
        public bool _isExplosive;
        public Egg(float xpos, float ypos) : base(xpos, ypos)
        {
            this._sprite = new SpriteMap(GetPath("egg"), 7, 8);
            this.graphic = this._sprite;
            this._sprite.frame = 0;
            this.airFrictionMult = 0.5f;
            this.center = new Vec2(3f, 3f);
            this.collisionOffset = new Vec2(-2f, -2f);
            this.collisionSize = new Vec2(5f, 6f);
        }
        public override void Initialize()
        {
            if (base.isServerForObject)
            {
                if (Rando.Int(1, 4) == 4)
                {
                    this._isExplosive = true;
                    this._sprite.frame = 1;
                }
            }
            base.Initialize();
        }
        public override void OnSolidImpact(MaterialThing with, ImpactedFrom from)
        {
            if (with != null)
            {
                if (with is Duck)
                {
                    Duck d = with as Duck;
                    if (!d.dead)
                    {
                        d.Kill(new DTImpact(this));
                        this.Crack();
                    }
                }
                else
                {
                    this.Crack();
                }
            }
            base.OnSolidImpact(with, from);
        }

        public void Crack()
        {
            SFX.Play(Mod.GetPath<RSMod>("SFX\\eggCrack"), 0.5f);
            if (Network.isActive)
            {
                this._netHitSound.Play();
            }
            if (isServerForObject)
            {
                Level.Remove(this);
                if (this._isExplosive)
                {
                    Level.Add(new GrenadeExplosion(this.x, this.y));
                }
                this.EggBreak();
                if (Network.isActive)
                {
                    Send.Message(new NMEggBreak(this.x, this.y));
                }
            } 
        }
        public void EggBreak()
        {
            for (int i = 0; i < 15; i++)
            {
                EggCrack e = new EggCrack();
                e.position = this.position;
                e.vSpeed = Rando.Float(-1f, 1f);
                e.hSpeed = Rando.Float(-1f, 1f);
                Level.Add(e);
            }
        }

        public StateBinding _netHitSoundBinding = new NetSoundBinding("_netHitSound");
        public NetSoundEffect _netHitSound = new NetSoundEffect(new string[]
        {
            Mod.GetPath<RSMod>("SFX\\eggCrack")
        })
        {
            volume = 0.5f
        };

        public StateBinding _explosiveBinding = new StateBinding("_isExplosive", -1, false, false);
    }
}