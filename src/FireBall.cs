using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    class FireBall : PhysicsObject, ITeleport
    {
        private SpriteMap _sprite;
        private Sprite _glow;
        public Thing shooter;
        public int _timer;
        public int _bouncer;
        public FireBall(float xpos, float ypos) : base(xpos, ypos)
        {
            this._sprite = new SpriteMap(Mod.GetPath<RSMod>("FireBall"), 10, 10);
            this._glow = new Sprite("redHotGlow");
            this.graphic = _sprite;
            this._sprite.frame = 0;
            this.center = new Vec2(4.5f, 4.5f);
            this.collisionSize = new Vec2(10f, 10f);
            this.collisionOffset = new Vec2(-4.5f, -4.5f);
            this.weight = 0f;
            this._sprite.AddAnimation("Default", 0.25f, true, new int[]
            {
                0,
                1,
                2,
                3
            });
            this.bouncy = 1f;
            this.friction = 0f;
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
            this._touchObjects = Level.CheckRectAll<PhysicsObject>(this.topLeft, this.bottomRight).ToList<PhysicsObject>();
            if (_touchObjects.Count > 0)
            {
                foreach (PhysicsObject p in _touchObjects)
                {
                    if (p != null && p != this)
                    { 
                        if (p is Holdable)
                        {
                            Holdable h = p as Holdable;
                            if (h != null)
                            {
                                if (h.flammable > 0)
                                {
                                    h.Burn(new Vec2(base.x, base.y), this);
                                }
                            }
                        }
                        else if (p is Duck)
                        {
                            Duck d = p as Duck;
                            if (d != null && d != this.shooter)
                            {
                                d.Burn(new Vec2(base.x, base.y), this);
                            }
                        }
                    }
                }
            }
            this._timer++;
            if (this._timer >= 15 && this.shooter != null)
            {
                this.shooter = null;
            }
            else if (this._timer >= 300)
            {
                this.alpha -= 0.25f;
                if (this.alpha <= 0)
                {
                    Level.Remove(this);
                }
            }
            base.Update();
            if (this.vSpeed > -0.5f && this.vSpeed < 0.5f && this.grounded)
            {
                this._bouncer++;
            }
            else
            {
                this._bouncer = 0;
            }
            if (this._bouncer >= 5)
            {
                Level.Remove(this);
            }
        }

        public override void OnSolidImpact(MaterialThing with, ImpactedFrom from)
        {
            if (with != null)
            {
                for (int index = 0; index < 5; index++)
                {
                    float max = Rando.Float(1f, 3f);
                    Level.Add(SmallFire.New(base.x, base.y - 2f, Rando.Float(-max, max), Rando.Float(-max, max + 2f), false, null, true, this, false));
                }
            }
            base.OnSoftImpact(with, from);
        }

        public override void Draw()
        {
            this._glow.alpha = 0.5f * this.alpha;
            Graphics.Draw(this._glow, x - 16, y - 16);
            base.Draw();
        }

        private List<PhysicsObject> _touchObjects = new List<PhysicsObject>();
        public StateBinding _shooterBinding = new StateBinding("shooter", -1, false, false);
    }
}
