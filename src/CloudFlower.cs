using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    [EditorGroup("Random Stuff|Equipment")]
    class CloudFlower : Hat
    {
        public Duck myDuck;
        public int clouds = 4;
        public int cloudedFrames;
        public int ungroundedFrames;
        public bool hasClouded;
        public CloudFlower(float xpos, float ypos) : base(xpos, ypos)
        {
            this._pickupSprite = new Sprite(GetPath("cloudFlower"), 0f, 0f);
            this._sprite = new SpriteMap(GetPath("cloudHelmet"), 32, 32);
            this.graphic = this._pickupSprite;
            this.center = new Vec2(8f, 8f);
            this.collisionOffset = new Vec2(-4f, -4f);
            this.collisionSize = new Vec2(12f, 12f);
            this._sprite.CenterOrigin();
            this._isArmor = false;
            this._equippedThickness = 3f;
            this._editorName = "Cloud Flower";
        }

        public override void Update()
        {
            base.Update();
            if (this.equippedDuck != null)
            {
                Duck d = this.equippedDuck;
                this.alpha = 1f;
                this.myDuck = d;
                if (d.grounded)
                {
                    this.cloudedFrames = 0;
                    this.ungroundedFrames = 0;
                    this.hasClouded = false;
                }
                if (!d.grounded)
                {
                    this.ungroundedFrames++;
                    if (this.ungroundedFrames >= 5 && d.inputProfile.Pressed("JUMP"))
                    {
                        if (!this.hasClouded)
                        {
                            this.hasClouded = true;
                            d.vSpeed = -3f;
                            Level.Add(new CloudBlock(d.x, d.bottom));
                            this.clouds--;
                            if (this.clouds <= 0)
                            {
                                this.alpha = 0f;
                                d.Unequip(this);
                                Level.Remove(this);
                            }
                        }
                    }
                }
                if (this.hasClouded)
                {
                    this.cloudedFrames++;
                    if (this.cloudedFrames < 75)
                    {
                        d._sprite.flipH = !d._sprite.flipH;
                    } 
                }
            }
            else
            {
                this.ungroundedFrames = 0;
                this.cloudedFrames = 0;
                if (this.myDuck != null)
                {
                    Duck d = this.myDuck;
                    if (d.dead)
                    {
                        Level.Remove(this);
                    }
                    this.alpha = 0f;
                    this.canPickUp = false;
                    if (d.ragdoll == null)
                    {
                        if (d.HasEquipment(typeof(Hat)))
                        {
                            Level.Remove(this);
                        }
                        else
                        {
                            d.Equip(this, false);
                        }
                    }
                }            
            }
        }
    }
}
