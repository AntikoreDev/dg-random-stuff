using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    //[EditorGroup("Random Stuff|Guns|Grenades")]
    class Lasernade : Grenade
    {
        private SpriteMap _sprite;
        public Lasernade(float xpos, float ypos) : base(xpos, ypos)
        {
            this._sprite = new SpriteMap(Mod.GetPath<RSMod>("lasernade"), 16, 16);
            this.graphic = this._sprite;
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
