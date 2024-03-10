using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    [EditorGroup("Random Stuff|Equipment")]
    public class DWBoots : Boots
    {
        public int ammo;
        private bool _keyShoot;
        private bool isGrounded;
        public DWBoots(float xpos, float ypos) : base(xpos, ypos)
        {
            this._pickupSprite = new Sprite(GetPath("dwBootsPickup"), 0f, 0f);
            this._sprite = new SpriteMap(GetPath("dwBoots"), 32, 32, false);
            this.graphic = this._pickupSprite;
            this.center = new Vec2(8f, 8f);
            this.collisionOffset = new Vec2(-6f, -6f);
            this.collisionSize = new Vec2(12f, 13f);
            this._equippedDepth = 1;
            this._editorName = "Pistol Boots";
            this.ammo = 4;
            this._keyShoot = false;
        }
        public override void Update()
        { 
            if (this.equippedDuck != null)
            {
                if (this.equippedDuck.grounded == true)
                {
                    this.ammo = 4;
                }
                if (this.equippedDuck.inputProfile.Down("JUMP") && !this._keyShoot)
                {
                    this.CheckForShoot();
                    this._keyShoot = true;
                }
                else if (this._keyShoot && !this.equippedDuck.inputProfile.Down("JUMP"))
                {
                    this._keyShoot = false;
                }
            }
            base.Update();
        }

        public void CheckForShoot()
        {
            if (this.equippedDuck != null && this.equippedDuck.ragdoll == null && this.equippedDuck.sliding == false)
            {
                Duck d = equippedDuck as Duck;
                if (!d.grounded && d.framesSinceJump > 1)
                { 
                    if (ammo > 0)
                    {
                        if (isServerForObject)
                        {
                            if (Network.isActive)
                            {
                                this._netShootSound.Play();
                            }
                            SFX.Play("shotgunFire", 0.8f);
                            this.ammo--;
                            this.equippedDuck.vSpeed = -3f;
                            DWBullet db = new DWBullet(0, 0);
                            db.shooter = d;
                            db.vSpeed = 5f;
                            Vec2 bulletPos = new Vec2((d.bottomLeft.x - ((d.bottomLeft.x - d.topRight.x) / 2)), d.bottomLeft.y);
                            db.x = bulletPos.x;
                            db.y = bulletPos.y;
                            Level.Add(db);
                        }
                    }
                }
            }
        }

        public StateBinding _netShootSoundBinding = new NetSoundBinding("_netShootSound");
        public NetSoundEffect _netShootSound = new NetSoundEffect(new string[]
        {
            "shotgunFire"
        })
        {
            volume = 1f
        };
        public StateBinding _ammoBinding = new StateBinding("ammo", -1, false, false);
    }
}
