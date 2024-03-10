using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    [EditorGroup("Random Stuff|Props")]
    class CompanionCube : Crate
    {
        private SpriteMap _sprite;
        public bool _isCompanion;
        private Duck duckSoul;
        public CompanionCube(float xpos, float ypos) : base(xpos, ypos)
        {
            this.collisionOffset = new Vec2(-7.5f, -7.5f);
            this.collisionSize = new Vec2(16f, 16f);
            base.depth = -0.5f;
            this.thickness = 3f;
            this.weight = 5f;
            this.buoyancy = 1f;
            this._sprite = new SpriteMap(GetPath("companionCube"), 16, 16);
            this.graphic = this._sprite;
            this._sprite.frame = 0;
            this.center = new Vec2(7.5f, 7.5f);
            this._editorName = "Companion Cube";
            base.collideSounds.Add("barrelThud");
        }

        public override void Update()
        {
            if (this.duckSoul != null && (this.duckSoul.dead && this.duckSoul.framesSinceKilled <= 1) && !this._isCompanion)
            {
                this._isCompanion = true;
            }
            this.duckSoul = null;
            this._sprite.frame = (this._isCompanion ? 1 : 0);
            base.Update();
        }

        public override void OnSoftImpact(MaterialThing with, ImpactedFrom from)
        {
            if (with is Duck && from == ImpactedFrom.Bottom)
            {
                Duck d = with as Duck;
                this.duckSoul = d;
            }
            base.OnSoftImpact(with, from);
        }

        protected override bool OnDestroy(DestroyType type)
        {
            if (type is DTIncinerate && this._isCompanion)
            {
                if (!Music.currentSong.Contains("TurretOrchestra"))
                {
                    if (Network.isActive)
                    {
                        Send.Message(new NMPlayMusic("TurretOrchestra"));
                    }
                    Music.Play("TurretOrchestra", false, 0f);
                }
            }
            base.OnDestroy(type);
            return true;
        }

        public StateBinding _isCompanionBinding = new StateBinding("_isCompanion", -1, false, false);
        public StateBinding duckSoulBinding = new StateBinding("duckSoul", -1, false, false);

    }
}
