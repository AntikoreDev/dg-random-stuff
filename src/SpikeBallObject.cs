using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    class SpikeBallObject : Thing
    {
        public SpikeBall parent;
        public float speed; 
        public float distance;
        public float _timer;
        public SpikeBallObject(float xpos, float ypos) : base(xpos, ypos)
        {
            this.graphic = new Sprite(Mod.GetPath<RSMod>("spikeBall"));
            this.depth = 3f;
            this.center = new Vec2(9.5f, 9.5f);
            this.collisionSize = new Vec2(19f, 19f);
            this.collisionOffset = new Vec2(-9.5f, -9.5f);
        }
        
        public override void Update()
        {
            if (parent != null)
            {
                this.position = new Vec2(parent.x + (Convert.ToSingle(Math.Cos(_timer))) * this.distance, parent.y + (Convert.ToSingle(Math.Sin(_timer))) * this.distance);
                this.angle = this._timer * 2f;
                this._timer += this.speed;
            }
            IEnumerable<IAmADuck> iaad = Level.CheckRectAll<IAmADuck>(this.topLeft, this.bottomRight);
            foreach (IAmADuck p in iaad)
            {
                if (p != null)
                {
                    if (p is IAmADuck)
                    {
                        MaterialThing mt = p as MaterialThing;
                        mt.Destroy(new DTImpale(this));
                    }
                }
            }
            base.Update();
        }

        public StateBinding _angleBinding = new StateBinding("angle", -1, false, false);
        public StateBinding _positionBinding = new InterpolatedVec2Binding("position");
        public StateBinding _timerBinding = new StateBinding("_timer", -1, false, false);
    }
}
