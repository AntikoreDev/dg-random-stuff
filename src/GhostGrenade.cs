using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    [EditorGroup("Random Stuff|Guns|Grenades")]
    class GhostGrenade : Grenade
    {
        public SpriteMap sprite;
        public GhostGrenade(float xval, float yval) : base(xval, yval)
        {
            this.sprite = new SpriteMap(GetPath("ghostGrenade"), 16, 16);
            this.graphic = sprite;
            this._editorName = "Ghost Grenade";
            this._timer = 5f;
            this.center = new Vec2(7f, 8f);
            this.collisionOffset = new Vec2(-4f, -5f);
            this.collisionSize = new Vec2(8f, 10f);
        }

        public override void Update()
        {
            this.sprite.frame = (_pin ? 0 : 1);
            if (!_pin)
            {
                if (base.alpha > 0.05)
                {
                    base.alpha -= 0.05f;
                }
                else
                {
                    this.sprite.frame = 2;
                }
            }
            base.Update();
        }
    }
}
