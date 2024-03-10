using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    class Quadastrophe : Thing, ITeleport
    {
        private int spriteFrame;
        private int spriteFrameTimer;
        public float hsp;
        public float vsp;
        public int life;
        public SpriteMap _sprite;
        public Quadastrophe(float xpos, float ypos) : base(xpos, ypos)
        {
            this._sprite = new SpriteMap(GetPath("quadastrophe"), 10, 10);
            this.graphic = _sprite;
            this._sprite.frame = 0;
            this.spriteFrame = 0;
            this.spriteFrameTimer = 0;
            this.center = new Vec2(4.5f, 4.5f);
            this.collisionSize = new Vec2(10f, 10f);
            this.collisionOffset = new Vec2(-4.5f, -4.5f);
            this._sprite.AddAnimation("Default", 1f, true, new int[]
            {
                0,
                1,
                2,
                3,
            });
            this.vsp = 0;
            this.hsp = 0;
            this.life = 20;
            this._sprite.SetAnimation("Default");
        }
        public override void Update()
        {
            this.life--;
            if (this.life <= 0)
            {
                this.LeQuadastrophe();
            }
            base.Update();
            this.x += hsp;
            this.y += vsp;
        }

        public void LeQuadastrophe()
        {
            if (isServerForObject)
            { 
                float cx = this.x;
                float cy = this.y - 2f;
                Level.Add(new ExplosionPart(cx, cy, true));
                int num = 6;
                if (Graphics.effectsLevel < 2)
                {
                    num = 3;
                }
                for (int i = 0; i < num; i++)
                {
                    float dir = (float)i * 60f + Rando.Float(-10f, 10f);
                    float dist = Rando.Float(12f, 20f);
                    ExplosionPart ins = new ExplosionPart(cx + (float)(Math.Cos((double)Maths.DegToRad(dir)) * (double)dist), cy - (float)(Math.Sin((double)Maths.DegToRad(dir)) * (double)dist), true);
                    Level.Add(ins);
                }
                for (int j = 0; j < 18; j++)
                {
                    float dir2 = (float)j * 22.5f;
                    Vec2 v = new Vec2((float)Math.Cos((double)Maths.DegToRad(dir2)), (float)(-(float)Math.Sin((double)Maths.DegToRad(dir2))));
                    v.x *= 2f;
                    v.y *= 2f;
                    Rando.Float(8f, 14f);
                    QuadLaserBullet ins2 = new QuadLaserBullet(this.x, this.y, v);
                    Level.Add(ins2);
                }
                Level.Remove(this);
                if (Network.isActive)
                {
                    this._netExplodeSound.Play();
                }
                SFX.Play("laserBlast", 1f, 0f, 0f, false);
            }
            Graphics.FlashScreen();
        }

        public StateBinding _netExplodeSoundBinding = new NetSoundBinding("_netExplodeSound");
        public NetSoundEffect _netExplodeSound = new NetSoundEffect(new string[]
        {
            "laserBlast"
        })
        {
            volume = 1f
        };
        public StateBinding _positionBinding = new StateBinding("position", -1, false, false);
    }
}
