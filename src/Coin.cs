using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    [EditorGroup("Random Stuff|Misc")]
    class Coin : Holdable
    {
        public SpriteMap _sprite;
        public Coin(float xpos, float ypos) : base(xpos, ypos)
        {
            this._sprite = new SpriteMap(Mod.GetPath<RSMod>("coin"), 16, 16);
            this._sprite.frame = 0;
            this.graphic = this._sprite;
            this.weight = 1f;
            this._sprite.AddAnimation("Default", 0.25f, true, new int[]
            {
                0,
                1,
                2,
                3,
                4,
                5,
                6,
                7
            });
            this._sprite.AddAnimation("None", 0f, true, new int[]
            {
                1
            });
            this.collisionSize = new Vec2(14f, 14f);
            this.center = new Vec2(8f, 8f);
            base.collideSounds.Add("smallMetalCollide");
            this.collisionOffset = new Vec2(-7f, -7f);
        }

        public override void Update()
        {
            this.angle = 0f;
            if (this.owner != null)
            {
                this._sprite.SetAnimation("None");
            }
            else
            {
                this.angle += (Math.Sign(this.hSpeed) * 85f);
                this._sprite.SetAnimation("Default");
            }
            base.Update();
        }

        public override void Initialize()
        {
            this._sprite.SetAnimation("Default");
            base.Initialize();
        }

        public override void EditorUpdate()
        {
            this._sprite.SetAnimation("Default");
            base.EditorUpdate();
        }
    }
}
