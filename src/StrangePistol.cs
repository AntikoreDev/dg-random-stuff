using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    [EditorGroup("Random Stuff|Guns|Misc")]
    class StrangePistol : Gun
    {
        public int shootType = 0;
        public int prevType = 0;
        public SpriteMap _sprite;
        public float LoadState = 1f;
        public int LoadProgress = 0;
        public StrangePistol(float xval, float yval) : base(xval, yval)
        {
            this._sprite = new SpriteMap(GetPath("strangePistol"), 32, 32);
            this._sprite.frame = 0;
            this.graphic = _sprite;
            this._editorName = "Strange Pistol";
            this.center = new Vec2(16f, 17f);
            this.collisionOffset = new Vec2(-8f, -4f);
            this.collisionSize = new Vec2(16f, 8f);
            this._barrelOffsetTL = new Vec2(24f, 16f);
            this._fireSound = "shotgun";
            this._kickForce = 2f;
            this._holdOffset = new Vec2(2f, 0f);
            this.ammo = 15;
        }

        public override void Update()
        {
            if (LoadState == 0.5f)
            {
                LoadProgress--;
                if (this.LoadProgress <= 0)
                {
                    _sprite.frame = 0;
                    LoadState = 1f;
                    if (Network.isActive)
                    {
                        this._netClickSound.Play();
                    }
                    SFX.Play("click");
                }
            }
            base.Update();
        }

        public override void OnPressAction()
        {
            if (ammo > 0)
            {
                if (LoadState == 0f)
                {
                    this.LoadState = 0.5f;
                    this.LoadProgress = 30;
                    if (Network.isActive)
                    {
                        this._netLoadSound.Play();
                    }
                    SFX.Play("shotgunLoad");
                }
                else if (LoadState == 1f)
                {
                    this._numBulletsPerFire = 1;
                    this.LoadState = 0f;
                    this._sprite.frame = 1;
                    this._bulletColor = Color.White;
                    while (shootType == prevType)
                    {
                        shootType = Rando.Int(1, 6);
                    }
                    prevType = shootType;
                    if (shootType == 1)
                    {
                        this._ammoType = new ATShrapnel();
                        this._ammoType.range = 170f;
                        this._ammoType.accuracy = 0.1f;
                        this._ammoType.penetration = 0.4f;
                        this._fireSound = "shotgun";
                        this.loseAccuracy = 0.2f;
                        this.maxAccuracyLost = 0.8f;
                        this._kickForce = 1f;
                        this.Fire();
                    }
                    if (shootType == 2)
                    {
                        this._ammoType = new ATShrapnel();
                        this._ammoType.range = 170f;
                        this._ammoType.accuracy = 0.5f;
                        this._numBulletsPerFire = 2;
                        this._ammoType.penetration = 0.4f;
                        this.loseAccuracy = 0.2f;
                        this.maxAccuracyLost = 0.8f;
                        this._fireSound = "shotgun";
                        this._kickForce = 1.2f;
                        this.Fire();
                    }
                    if (shootType == 3)
                    {
                        this._ammoType = new ATMissile();
                        this.loseAccuracy = 0.1f;
                        this.maxAccuracyLost = 0.6f;
                        this._fireSound = "missile";
                        this._kickForce = 5f;
                        this.Fire();

                    }
                    if (shootType == 4)
                    {
                        this._ammoType = new AT9mm();
                        this._ammoType.accuracy = 0.8f;
                        this._ammoType.penetration = 0.4f;
                        this._fireSound = "pistolFire";
                        this.loseAccuracy = 0.2f;
                        this.maxAccuracyLost = 0.8f;
                        this._kickForce = 3f;
                        this.Fire();
                    }
                    if (shootType == 5)
                    {
                        this._ammoType = new ATMagnum();
                        this._kickForce = 3f;
                        this._fireSound = "magnum";
                        this.Fire();
                    }
                    if (shootType == 6)
                    {
                        this._kickForce = 3f;
                        this._ammoType = new ATGrenade();
                        this._fireSound = "deepMachineGun";
                        this._bulletColor = Color.White;
                        this.Fire();
                    }
                }
            }
            else
            {
                base.DoAmmoClick();
            }
        }

        public StateBinding _netLoadSoundBinding = new NetSoundBinding("_netLoadSound");
        public NetSoundEffect _netLoadSound = new NetSoundEffect(new string[]
        {
            "shotgunLoad"
        })
        {
            volume = 1f
        };
        public StateBinding _netClickSoundBinding = new NetSoundBinding("_netClickSound");
        public NetSoundEffect _netClickSound = new NetSoundEffect(new string[]
        {
            "click"
        })
        {
            volume = 1f
        };
        public StateBinding _loadStateBinding = new StateBinding("LoadState", -1, false, false);
        public StateBinding _loadProgressBinding = new StateBinding("LoadProgress", -1, false, false);
        public StateBinding prevTypeBinding = new StateBinding("prevType", -1, false, false);

    }
}
