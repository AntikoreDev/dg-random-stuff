using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    //[EditorGroup("Random Stuff|Equipment")]
    class SniperHelmet : Hat
    {
        public SniperHelmet(float xpos, float ypos) : base(xpos, ypos)
        {
            this._pickupSprite = new Sprite(GetPath("sniperHelmetPickup"), 0f, 0f);
            this._sprite = new SpriteMap(GetPath("sniperHelmet"), 32, 32);
            this.graphic = this._pickupSprite;
            this.center = new Vec2(8f, 8f);
            this.collisionOffset = new Vec2(-5f, -2f);
            this.collisionSize = new Vec2(12f, 8f);
            this._sprite.CenterOrigin();
            this._isArmor = true;
            this._equippedThickness = 3f;
            this._editorName = "Sniper Helmet";
        }

        public override void Update()
        {
            base.Update();
            if (this.owner != null)
            {
                Duck d = this.owner as Duck;
                if (d.inputProfile.Down("UP") && d.holdObject != null)
                {
                    d.holdObstructed = true;
                }
            }
        }
    }
}
