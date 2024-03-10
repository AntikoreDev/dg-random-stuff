using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    [EditorGroup("Random Stuff|Guns")]
    class MissileBoy : Gun
    {
        public MissileBoy(float xval, float yval) : base(xval, yval)
        {
            this.ammo = 10;
            this._ammoType = new ATMissileBoy();
            this._type = "gun";
            this.graphic = new Sprite(Mod.GetPath<RSMod>("missileBoy"), 0f, 0f);
            this.center = new Vec2(12f, 9f);
            this.collisionOffset = new Vec2(-10f, -7f);
            this.collisionSize = new Vec2(23f, 11f);
            this._barrelOffsetTL = new Vec2(25f, 6f);
            this._fireSound = "missile";
            this._kickForce = 2f;
            this._manualLoad = true;
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
                            this._netLoad.Play(1f, 0f);
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

        public StateBinding _loadStateBinding = new StateBinding("_loadState", -1, false, false);
        public StateBinding _angleOffsetBinding = new StateBinding("_angleOffset", -1, false, false);
        public StateBinding _netLoadBinding = new NetSoundBinding("_netLoad");
        public NetSoundEffect _netLoad = new NetSoundEffect(new string[]
        {
        "loadSniper"
        });
        public int _loadState = -1;
        public int _loadAnimation = -1;
        public float _angleOffset;
    }
}