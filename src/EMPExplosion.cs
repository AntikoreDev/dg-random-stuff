using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    public class EMPExplosion : Thing
    {
        public EMPExplosion(float xpos, float ypos) : base(xpos, ypos, null)
        {
            this.center = new Vec2(7.5f, 7.5f);
            this.collisionOffset = new Vec2(-7.5f, -7.5f);
            this.collisionSize = new Vec2(15f, 15f);
            base.depth = -0.5f;
            base.alpha = 1f;
            this.ringtime = 5f;
        }

        public override void Update()
        {
            base.Update();
            this.ringtime = Lerp.Float(this.ringtime, 70f, 5f);
        }

        public override void Draw()
        {
            bool flag = this.ringtime < 70f;
            if (flag)
            {
                Graphics.DrawCircle(this.position, this.ringtime, Color.LightBlue * 0.014f * (70f - this.ringtime), 0.1f * (70f - this.ringtime), 3f, 32);
            }
            base.Draw();
        }

        private SpriteMap _sprite;
        private SinWave _pulse = 0.18f;
        public float len = 2f;
        public float ringtime = 5f;
        private Sprite _burnGlow;
        private Sprite _ringo;
    }
}

