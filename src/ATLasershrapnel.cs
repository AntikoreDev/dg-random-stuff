using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    class ATLasershrapnel : AmmoType
    {
        public ATLasershrapnel()
        {
            this.bulletType = typeof(LaserBullet);
            this.penetration = 2.5f;
        }
    }
}
