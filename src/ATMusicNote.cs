using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    class ATMusicNote : AmmoType
    {
        public ATMusicNote()
        {
            this.accuracy = 0.75f;
            this.range = 800f;
            this.penetration = 2.5f;
            this.bulletLength = 0.2f;
            this.bulletSpeed = 15f;
            this.bulletThickness = 2f;
            this.sprite = new Sprite(Mod.GetPath<RSMod>("redNotes"), 0f, 0f);
            this.sprite.CenterOrigin();
        }
    }
}
