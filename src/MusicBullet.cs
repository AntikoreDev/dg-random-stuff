using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    class MusicBullet : Bullet
    {
        public MusicBullet(float xval, float yval, AmmoType type, float ang = -1f, Thing owner = null, bool rbound = false, float distance = -1f, bool tracer = false, bool network = false) : base(xval, yval, type, ang, owner, rbound, distance, tracer, network)
        {
        }
    }
}
