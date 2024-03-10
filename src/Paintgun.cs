using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    //[EditorGroup("Random Stuff|Guns")]
    class Paintgun : Gun
    {
        public Paintgun(float xval, float yval) : base(xval, yval)
        {
            this.graphic = new Sprite(GetPath("paintGun"));
            this.ammo = 16;
            this._ammoType = new ATPaintball();
            this.loseAccuracy = 0.1f;
            this.maxAccuracyLost = 0.4f;
            this._editorName = "PaintGun";
            this._kickForce = 3f;
            this.center = new Vec2(15f, 18f);
            this.collisionOffset = new Vec2(-10f, -10f);
            this.collisionSize = new Vec2(26f, 12f);
            this._barrelOffsetTL = new Vec2(28f, 15f);
        }

        public override void OnPressAction()
        {
            if (this.ammo > 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    Vec2 vec = this.Offset(new Vec2(-9f, 0f));
                    Vec2 hitAngle = base.barrelVector.Rotate(Rando.Float(1f), Vec2.Zero);
                    Level.Add(Spark.New(vec.x, vec.y, hitAngle, 0.1f));
                }
            }
            else
            {
                base.DoAmmoClick();
            }
            base.Fire();
        }
    }
}
