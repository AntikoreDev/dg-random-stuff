using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    [EditorGroup("Random Stuff|Misc")]
    class DonationBox : Platform
    {
        public DonationBox(float xpos, float ypos) : base(xpos, ypos)
        {
            this.graphic = new Sprite(Mod.GetPath<RSMod>("donationBox"));
            this.center = new Vec2(8f, 8f);
            this.collisionOffset = new Vec2(-8f, -8f);
            this.collisionSize = new Vec2(16f, 16f);
            this._canFlip = false;
        }

        public override void Initialize()
        {
            if (Network.isActive)
            {
                Rando.generator = new Random(NetRand.currentSeed);
            }
            base.Initialize();
        }

        public override void Update()
        {
            List<Coin> _coins = new List<Coin>();
            _coins = Level.CheckRectAll<Coin>(base.topLeft + new Vec2(-1f, -4f), base.bottomRight + new Vec2(1f, -12f)).ToList<Coin>();
            foreach (Coin c in _coins)
            {
                if (c != null && c.owner == null && c.bottom <= this.top && c.vSpeed >= 0)
                {
                    if (c.isLocal)
                    {
                        if (Rando.Int(1, 3) == 3)
                        {
                            this.netGiftSound.Play();
                            Present p = new Present(this.x, this.y - 16);
                            Level.Add(p);
                            base.Fondle(p);
                        }
                        else if (Rando.Int(1, 8) == 8)
                        {
                            this.netGrenadeSound.Play();
                            Grenade g = new Grenade(this.x, this.y - 16);
                            g._pin = false;
                            Level.Add(g);
                            base.Fondle(g);
                        }
                        else
                        {
                            this.netMoneySound.Play();
                        }
                        DonationPopup dp = new DonationPopup(this.x - 4, this.top - 8);
                        Level.Add(dp);
                        base.Fondle(dp);
                    }
                    Level.Remove(c);
                    if (Network.isActive)
                    {
                        Send.Message(new NMDonationBox(c));
                    }
                }   
            }
            base.Update();
        }

        public StateBinding netGrenadeSoundBinding = new NetSoundBinding("netGrenadeSound");
        public NetSoundEffect netGrenadeSound = new NetSoundEffect(new string[]
        {
            "pullPin"
        });
        public StateBinding netGiftSoundBinding = new NetSoundBinding("netGiftSound");
        public NetSoundEffect netGiftSound = new NetSoundEffect(new string[]
        {
            "convert"
        });
        public StateBinding netMoneySoundBinding = new NetSoundBinding("netMoneySound");
        public NetSoundEffect netMoneySound = new NetSoundEffect(new string[]
        {
            Mod.GetPath<RSMod>("SFX\\coinInsert")
        });
    }
}
