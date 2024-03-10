using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    [EditorGroup("Random Stuff|Guns")]
    class Ace : Gun
    {
        public Ace(float xval, float yval) : base(xval, yval)
		{
            this.ammo = 7;
            this._ammoType = new ATMagnum();
            this._type = "gun";
            this.graphic = new Sprite(GetPath("ace"), 0f, 0f);
            this.center = new Vec2(6f, 7f);
            this.collisionOffset = new Vec2(-5f, -6f);
            this.collisionSize = new Vec2(18f, 9f);
            this._barrelOffsetTL = new Vec2(19f, 3f);
            this._fireSound = "magnum";
            this._kickForce = 4f;
            this._holdOffset = new Vec2(-2f, 1f);
            this._editorName = "Ace";
        }
    }
}
