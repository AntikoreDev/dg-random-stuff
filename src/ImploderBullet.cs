using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    class ImploderBullet : Bullet
    {
        public Tex2D _beem;
        public float _thickness;
        public ImploderBullet(float xval, float yval, AmmoType type, float ang = -1f, Thing owner = null, bool rbound = false, float distance = -1f, bool tracer = false, bool network = true) : base(xval, yval, type, ang, owner, rbound, distance, tracer, network)
        {
            this._beem = Content.Load<Tex2D>(Mod.GetPath<RSMod>("rainbowTrail"));
            this._thickness = type.bulletThickness;
        }

        public override void Draw()
        {
            if (this.bulletDistance > 0.1f)
            {
                float length = (this.drawStart - this.drawEnd).length;
                float num = 0f;
                float num2 = 1f / (length / 8f);
                float num3 = 0f;
                float num4 = 8f;
                for (; ; )
                {
                    bool flag = false;
                    if (num + num4 > length)
                    {
                        num4 = length - Maths.Clamp(num, 0f, 99f);
                        flag = true;
                    }
                    num3 += num2;
                    Graphics.DrawTexturedLine(this._beem, this.drawStart + this.travelDirNormalized * num, this.drawStart + this.travelDirNormalized * (num + num4), Color.White * num3, this._thickness, 0.6f);
                    if (flag)
                    {
                        break;
                    }
                    num += 8f;
                }
            }
            base.Draw();
        }
    }
}
