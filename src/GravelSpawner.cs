using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    class GravelSpawner : Thing
    {
        public Gravel myGravel;
        public float delay;
        public float _timer;
        public GravelSpawner(float xpos, float ypos) : base(xpos, ypos)
        {
            this.graphic = new Sprite(Mod.GetPath<RSMod>("gravelSpawner"));
            base.depth = -0.5f;
            this.center = new Vec2(8f, 8f);
            this.collisionSize = new Vec2(16f, 16f);
            this.collisionOffset = new Vec2(-8f, -8f);
        }

        public override void Initialize()
        {
            if (!Network.isActive || (Network.isActive && Network.isServer))
            {
                if (!(Level.current is Editor))
                {
                    this.Spawn();
                }
            }
            base.Initialize();
        }

        public override void Update()
        {
            if (!Network.isActive || (Network.isActive && Network.isServer))
            {
                if (this.myGravel == null)
                {
                    this._timer++;
                    if (this._timer >= this.delay)
                    {
                        this.Spawn();
                        this._timer = 0;
                    }
                }
            }
            base.Update();
        }

        public void Spawn()
        {
            Gravel g = new Gravel(this.x, this.y);
            g.parent = this;
            g.alpha = 0f;
            Level.Add(g);
            this.myGravel = g;
        }

        public StateBinding _gravelBinding = new StateBinding("myGravel", -1, false, false);
        public StateBinding positionBinding = new StateBinding("position", -1, false, false);
    }
}
