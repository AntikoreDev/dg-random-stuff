using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    [EditorGroup("Random Stuff|Guns|Explosive")]
    class GrenadeGun : Gun
    {
        public GrenadeGun(float xpos, float ypos) : base(xpos, ypos)
        {
            this.graphic = new Sprite(Mod.GetPath<RSMod>("grenadeGun"));
            this.center = new Vec2(8, 10);
            this.collisionOffset = new Vec2(-7, -8);
            this.collisionSize = new Vec2(15, 11);
            this._barrelOffsetTL = new Vec2(14, 4);
            this.ammo = 9;
            this._ammoType = new ATGrenadeGun();
            this._editorName = "GrenadeGun";
        }
    }
}
