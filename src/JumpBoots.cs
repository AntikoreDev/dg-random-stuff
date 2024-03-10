using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    [EditorGroup("Random Stuff|Equipment")]
    class JumpBoots : Boots
    {
        public bool canJump;
        public JumpBoots(float xpos, float ypos) : base(xpos, ypos)
        {
            this._pickupSprite = new Sprite(GetPath("jumpBootsPickup"), 0f, 0f);
            this._sprite = new SpriteMap(GetPath("jumpBoots"), 32, 32, false);
            this.graphic = this._pickupSprite;
            this.center = new Vec2(8f, 8f);
            this.collisionOffset = new Vec2(-6f, -6f);
            this.collisionSize = new Vec2(12f, 13f);
            this._equippedDepth = 1;
            this.canJump = true;
        }
        public override void Update()
        {
            if (equippedDuck != null)
            {
                Duck d = equippedDuck;
                if (d.inputProfile.Pressed("JUMP") && this.canJump && !d.grounded && d.framesSinceJump > 1)
                {
                    if (this.isServerForObject)
                    {
                        this.JumpEffects(d);
                        if (Network.isActive)
                        {
                            Send.Message(new NMJumpBootsParticle(d));
                        }
                    }
                    d.vSpeed = -4.2f;
                    if (Network.isActive)
                    {
                        this._netJumpSound.Play();
                    }
                    SFX.Play("jump", 0.5f, 0.1f);
                    this.canJump = false;
                }
                if (d.grounded && !canJump)
                {
                    this.canJump = true;
                }
            }
            else
            {
                this.canJump = true;
            }
            base.Update();
        }

        public void JumpEffects(Thing t)
        {
            for (int i = 0; i < 5; i++)
            {
                float yy = t.bottom;
                float xx = Rando.Float(t.bottomLeft.x, t.topRight.x);
                SmallCloud sc = new SmallCloud(xx, yy);
                Level.Add(sc);
            }
        }
        public StateBinding _netJumpSoundBinding = new NetSoundBinding("_netJumpSound");
        public NetSoundEffect _netJumpSound = new NetSoundEffect(new string[]
        {
             "jump"
        })
        {
            volume = 0.5f,
            pitch = 0.1f
        };
    }
}

