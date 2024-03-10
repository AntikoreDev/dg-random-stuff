using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    [EditorGroup("Random Stuff|Guns|Grenades")]
    class EMPGrenade : Gun
    {
        public bool _pin = true;
        public float _timer = 1.2f;
        public SpriteMap _sprite;
        public EMPGrenade(float xval, float yval) : base(xval, yval)
        {
            this.ammo = 1;
            this._ammoType = new ATShrapnel();
            this._ammoType.penetration = 0.4f;
            this._type = "gun";
            this._sprite = new SpriteMap(GetPath("empgrenade"), 16, 16, false);
            this.graphic = this._sprite;
            this.center = new Vec2(7f, 8f);
            this.collisionOffset = new Vec2(-4f, -5f);
            this.collisionSize = new Vec2(8f, 11f);
            base.bouncy = 0.4f;
            this.friction = 0.05f;
            this._editorName = "EMP Grenade";
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

        public void CreateExplosion()
        {
            IEnumerable<Gun> weapons = new List<Gun>();
            EMPExplosion emp = new EMPExplosion(base.x, base.y);
            Level.Add(emp);
            base.Fondle(emp);
            weapons = Level.CheckCircleAll<Gun>(this.position, 100f);
            Graphics.FlashScreen();
            if (isServerForObject)
            {
                foreach (Gun g in weapons)
                {
                    if (g.active)
                    {
                        if (g.owner != null && g.owner is Duck)
                        {
                            Duck d = g.owner as Duck;
                            d.ThrowItem();
                            if (Network.isActive)
                            {
                                Send.Message(new NMThrowItem(d));
                            }
                        }
                        g.hSpeed = Rando.Float(-3f, 3f);
                        g.vSpeed = Rando.Float(-2f, -5f);
                        Level.Add(new EMPAction(0, 0, g));
                    }
                }
            }
            if (Network.isActive)
            {
                this._netExplodeSound.Play();
            }
            SFX.Play(Mod.GetPath<RSMod>("SFX\\empExplosion"));
            Level.Remove(this);
            base._destroyed = true;
        }

        public override void Update()
        {
            if (!this._pin)
            {
                this._timer -= 0.01f;
                if (this._timer <= 0f)
                {
                    this.CreateExplosion();
                }
            }
            this._sprite.frame = (this._pin ? 0 : 1);
            base.Update();
        }

        public StateBinding _netExplodeSoundBinding = new NetSoundBinding("_netExplodeSound");
        public NetSoundEffect _netExplodeSound = new NetSoundEffect(new string[]
        {
            Mod.GetPath<RSMod>("SFX\\empExplosion")
        })
        {
            volume = 1f
        };
        public StateBinding _timerBinding = new StateBinding("_timer", -1, false, false);
        public StateBinding _pinBinding = new StateBinding("_pin", -1, false, false);
    }
}
