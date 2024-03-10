using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    [EditorGroup("Random Stuff|Guns|Explosive")]
    class Minesniper : Gun
    {
        public float loadState = -1;
        public int loadProgress = 0;
        public Minesniper(float xpos, float ypos) : base(xpos, ypos)
        {
            this.ammo = 4;
            this._ammoType = new ATSniper();
            this._type = "gun";
            this.graphic = new Sprite(GetPath("minesniper"), 0f, 0f);
            this.center = new Vec2(16f, 4f);
            this.collisionOffset = new Vec2(-8f, -4f);
            this.collisionSize = new Vec2(16f, 8f);
            this._barrelOffsetTL = new Vec2(30f, 3f);
            this._fireSound = "sniper";
            this._kickForce = 2f;
            this.laserSight = true;
            this._laserOffsetTL = new Vec2(32f, 3.5f);
            this._manualLoad = true;
        }

        public override void Fire()
        {
            if (ammo > 0)
            {
                this.loaded = false;
                if (isServerForObject)
                {
                    Mine m = new Mine(0, 0);
                    m.position = Offset(this._barrelOffsetTL);
                    m.vSpeed = this.barrelVector.y * 12f;
                    m.hSpeed = this.barrelVector.x * 12f;
                    m._pin = false;
                    m._armed = true;
                    Level.Add((Thing)m);
                    base.ApplyKick();
                }
                if (Network.isActive)
                {
                    this._netShootSound.Play();
                }
                SFX.Play("sniper");
            }
            else
            {
                base.DoAmmoClick();
            }
        }

        public override void Update()
        {
            base.Update();
            if (this._loadState > -1)
            {
                if (this.owner == null)
                {
                    if (this._loadState == 3)
                    {
                        this.loaded = true;
                    }
                    this._loadState = -1;
                    this._angleOffset = 0f;
                    this.handOffset = Vec2.Zero;
                }
                if (this._loadState == 0)
                {
                    if (Network.isActive)
                    {
                        if (base.isServerForObject)
                        {
                            this._netLoadSound.Play(1f, 0f);
                        }
                    }
                    else
                    {
                        SFX.Play("loadSniper", 1f, 0f, 0f, false);
                    }
                    this._loadState++;
                }
                else if (this._loadState == 1)
                {
                    if (this._angleOffset < 0.16f)
                    {
                        this._angleOffset = MathHelper.Lerp(this._angleOffset, 0.2f, 0.15f);
                    }
                    else
                    {
                        this._loadState++;
                    }
                }
                else if (this._loadState == 2)
                {
                    this.handOffset.x = this.handOffset.x + 0.4f;
                    if (this.handOffset.x > 4f)
                    {
                        this._loadState++;
                        this.Reload(true);
                        this.loaded = false;
                    }
                }
                else if (this._loadState == 3)
                {
                    this.handOffset.x = this.handOffset.x - 0.4f;
                    if (this.handOffset.x <= 0f)
                    {
                        this._loadState++;
                        this.handOffset.x = 0f;
                    }
                }
                else if (this._loadState == 4)
                {
                    if (this._angleOffset > 0.04f)
                    {
                        this._angleOffset = MathHelper.Lerp(this._angleOffset, 0f, 0.15f);
                    }
                    else
                    {
                        this._loadState = -1;
                        this.loaded = true;
                        this._angleOffset = 0f;
                    }
                }
            }
            if (this.loaded && this.owner != null && this._loadState == -1)
            {
                this.laserSight = true;
                return;
            }
            this.laserSight = false;
        }

        public override void Draw()
        {
            float ang = this.angle;
            if (this.offDir > 0)
            {
                this.angle -= this._angleOffset;
            }
            else
            {
                this.angle += this._angleOffset;
            }
            base.Draw();
            this.angle = ang;
        }


        public override void OnPressAction()
        {
            if (this.loaded)
            {
                base.OnPressAction();
                return;
            }
            if (this.ammo > 0 && this._loadState == -1)
            {
                this._loadState = 0;
                this._loadAnimation = 0;
            }
        }

        public StateBinding _netShootSoundBinding = new NetSoundBinding("_netShootSound");
        public NetSoundEffect _netShootSound = new NetSoundEffect(new string[]
        {
            "sniper"
        })
        {
            volume = 1f
        };
        public StateBinding _netLoadSoundBinding = new NetSoundBinding("_netLoadSound");
        public NetSoundEffect _netLoadSound = new NetSoundEffect(new string[]
        {
            "loadSniper"
        })
        {
            volume = 1f
        };
        public StateBinding _loadStateBinding = new StateBinding("_loadState", -1, false, false);
		public StateBinding _angleOffsetBinding = new StateBinding("_angleOffset", -1, false, false);
		public int _loadState = -1;
		public int _loadAnimation = -1;
		public float _angleOffset;
    }
}
