using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    class CloudBlock : MaterialThing, IPlatform
    {
        public int state;
        public int timer;
        public float fscale;
        public CloudBlock(float xpos, float ypos) : base(xpos, ypos)
        {
            this.graphic = new Sprite(Mod.GetPath<RSMod>("cloudy"));
            this.center = new Vec2(16f, 8f);
            this.collisionOffset = new Vec2(-16f, -8f);
            this.collisionSize = new Vec2(32f, 16f);
            this.fscale = 0;
            this.xscale = fscale;
            this.yscale = fscale;
            this.solid = false;
        }

        public override void Update()
        {
            this.xscale = fscale;
            this.yscale = fscale;
            base.Update();
            switch (this.state)
            {
                case 0: 
                    this.Starting(); 
                    break;
                case 1: 
                    this.Waiting(); 
                    break;
                case 2: 
                    this.Closing(); 
                    break;
                default: 
                    this.Closing(); 
                    break;   
            }
        }

        public void Starting()
        {
            this.fscale += 0.1f;
            if (this.fscale >= 1f)
            {
                this.state = 1;
                this.solid = true;
                if (isServerForObject)
                {
                    this.SpawnParticles();
                    if (Network.isActive)
                    {
                        Send.Message(new NMCloudBlockParticles(this));
                    }
                }
            }
        }

        public void Waiting()
        {
            this.timer++;
            if (timer > 600) 
            {
                this.solid = false;
                this.state = 2;
            }
        }

        public void Closing()
        {
            this.fscale -= 0.1f;
            if (this.fscale <= 0f)
            {
                Level.Remove(this);
            }
        }

        public void SpawnParticles()
        {
            for (int i = 0; i < 15; i++)
            {
                float yy = this.bottom;
                float xx = Rando.Float(this.bottomLeft.x, this.topRight.x);
                SmallCloud sc = new SmallCloud(xx, yy);
                Level.Add(sc);
            }
        }
    }
}
