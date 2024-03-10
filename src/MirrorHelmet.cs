using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    [EditorGroup("Random Stuff|Equipment")]
    class MirrorHelmet : Hat
    {
        public MirrorHelmet(float xpos, float ypos) : base(xpos, ypos)
        {
            this._pickupSprite = new Sprite(GetPath("mirrorHelmetPickup"), 0f, 0f);
            this._sprite = new SpriteMap(GetPath("mirrorHelmet"),32,32);
            this.graphic = this._pickupSprite;
            this.center = new Vec2(8f, 8f);
            this.collisionOffset = new Vec2(-5f, -2f);
            this.collisionSize = new Vec2(12f, 8f);
            this._sprite.CenterOrigin();
            this._isArmor = true;
            this._equippedThickness = 3f;
            this._editorName = "Mirror Helmet";
        }

        public override void Update()
        {
            base.Update();
            this._sprite.frame = 0;
        }
    }
}
