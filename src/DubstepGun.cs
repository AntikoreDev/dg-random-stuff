using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    [EditorGroup("Random Stuff|Guns")]
    class DubstepGun : Gun
    {
        public SpriteMap _sprite;
        public int _musicPart;
        public bool _musicPlaying;
        public float _musicTimer;
        public int sprframe;
        public int charge;
        public DubstepGun(float xpos, float ypos) : base(xpos, ypos)
        {
            this._sprite = new SpriteMap(Mod.GetPath<RSMod>("dubstepGun"), 24, 17);
            this.ammo = 999;
            this._ammoType = new ATMusicNote();
            this.graphic = this._sprite;
            this._sprite.frame = 0;
            this.sprframe = 0;
            this.center = new Vec2(9f, 12f);
            this.collisionOffset = new Vec2(-7f, -10f);
            this.collisionSize = new Vec2(20f, 12f);
            this._barrelOffsetTL = new Vec2(22f, 8f);
            this.weight = 7.5f;
            this._fireSound = "";
        }

        public override void Fire()
        {
        }

        public override void Update()
        {
            if (this.owner == null)
            {
                this.charge = 0;
            }
            this._sprite.frame = this.sprframe;
            this.ammo = 999;
            this.UpdateMusic();
            this.UpdateSprite();
            base.Update();
        }

        public override void OnHoldAction()
        {
            if (!this._musicPlaying)
            {
                this.charge++;
                if (charge == 120)
                {
                    SFX.Play(Mod.GetPath<RSMod>("SFX\\beep"), 0.7f, 1f);
                    if (Network.isActive)
                    {
                        this._netCharge2Sound.Play();
                    }
                }
                else if (charge == 60)
                {
                    SFX.Play(Mod.GetPath<RSMod>("SFX\\beep"), 0.7f, 0.75f);
                    if (Network.isActive)
                    {
                        this._netCharge1Sound.Play();
                    }
                }
            }
            if (this.charge >= 180)
            {
                if (this.charge == 180)
                {
                    this._musicTimer = 86;
                    this._musicPart = 0;
                }
                this.sprframe = 3;
                this.charge = 181;
                this._musicPlaying = true;
                base.Fire();
            }
            base.OnHoldAction();
        }

        public override void OnReleaseAction()
        {
            this.charge = 0;
            base.OnReleaseAction();
        }

        public void UpdateMusic()
        {
            if (this._musicPlaying)
            {
                bool itsPressed = (this.charge >= 180);
                this._musicTimer++;
                if (this._musicTimer >= 86)
                {
                    if (this._musicPart == 0)
                    {
                        if (itsPressed)
                        {
                            SFX.Play(Mod.GetPath<RSMod>("SFX\\dubstep1"));
                            if (Network.isActive)
                            {
                                this._netDub1Sound.Play();
                            }
                        }
                        else
                        {
                            SFX.Play(Mod.GetPath<RSMod>("SFX\\dubstepFade1"));
                            if (Network.isActive)
                            {
                                this._netDubF1Sound.Play();
                            }
                            this._musicPlaying = false;
                        }
                        this._musicPart = 1;
                    }
                    else
                    {
                        if (itsPressed)
                        {
                            SFX.Play(Mod.GetPath<RSMod>("SFX\\dubstep2"));
                            if (Network.isActive)
                            {
                                this._netDub2Sound.Play();
                            }
                        }
                        else
                        {
                            SFX.Play(Mod.GetPath<RSMod>("SFX\\dubstepFade2"));
                            if (Network.isActive)
                            {
                                this._netDubF2Sound.Play();
                            }
                            this._musicPlaying = false;
                        }
                        this._musicPart = 0;
                    }
                    this._musicTimer = 0;
                }
            }
        }

        public void UpdateSprite()
        {
            if (charge >= 180)
            {
                this.sprframe = 3;
            }
            else if (charge >= 120)
            {
                this.sprframe = 2;
            }
            else if (charge >= 60)
            {
                this.sprframe = 1;
            }
            else
            {
                this.sprframe = 4;
                if (!this._musicPlaying)
                {
                    this.sprframe = 0;
                }
            }
        }
        public StateBinding _netDub1SoundBinding = new NetSoundBinding("_netDub1Sound");
        public NetSoundEffect _netDub1Sound = new NetSoundEffect(new string[]
        {
            Mod.GetPath<RSMod>("SFX\\dubstep1")
        });
        public StateBinding _netDub2SoundBinding = new NetSoundBinding("_netDub2Sound");
        public NetSoundEffect _netDub2Sound = new NetSoundEffect(new string[]
        {
            Mod.GetPath<RSMod>("SFX\\dubstep2")
        });
        public StateBinding _netDubF1SoundBinding = new NetSoundBinding("_netDubF1Sound");
        public NetSoundEffect _netDubF1Sound = new NetSoundEffect(new string[]
        {
            Mod.GetPath<RSMod>("SFX\\dubstepFade1")
        });
        public StateBinding _netDubF2SoundBinding = new NetSoundBinding("_netDubF2Sound");
        public NetSoundEffect _netDubF2Sound = new NetSoundEffect(new string[]
        {
            Mod.GetPath<RSMod>("SFX\\dubstepFade2")
        });
        public StateBinding _netCharge1SoundBinding = new NetSoundBinding("_netCharge1Sound");
        public NetSoundEffect _netCharge1Sound = new NetSoundEffect(new string[]
        {
            Mod.GetPath<RSMod>("SFX\\beep")
        })
        {
            volume = 0.7f,
            pitch = 0.75f,
        };
        public StateBinding _netCharge2SoundBinding = new NetSoundBinding("_netCharge2Sound");
        public NetSoundEffect _netCharge2Sound = new NetSoundEffect(new string[]
        {
            Mod.GetPath<RSMod>("SFX\\beep")
        })
        {
            volume = 0.7f,
            pitch = 1f,
        };
        public StateBinding _netFrameBinding = new StateBinding("sprframe", -1, false, false);
    }
}
