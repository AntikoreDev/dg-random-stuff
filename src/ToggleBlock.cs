using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    [EditorGroup("Random Stuff|Blocks|Toggle")]
    class ToggleBlock : MaterialThing
    {
        public SpriteMap _sprite;
        public bool _open;
        public DelayBlockHitter _blockSlave;
        public bool _slavePlaced;
        public EditorProperty<bool> color = new EditorProperty<bool>(false, null, 0f, 1f, 0.1f, null, false, false);
        public ToggleBlock(float xpos, float ypos) : base(xpos, ypos)
        {
            this._sprite = new SpriteMap(GetPath("toggleBlock"), 16, 16);
            this.graphic = _sprite;
            this.center = new Vec2(8f, 8f);
            this.collisionSize = new Vec2(16f, 16f);
            this.collisionOffset = new Vec2(-8f, -8f);
            this._canFlip = false;
            base.depth = -0.5f;
            this._editorName = "Toggle Block";
        }

        public override void Initialize()
        {
            if (!(Level.current is Editor))
            {
                this._slavePlaced = false;
                if (_blockSlave == null)
                {
                    DelayBlockHitter _s = new DelayBlockHitter(base.x, base.y, this);
                    this._blockSlave = _s;
                    if (ToggleSwitch.isOn == color)
                    {
                        Level.Add(_blockSlave);
                        this._slavePlaced = true;
                    }
                }
                base.Initialize();
            }
        }

        public override void EditorUpdate()
        {
            this.ModifySprite();
        }

        public override void Update()
        {
            this.CheckSides();
            if (ToggleSwitch.isOn != color)
            {
                _open = true;
            }
            else
            {
                _open = false;
            }
            this.ModifySprite();
            if (this._open == false && !_slavePlaced)
            {
                this._blockSlave = new DelayBlockHitter(base.x, base.y, this);
                Level.Add(_blockSlave);
                this._slavePlaced = true;
                this._depth = 1f;
                this.objectsInside = Level.CheckRectAll<PhysicsObject>(base.topLeft, base.bottomRight).ToList<PhysicsObject>();
                foreach (PhysicsObject p in objectsInside)
                {
                    if (p != null && this.sided)
                    {
                        if (p is Duck)
                        {
                            Duck d = p as Duck;
                            d.Kill(new DTImpale(this));
                            d.y -= 25000f;
                        }
                        else if (p is RagdollPart)
                        {
                            RagdollPart r = p as RagdollPart;
                            Duck d = r._doll._duck;
                            d.Kill(new DTImpale(this));
                            d.y -= 25000f;
                        }
                        else if (p is TrappedDuck)
                        {
                            TrappedDuck td = p as TrappedDuck;
                            Duck d = td.captureDuck;
                            if (d != null && !d.dead)
                            {
                                d.Kill(new DTImpale(this));
                            }
                        }
                    }
                }
            }
            else if (this._open == true && _slavePlaced)
            {
                this._blockSlave.Kill();
                this._slavePlaced = false;
                this._depth = -0.5f;
            }
            base.Update();
        }

        public void ModifySprite()
        {
            if (color == false)
            {
                this._sprite.frame = (this._open ? 1 : 0);
            }
            else
            {
                this._sprite.frame = (this._open ? 3 : 2);
            }
        }

        public void CheckSides()
        {
            bool left = Level.CheckPoint<Block>(new Vec2(this.x - 16, this.y), this, null) != null;
            bool right = Level.CheckPoint<Block>(new Vec2(this.x + 16, this.y), this, null) != null;
            bool up = Level.CheckPoint<Block>(new Vec2(this.x, this.y - 16), this, null) != null;
            bool down = Level.CheckPoint<Block>(new Vec2(this.x, this.y + 16), this, null) != null;
            bool upleft = Level.CheckPoint<Block>(new Vec2(this.x - 16, this.y - 16), this, null) != null;
            bool upright = Level.CheckPoint<Block>(new Vec2(this.x + 16, this.y - 16), this, null) != null;
            bool downleft = Level.CheckPoint<Block>(new Vec2(this.x - 16, this.y + 16), this, null) != null;
            bool downright = Level.CheckPoint<Block>(new Vec2(this.x + 16, this.y + 16), this, null) != null;
            this.sided = left && right && up && down && upleft && upright && downleft && downright;
        }

        public bool sided;
        protected List<PhysicsObject> objectsInside = new List<PhysicsObject>();
    }
}
