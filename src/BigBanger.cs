using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    [BaggedProperty("isFatal", true), EditorGroup("Random Stuff|Guns")]
    class BigBanger : Gun
    {
        public BigBanger(float xval, float yval) : base(xval, yval)
        {
            this.ammo = 5;
            this.graphic = new Sprite(GetPath("bigBanger"), 0f, 0f);
            this._ammoType = new ATShotgun();
            this.center = new Vec2(11f, 7f);
            this.collisionOffset = new Vec2(-9f, -5f);
            this.collisionSize = new Vec2(20f, 10f);
            this._barrelOffsetTL = new Vec2(23f, 3.5f);
            this._fireSound = "shotgunFire2";
            this._kickForce = 4.8f;
            this._numBulletsPerFire = 6;
            this._ammoType.range = 64f;
            this._ammoType.accuracy = 0.5f;
            this._editorName = "Big Banger";
        }

        public override void Fire()
        {
            base.Fire();
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
