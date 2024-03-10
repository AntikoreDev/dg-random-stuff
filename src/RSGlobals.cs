using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    class RSGlobals
    {
        public static double LengthdirX(float length, float dir)
        {
            return length * Math.Cos(dir * Math.PI / -180);
        }

        public static double LengthdirY(float length, float dir)
        {
            return length * Math.Sin(dir * Math.PI / -180);
        }
    }
}
