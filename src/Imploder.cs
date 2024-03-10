using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    [EditorGroup("Random Stuff|Guns|Lethal")]
    class Imploder : Gun
    {
        public SpriteMap _sprite;
        public Imploder(float xpos, float ypos) : base(xpos, ypos)
        {
            this.ammo = 6;
            this._ammoType = new ATImploderBullet();
            this._sprite = new SpriteMap(Mod.GetPath<RSMod>("imploder"), 20, 13);
            this.graphic = this._sprite;
            this._sprite.frame = 0;
            this._fireSound = Mod.GetPath<RSMod>("SFX\\imploderShoot");
            this._kickForce = 5f;
            this._sprite.AddAnimation("default", 0.3f, true, new int[]
            {
                0,
                1,
                2,
                3,
                4,
                5,
                6,
                7,
                8,
                9,
                10,
                11,
                12,
            });
            this._sprite.SetAnimation("default");
            this._editorName = "|DGYELLOW|Quad Deagle";
            this.center = new Vec2(9f, 5f);
            this.collisionOffset = new Vec2(-9f, -5f);
            this.collisionSize = new Vec2(18f, 11f);
            this._barrelOffsetTL = new Vec2(19f, 3f);
        }
    }
}
