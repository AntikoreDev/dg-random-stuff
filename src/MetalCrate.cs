using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    [EditorGroup("Random Stuff|Props")]
    class MetalCrate : Holdable, IPlatform
    {
        public Sprite _sprite;
        public MetalCrate(float xpos, float ypos) : base(xpos, ypos)
        {
            this._sprite = new Sprite(GetPath("metalCrate"));
            this.graphic = this._sprite;
            this._maxHealth = 99999f;
            this._hitPoints = 99999f;
            this.flammable = 0f;
            this.center = new Vec2(8f, 8f);
            this.collisionOffset = new Vec2(-8f, -8f);
            this.collisionSize = new Vec2(16f, 16f);
            base.depth = -0.5f;
            this.thickness = 2f;
            this.weight = 5f;
            this.buoyancy = 1f;
            this.physicsMaterial = PhysicsMaterial.Metal;
            this._editorName = "Metal Crate";
            base.collideSounds.Add("barrelThud");
        }

        public override void Update()
        {
            this._hitPoints = 99999f;
            base.Update();
        }
    }
}
