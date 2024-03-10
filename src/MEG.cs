﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    [EditorGroup("Random Stuff|Misc")]
    class MEG : MaterialThing
    {
        public int animFrame = 0;
        public int animSpeed = 3;
        public int animPointer = 0;
        public SpriteMap _horzSpr;
        public SpriteMap _vertSpr;
        public Vec2 coordTopLeft;
        public Vec2 coordBottomRight;
        public EditorProperty<bool> horizontal = new EditorProperty<bool>(false, null, 0f, 1f, 0.1f, null, false, false);
        public EditorProperty<bool> visual = new EditorProperty<bool>(false, null, 0f, 1f, 0.1f, null, false, false);
        public MEG(float xpos, float ypos) : base(xpos, ypos)
        {
            this._vertSpr = new SpriteMap(GetPath("materialEmancipationGrill"), 16, 16);
            this._horzSpr = new SpriteMap(GetPath("materialEmancipationGrillHorz"), 16, 16);
            this._graphic = this._vertSpr;
            this._editorName = "MEG";
            this.center = new Vec2(8f, 8f);
            this.collisionSize = new Vec2(16f, 16f);
            this.collisionOffset = new Vec2(-8f, -8f);
            this.depth = -0.5f;
            this._canFlip = false;
        }

        public override void Update()
        {
            this.UpdateHitbox();
            this.SpriteUpdate();
            if (!this.visual)
            {
                this._checker = Level.CheckRectAll<Thing>(this.coordTopLeft, this.coordBottomRight).ToList<Thing>();
                if (this._checker.Count > 0)
                {
                    foreach (Thing t in this._checker)
                    {
                        if (t is Holdable && !(t is Duck || t is RagdollPart || t is TrappedDuck))
                        {
                            Holdable h = t as Holdable;
                            this.TryDespawnItem(h);
                        }
                    }
                }
            }
            base.Update();
        }

        public void SpriteUpdate()
        {
            this.graphic = (this.horizontal ? this._horzSpr : this._vertSpr);
            animPointer++;
            if (animPointer >= animSpeed)
            {
                animPointer = 0;
                animFrame++;
                if (animFrame >= 9)
                {
                    animFrame = 0;
                }
            }
            this._horzSpr.frame = this.animFrame;
            this._vertSpr.frame = this.animFrame;

        }

        public override void EditorUpdate()
        {
            this.SpriteUpdate();
            base.EditorUpdate();
        }

        public void TryDespawnItem(Holdable h)
        {
            if (h is Equipment)
            {
                Equipment e = h as Equipment;
                if (e._equippedDuck != null)
                {
                    return;
                }
            }
            if (h.owner != null)
            {
                Duck d = h.owner as Duck;
                if (!d.HasEquipment(typeof(MirrorHelmet)))
                {
                    d.ThrowItem();
                    this.DespawnItem(h);
                }
                return;
            }
            this.DespawnItem(h);     
        }

        public void DespawnItem(Holdable h)
        {
            float xx = h.x; float yy = h.y;
            Level.Remove(h);
            Level.Add(SmallSmoke.New(xx, yy));
            Level.Add(SmallSmoke.New(xx + 4f, yy));
            Level.Add(SmallSmoke.New(xx - 4f, yy));
            Level.Add(SmallSmoke.New(xx, yy + 4f));
            Level.Add(SmallSmoke.New(xx, yy - 4f));
        }

        public void UpdateHitbox()
        {
            if (this.horizontal)
            {
                this.coordTopLeft = new Vec2(this.topLeft.x, this.topRight.y + 7f);
                this.coordBottomRight = new Vec2(this.bottomRight.x, this.bottomRight.y - 7f);
            }
            else
            {
                this.coordTopLeft = new Vec2(this.topLeft.x + 7f, this.topRight.y);
                this.coordBottomRight = new Vec2(this.bottomRight.x - 7f, this.bottomRight.y);
            }
        }

        private List<Thing> _checker = new List<Thing>(); 
    }
}
