using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    class ATPaintball : AmmoType
    {
        public ATPaintball()
        {
            this.accuracy = 1f;
            this.penetration = 0.01f;
            this.bulletSpeed = 7.5f;
            this.rangeVariation = 0f;
            this.speedVariation = 0f;
            this.range = 2000f;
            this.rebound = false;
            this.affectedByGravity = true;
            this.deadly = true;
            this.weight = 1f;
            this.bulletThickness = 2f;
            this.bulletColor = Color.White;
            this.immediatelyDeadly = true;
        }
    }
}
