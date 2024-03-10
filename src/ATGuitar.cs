using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckGame.RSModDEV
{
    class ATGuitar : AmmoType
    {
        public ATGuitar()
        {
            this.sprite = new Sprite(Mod.GetPath<RSMod>("guitarWave"));
            this.sprite.CenterOrigin();
            this.bulletThickness = 0f;
            this.range = 1500f;
            this.bulletSpeed = 8f;
            this.penetration = 99f;
            this.accuracy = 0.97f;
        }
    }
}
