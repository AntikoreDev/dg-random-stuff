using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    [EditorGroup("Random Stuff|Guns|Standard")]
    class PeaShooter : Gun
    {
        public PeaShooter(float xpos, float ypos) : base(xpos, ypos)
        {
            this.ammo = 7;
            this._ammoType = new ATPea();
            this.graphic = new Sprite(Mod.GetPath<RSMod>("peaShooter"));
            this.center = new Vec2(5f, 8f);
            this.collisionOffset = new Vec2(-4f, -3f);
            this.collisionSize = new Vec2(14f, 7f);
            this._barrelOffsetTL = new Vec2(14f, 8f);
            this._manualLoad = false;
            this._fullAuto = false;
            this._editorName = "Peashooter";
            this._fireSound = Mod.GetPath<RSMod>("SFX\\eggPop");
        }
    }
}
