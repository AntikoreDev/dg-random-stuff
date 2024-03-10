using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    class ATImploderBullet : AmmoType
    {
        public SpriteMap spriteMap;
        public ATImploderBullet()
        {
            this.penetration = 1f;
            this.bulletSpeed = 8f;
            this.bulletThickness = 1f;
            this.accuracy = 1f;
            this.range = 1000f;
            this.spriteMap = new SpriteMap(Mod.GetPath<RSMod>("imploderBullet"), 6, 6);
            this.spriteMap.AddAnimation("superbullet", 0.5f, true, new int[]
            {
                0,
                1,
                2,
                3,
            });
            this.spriteMap.SetAnimation("superbullet");
            this.sprite = this.spriteMap;
            this.bulletType = typeof(ImploderBullet);
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
                float vsp = 4f;
                float hsp = 4f;
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        this.DeployBullets(hsp, vsp, b);
                        vsp = -vsp;
                    }
                    hsp = -hsp;
                }
            }
            base.OnHit(destroyed, b);
        }

        public void DeployBullets(float hsp, float vsp, Bullet b)
        {
            QuadLaserBullet quad = new QuadLaserBullet(b.x, b.y, new Vec2(hsp, vsp));
            Level.Add(quad);
            SFX.Play(Mod.GetPath<RSMod>("SFX\\imploderExplode"));
        } 
    }
}
