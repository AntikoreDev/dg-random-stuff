using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    //[EditorGroup("Random Stuff|Equipment")]
    class SonicHelmet : Hat
    {
        public Duck _prevDuck;
        public StateBinding _crushedBinding = new StateBinding("crushed", -1, false, false);
        public bool crushed;
        public Holdable _currentGun;
        public bool _hasGun = false;
        public SonicHelmet(float xpos, float ypos) : base(xpos, ypos)
        {
            this._pickupSprite = new Sprite(GetPath("sonicHelmetPickup"), 0f, 0f);
            this._sprite = new SpriteMap(GetPath("sonicHelmet"), 32, 32);
            this.graphic = this._pickupSprite;
            this.center = new Vec2(8f, 8f);
            this.collisionOffset = new Vec2(-5f, -2f);
            this.collisionSize = new Vec2(12f, 8f);
            this._sprite.CenterOrigin();
            this._isArmor = true;
            this._equippedThickness = 3f;
            this._editorName = "Sonic Helmet";
        }

        public virtual void Crush()
        {
            this.crushed = true;
        }

        public override void Update()
        {
            if (this._equippedDuck != null)
            {
                _prevDuck = _equippedDuck;
                if (this._equippedDuck.holdObject != null && this._equippedDuck.holdObject is Gun && this._currentGun == null)
                {
                    Gun a = this.equippedDuck.holdObject as Gun;
                    this._hasGun = true;
                    this._currentGun = this._equippedDuck.holdObject;
                    a._fireWait /= 2.0f;
                }
                if (this._equippedDuck.holdObject == null && this.equippedDuck._lastHoldItem is Gun && this._currentGun != null)
                {
                    Gun a = this.equippedDuck._lastHoldItem as Gun;
                    if (a != null)
                    {
                        a._fireWait *= 2.0f;
                        this._currentGun = null;
                        this._hasGun = false;
                    }
                }
            }
            else if (_prevDuck != null)
            {
                if (this._prevDuck._lastHoldItem != null && this._prevDuck._lastHoldItem is Gun && this._hasGun == true)
                {
                    Gun a = this._prevDuck._lastHoldItem as Gun;
                    a._fireWait *= 2.0f;
                    this._prevDuck = null;
                    this._hasGun = false;
                }
            }
            base.Update();
        }

        public override void Draw()
        {
            int frm = this._sprite.frame;
            this._sprite.frame = (this.crushed ? 1 : 0);
            base.Draw();
            this._sprite.frame = frm;
        }
    }
}
