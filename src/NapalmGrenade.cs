using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using DuckGame;

namespace DuckGame.RSModDEV
{
    [EditorGroup("Random Stuff|Guns|Grenades")]
    class NapalmGrenade : Gun
    {
        private bool _localDidExplode;
        public SpriteMap sprite;
        public NapalmGrenade(float xval, float yval) : base(xval, yval)
        {
            this.sprite = new SpriteMap(GetPath("incendiaryGrenade"), 16, 16);
            this.graphic = sprite;
            this._ammoType = new ATLaser();
            this.ammo = 8;
            this._editorName = "Napalm Grenade";
            this.center = new Vec2(7f, 8f);
            this.collisionOffset = new Vec2(-4f, -5f);
            this.collisionSize = new Vec2(8f, 10f);
            base.bouncy = 0.4f;
            this.friction = 0.05f;
        }

        public override void OnNetworkBulletsFired(Vec2 pos)
        {
            this._pin = false;
            this._localDidExplode = true;
            if (!this._explosionCreated)
            {
                Graphics.flashAdd = 1.3f;
                Layer.Game.darken = 1.3f;
            }
        }

        public override void Update()
        {
            if (!this._pin)
            {
                this._timer -= 0.01f;
            }
            this.sprite.frame = (_pin ? 0 : 1);
            if (!this._localDidExplode && this._timer < 0f)
            {
                this.Explode(this, true);
            }
            base.Update();
        }

        public void Explode(Gun g, bool server = true)
        {
            if (isServerForObject)
            {
                float cx = g.x;
                float cy = g.y - 2f;
                Level.Add(new ExplosionPart(cx, cy, true));
                int num = 6;
                if (Graphics.effectsLevel < 2)
                {
                    num = 3;
                }
                for (int i = 0; i < num; i++)
                {
                    float dir = (float)i * 60f + Rando.Float(-10f, 10f);
                    float dist = Rando.Float(12f, 20f);
                    ExplosionPart ins = new ExplosionPart(cx + (float)(Math.Cos((double)Maths.DegToRad(dir)) * (double)dist), cy - (float)(Math.Sin((double)Maths.DegToRad(dir)) * (double)dist), true);
                    Level.Add(ins);
                }
                if (server)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        Level.Add(SmallFire.New(g.x - 6f + Rando.Float(12f), g.y - 8f + Rando.Float(4f), -6f + Rando.Float(12f), 2f - Rando.Float(8.5f), false, null, true, g, false));
                    }
                    Level.Remove(g);
                }
                Graphics.FlashScreen();
                SFX.Play("explode", 1f, 0f, 0f, false);
            }
        }

        public override void OnSolidImpact(MaterialThing with, ImpactedFrom from)
        {
            if (this.pullOnImpact)
            {
                this.OnPressAction();
            }
            base.OnSolidImpact(with, from);
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

        public StateBinding _timerBinding = new StateBinding("_timer", -1, false);
        public StateBinding _pinBinding = new StateBinding("_pin", -1, false);
        private SpriteMap _sprite;
        public bool _pin = true;
        public float _timer = 1.2f;
        private Duck _cookThrower;
        private float _cookTimeOnThrow;
        public bool pullOnImpact;
        private bool _explosionCreated;
        private bool _didBonus;
        private static int grenade;
        public int gr;
        public int _explodeFrames = -1;
    }
}
