using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    class ArrowBullet : Bullet
    {
        public ArrowBullet(float xval, float yval, AmmoType type, float ang = -1f, Thing owner = null, bool rbound = false, float distance = -1f, bool tracer = false, bool network = true) : base(xval, yval, type, ang, owner, rbound, distance, tracer, network)
        {
        }

        public override void Update()
        {
            if (this.bulletDistance > 400f)
            {
                this.gravityAffected = true;
            }
            base.Update();
        }

        public override void OnCollide(Vec2 pos, Thing t, bool willBeStopped)
        {
            if (t != null & t is Duck)
            {
                Duck d = t as Duck;
                BlowpipeEffect be = new BlowpipeEffect(0, 0, d);
                Level.Add(be);
                be.dx = d.x;
                be.dy = d.y;
                d.immobilized = true;
            }
            base.OnCollide(pos, t, willBeStopped);
        }
    }
}
