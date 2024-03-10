using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    class WahBlock : BlockBase
    {
        public WahBlock(float xpos, float ypos) : base(xpos, ypos)
        {
            this.sprite = new SpriteMap(Mod.GetPath<RSMod>("wahBlock"), 16, 16);
            this.graphic = sprite;
            this._canFlip = false;
            this._editorName = "Wah Block";
            this._netHitSound = new NetSoundEffect(new string[]
            {
                Mod.GetPath<RSMod>("SFX\\warp")
            });
        }

        public override void Activate(MaterialThing with)
        {
            base.Activate(with);
        }
    }
}
