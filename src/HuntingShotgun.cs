using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    [EditorGroup("Random Stuff|Guns|Standard")]
    class HuntingShotgun : Shotgun
    {
        public HuntingShotgun(float xpos, float ypos): base(xpos, ypos)
        {
            this.ammo = 3;
            this._loaderSprite = new SpriteMap(Mod.GetPath<RSMod>("hShotgunLoader"), 8, 8, false);
            this._loaderSprite.center = new Vec2(4f, 4f);
            this.graphic = new Sprite(Mod.GetPath<RSMod>("hShotgun"));
        }
    }
}
