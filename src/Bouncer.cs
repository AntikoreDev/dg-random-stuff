using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    [EditorGroup("Random Stuff|Props")]
    class Bouncer : Holdable, IPlatform
    {
        public bool itSound;
        public Bouncer(float xpos, float ypos) : base(xpos, ypos)
        {
            this.graphic = new Sprite(GetPath("bouncer"));
            this._maxHealth = 9999f;
            this.center = new Vec2(8f, 8f);
            this.collisionOffset = new Vec2(-8f, -8f);
            this.collisionSize = new Vec2(16f, 16f);
            base.depth = -0.5f;
            this.thickness = 2f;
            this.weight = 5f;
            this.buoyancy = 1f;
            this._editorName = "Bouncer";
            this._hitPoints = 9999f;
        }

        public override void Update()
        {
            List<PhysicsObject> _bounceObjects = new List<PhysicsObject>();
            _bounceObjects = Level.CheckRectAll<PhysicsObject>(base.topLeft + new Vec2(-2f, -4f), base.bottomRight + new Vec2(1f, -12f)).ToList<PhysicsObject>();
            if (_bounceObjects.Count > 0)
            {
                foreach (PhysicsObject t in _bounceObjects)
                {
                    if (!(t is DWBullet))
                    {
                        if (!(this.owner != null && this.owner == t) && t != this && this.top >= t.bottom && t.vSpeed >= 0 && t.owner == null && t.gravMultiplier != 0f && t.active)
                        {
                            t.vSpeed = -7f;
                            if (t is Gun)
                            {
                                Gun g = t as Gun;
                                g.OnPressAction();
                            }
                            if (!itSound)
                            {
                                this.Sound();
                            }
                        }
                    }          
                }  
            }
            this.itSound = false;
            base.Update();
        }

        public void Sound()
        {
            SFX.Play("spring", 0.5f);
            if (Network.isActive)
            {
                this._netBounceSound.Play();
            }
            this.itSound = true;
        }

        public StateBinding _netBounceSoundBinding = new NetSoundBinding("_netBounceSound");
        public NetSoundEffect _netBounceSound = new NetSoundEffect(new string[]
        {
            "spring"
        })
        {
            volume = 0.5f
        };
    }
}
