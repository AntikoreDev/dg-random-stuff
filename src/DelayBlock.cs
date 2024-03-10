using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    [EditorGroup("Random Stuff|Blocks")]
    class DelayBlock : MaterialThing
    {
        public DelayBlockHitter _blockSlave;
        public SpriteMap _sprite;
        public float timer;
        public bool ended;
        public EditorProperty<float> time = new EditorProperty<float>(1f, null, 1f, 300f, 0.1f, null, false, false);
        public float mtime;
        public EditorProperty<bool> reversed = new EditorProperty<bool>(false, null, 0f, 1f, 0.1f, null, false, false);
        public bool _opened;
        public DelayBlock(float xpos, float ypos) : base(xpos, ypos)
        {
            this._sprite = new SpriteMap(GetPath("delayBlock"), 16, 16);
            this.graphic = _sprite;
            this.center = new Vec2(8f, 8f);
            this.collisionSize = new Vec2(16f, 16f);
            this.collisionOffset = new Vec2(-8f, -8f);
            this._canFlip = false;
            base.depth = 0.92f;
            this.solid = false;
            this._editorName = "Delay Block";
        }

        public override void Initialize()
        {
            if (!(Level.current is Editor))
            {
                DelayBlockHitter _s = new DelayBlockHitter(base.x, base.y, this);
                this._blockSlave = _s;
                if (!reversed)
                {
                    Level.Add(_blockSlave);
                }
                this._opened = false;
                this.solid = true;
                if (reversed == true)
                {
                    this._opened = true;
                    this.solid = false;
                    this.depth = -0.5f;
                }
                this.mtime = (time * 60f);
                base.Initialize();
            }
        }

        public override void Update()
        {
            base.Update();
            this._sprite.frame = (this._opened ? 1 : 0);
            if (!ended)
            {
                timer++;
                if (timer >= mtime)
                {
                    if (_blockSlave != null)
                    {
                        if (reversed)
                        {
                            this.objectsInside = Level.CheckRectAll<PhysicsObject>(base.topLeft, base.bottomRight).ToList<PhysicsObject>();
                            if (this.objectsInside.Count != 0)
                            {
                                return;
                            }
                            _opened = false;
                            this.depth = 0.92f;
                            Level.Add(_blockSlave);
                        }
                        else
                        {
                            _opened = true;
                            this.depth = -0.5f;
                            this._blockSlave.Kill();
                        }
                        this.ended = true;
                        if (Network.isActive)
                        {
                            this._netOpenSound.Play();
                        }
                        SFX.Play("deedleBeep", 0.5f, 0f, 0f, false);
                    }
                }
            }
        }
        protected List<PhysicsObject> objectsInside = new List<PhysicsObject>();
        public StateBinding _timerBinding = new StateBinding("timer", -1, false, false);
        public StateBinding _netOpenSoundBinding = new NetSoundBinding("_netOpenSound");
        public NetSoundEffect _netOpenSound = new NetSoundEffect(new string[]
        {
            "deedleBeep"
        })
        {
            volume = 1f
        };
    }
}