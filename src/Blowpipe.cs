using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    [EditorGroup("Random Stuff|Guns|Misc")]
    class Blowpipe : Gun
    {
        public Blowpipe(float xpos, float ypos) : base(xpos, ypos)
        {
            this.graphic = new Sprite(Mod.GetPath<RSMod>("blowpipe"));
            this.graphic.CenterOrigin();
            this.ammo = 5;
            this.collisionSize = new Vec2(12f, 3f);
            this.center = new Vec2(8f, 8f);
            this.collisionOffset = new Vec2(-5f, -1f);
            this._barrelOffsetTL = new Vec2(14f, 8f);
            this._editorName = "Blowpipe";
        }

        public override void OnPressAction()
        {
            if (this.ammo > 0)
            {
                this.ammo--;
                base.ApplyKick();
                Vec2 pos = this.Offset(base.barrelOffset);
                SFX.Play(Mod.GetPath<RSMod>("SFX\\blowpipeFire"));
                if (isServerForObject)
                {
                    DartArrow dart = new DartArrow(pos.x, pos.y - 2f);
                    dart.vSpeed = this.barrelVector.y * 5f;
                    dart.hSpeed = this.barrelVector.x * 5f;
                    dart.responsibleDuck = this.owner as Duck;
                    Level.Add((Thing)dart);
                }
                SmallSmoke s = SmallSmoke.New(pos.x, pos.y - 2f);
                s.hSpeed += Rando.Float(-0.3f, 0.3f);
                s.vSpeed -= Rando.Float(0.1f, 0.2f);
                Level.Add(s);
            }
            else
            {
                base.DoAmmoClick();
            }
            base.OnPressAction();
        }

        public override void Fire()
        {
        }
    }
}
