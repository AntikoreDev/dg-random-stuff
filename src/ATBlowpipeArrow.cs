using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    class ATBlowpipeArrow : AmmoType
    {
        public ATBlowpipeArrow()
        {
            this.sprite = new Sprite(Mod.GetPath<RSMod>("arrow"));
            this.sprite.CenterOrigin();
            this.range = 500f;
            this.bulletThickness = 2f;
            this.bulletSpeed = 8f;
            this.accuracy = 1f;
            this.affectedByGravity = false;
            this.deadly = true;
            this.bulletType = typeof(ArrowBullet);
        }
    }
}
