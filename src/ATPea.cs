using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    class ATPea : AmmoType
    {
        public ATPea()
        {
            this.accuracy = 1f;
            this.range = 200f;
            this.penetration = 0.4f;
            this.bulletLength = 0f;
            this.bulletSpeed = 8f;
            this.bulletThickness = 2f;
            this.sprite = new Sprite(Mod.GetPath<RSMod>("pea"), 0f, 0f);
            this.sprite.CenterOrigin();
        }

        public override void OnHit(bool destroyed, Bullet b)
        {
            if (destroyed)
            {
                this.SpawnParticles(b.x, b.y, b);
            }
            base.OnHit(destroyed, b);
        }

        public void SpawnParticles(float xx, float yy, Bullet b)
        {
            for (int i = 0; i < 6; i++)
            {
                Level.Add(new PeaParticle
                {
                    position = new Vec2(xx, yy),
                    vSpeed = Rando.Float(-1f, 1f),
                    hSpeed = Rando.Float(-1f, 1f)
                });
            }
        }
    }
}
