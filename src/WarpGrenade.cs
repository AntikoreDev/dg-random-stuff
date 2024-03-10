using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    [EditorGroup("Random Stuff|Guns|Grenades")]
    class WarpGrenade : Gun
    {
        public SpriteMap _sprite;
        public bool _pin = true;
        public float _timer = 1.2f;
        public WarpGrenade(float xpos, float ypos) : base(xpos, ypos)
        {
            this.ammo = 1;
            this._sprite = new SpriteMap(Mod.GetPath<RSMod>("warpGrenade"), 16, 16);
            this.graphic = this._sprite;
            this.center = new Vec2(7f, 8f);
            this.collisionOffset = new Vec2(-4f, -5f);
            this.collisionSize = new Vec2(8f, 10f);
            base.bouncy = 0.4f;
            this.friction = 0.05f;
            this._editorName = "Warp Grenade";
        }

        public override void Initialize()
        {
            if (Network.isActive)
            {
                Rando.generator = new Random(NetRand.currentSeed);
            }
            base.Initialize();
        }

        public override void OnPressAction()
        {
            if (this._pin)
            {
                this._pin = false;
                Level.Add(new GrenadePin(base.x, base.y)
                {
                    hSpeed = (float)(-(float)this.offDir) * (1.5f + Rando.Float(0.5f)),
                    vSpeed = -2f
                });
                SFX.Play("pullPin", 1f, 0f, 0f, false);
            }
        }

        public override void Update()
        {
            base.Update();
            if (!this._pin)
            {
                this._timer -= 0.01f;
            }
            if (this._timer <= 0f)
            {
                this.Explode();
            }
            this._sprite.frame = (this._pin ? 0 : 1);
        }

        public void Explode()
        {
            foreach (PhysicsObject p in Level.CheckCircleAll<PhysicsObject>(this.position, 64f).ToList<PhysicsObject>())
            {
                if (p != null && p != this)
                {
                    this.Warp(p, p.x, p.y);
                }
            }
            SFX.Play(Mod.GetPath<RSMod>("SFX\\warp"));
            if (Network.isServer)
            {
                this._netWarpSound.Play();
            }
            EMPExplosion emp = new EMPExplosion(base.x, base.y);
            Level.Add(emp);
            Level.Remove(this);
        }

        public void Warp(PhysicsObject p, float oldx, float oldy)
        {
            bool goodPosition = false;
            bool neverTped = false;
            int tries = 0;
            while (!goodPosition)
            {
                if (tries >= 10)
                {
                    this.position = new Vec2(oldx, oldy);
                    neverTped = true;
                    goodPosition = true;
                }
                float xx = Rando.Float(Level.activeLevel.topLeft.x, Level.activeLevel.bottomRight.x);
                float yy = Rando.Float(Level.activeLevel.topLeft.y - (p.bottom - p.top), Level.activeLevel.bottomRight.y);
                p.position = new Vec2(xx, yy);
                List<Block> checkObjects = Level.CheckRectAll<Block>(p.topLeft, p.bottomRight).ToList<Block>();
                if (checkObjects.Count == 0)
                {
                    goodPosition = true;
                }
                tries++;
            }
            if (!neverTped)
            {
                if (isServerForObject)
                {
                    this.WarpEffect(oldx, oldy);
                    if (Network.isActive)
                    {
                        Send.Message(new NMWarpParticles(oldx, oldy));
                    }
                }
            }
            p.sleeping = false;
        }

        public void WarpEffect(float xx, float yy)
        {
            for (int i = 0; i < 15; i++)
            {
                WarpParticle wp = new WarpParticle(xx, yy);
                Level.Add(wp);
                base.Fondle(wp);
            }
        }

        public StateBinding _netWarpSoundBinding = new NetSoundBinding("_netWarpSound");
        public NetSoundEffect _netWarpSound = new NetSoundEffect(new string[]
        {
            Mod.GetPath<RSMod>("SFX\\warp")
        });
        public StateBinding _timerBinding = new StateBinding("_timer", -1, false, false);
        public StateBinding _pinBinding = new StateBinding("_pin", -1, false, false);
    }
}
