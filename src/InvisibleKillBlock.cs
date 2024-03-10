using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    [BaggedProperty("canSpawn", false), EditorGroup("Random Stuff|Misc")]
    class InvisibleKillBlock : MaterialThing
    {
        public SpriteMap sprite;
        public InvisibleKillBlock(float xpos, float ypos) : base(xpos, ypos)
        {
            this.sprite = new SpriteMap(GetPath("invKillBlock"), 16, 16);
            this.sprite.frame = 0;
            this.graphic = sprite;
            this.center = new Vec2(8f, 8f);
            this.collisionOffset = new Vec2(-8f, -8f);
            this.collisionSize = new Vec2(16f, 16f);
            this._editorName = "Invisible Death Block";
        }

        public override void Draw()
        {
            if (Level.current is Editor)
            {
                base.Draw();
            }
        }

        public override void OnSoftImpact(MaterialThing with, ImpactedFrom from)
        {
            if (with != null)
            {
                if (with is IAmADuck)
                {
                    MaterialThing mt = (MaterialThing)with;
                    mt.Destroy(new DTImpale(this));
                }
            }
        }
    }
}
