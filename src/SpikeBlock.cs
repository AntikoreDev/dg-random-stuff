using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    class SpikeBlock : Thing
    {
        public SpriteMap _sprite;
        public bool canKill;
        public SpikeTrap myOwner;
        public SpikeBlock(float xpos, float ypos) : base(xpos, ypos)
        {
            this._sprite = new SpriteMap(GetPath("Spikes"),16,16);
            this.center = new Vec2(8f, 8f);
            this.collisionSize = new Vec2(16f, 16f);
            this.collisionOffset = new Vec2(-8f, -8f);
            this._sprite.frame = 0;
            this.depth = 1f;
            this.graphic = _sprite;
        }

        public override void Update()
        {
            this._touchObjects = Level.CheckRectAll<PhysicsObject>(new Vec2(this.topLeft.x + 4, this.topLeft.y + 4), new Vec2(this.bottomRight.x - 4, this.bottomRight.y - 4)).ToList<PhysicsObject>();
            if (this._touchObjects.Count > 0 && this.canKill)
            {
                foreach (PhysicsObject p in _touchObjects)
                {
                    if (p != null)
                    { 
                        if (p is IAmADuck)
                        {
                            MaterialThing mt = (MaterialThing)p;
                            mt.Destroy(new DTImpale(this));
                        }
                    }
                }
            }
            if (this.myOwner._moment > 50) { this._sprite.frame = 3; }
            else if (this.myOwner._moment > 40) { this._sprite.frame = 2; }
            else if (this.myOwner._moment > 0) { this._sprite.frame = 1; }
            else { this._sprite.frame = 0; }
            base.Update();
        }

        private List<PhysicsObject> _touchObjects = new List<PhysicsObject>();
    }
}
