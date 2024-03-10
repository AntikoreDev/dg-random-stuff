using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    class ATGrenadeGun : AmmoType
    {
        public ATGrenadeGun()
        {
            this.accuracy = 1f;
            this.range = 850f;
            this.penetration = 0.35f;
            this.bulletSpeed = 6f;
            this.bulletThickness = 2f;
            this.affectedByGravity = true;
            this.sprite = new Sprite(Mod.GetPath<RSMod>("greanade"), 0, 0);
            this.sprite.CenterOrigin();
        }

        public override void OnHit(bool destroyed, Bullet b)
        {
            if (!b.isLocal)
            {
                return;
            }
            if (destroyed)
            {
                Level.Add(new GrenadeExplosion(b.x, b.y));
                //this.CreateExplosion(b.position);
            }
            base.OnHit(destroyed, b);
        }

        public void CreateExplosion(Vec2 pos)
        {
            float x = pos.x;
            float num = pos.y - 2f;
            Level.Add(new ExplosionPart(x, num, true));
            int num2 = 6;
            if (Graphics.effectsLevel < 2)
            {
                num2 = 3;
            }
            for (int i = 0; i < num2; i++)
            {
                float deg = (float)i * 60f + Rando.Float(-10f, 10f);
                float num3 = Rando.Float(12f, 20f);
                ExplosionPart thing = new ExplosionPart(x + (float)(Math.Cos((double)Maths.DegToRad(deg)) * (double)num3), num - (float)(Math.Sin((double)Maths.DegToRad(deg)) * (double)num3), true);
                Level.Add(thing);
            }
        }
    }
}
