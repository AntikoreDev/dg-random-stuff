using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;
 
namespace DuckGame.RSModDEV
{
    [EditorGroup("Random Stuff|Blocks")]
    class LockedBlock : MaterialThing
    {
        public bool _open = false;
        public DelayBlockHitter _blockSlave;
        public SpriteMap _sprite;
        public LockedBlock(float xpos, float ypos) : base(xpos, ypos)
        {
            this._sprite = new SpriteMap(GetPath("lockedBlock"), 16, 16);
            this.graphic = this._sprite;
            base.depth = 0.92f;
            this.center = new Vec2(8f, 8f);
            this.collisionSize = new Vec2(16f, 16f);
            this.collisionOffset = new Vec2(-8f, -8f);
            this._solid = false;
            this._editorName = "Locked Block";
        }

        public override void Initialize()
        {
            if (!(Level.current is Editor))
            {
                this._open = false;
                if (this._blockSlave == null)
                {
                    this._blockSlave = new DelayBlockHitter(base.x, base.y, this);
                    Level.Add(_blockSlave);
                }
            }
            base.Initialize();
        }

        public override void Update()
        {
            this._sprite.frame = (_open ? 1 : 0);
            if (!_open)
            {
                this._nearKeys = Level.CheckRectAll<Key>(base.topLeft + new Vec2(-1f, -1f), base.bottomRight + new Vec2(1f, 1f)).ToList<Key>();
                if (_nearKeys.Count > 0)
                {
                    bool firstKey = false;
                    foreach (Key k in _nearKeys)
                    {
                        if (!firstKey)
                        {
                            firstKey = true;
                            if (!(Level.current is Editor))
                            {
                                Level.Add(SmallSmoke.New(k.x, k.y));
                                Level.Add(SmallSmoke.New(k.x + 4f, k.y));
                                Level.Add(SmallSmoke.New(k.x - 4f, k.y));
                                Level.Add(SmallSmoke.New(k.x, k.y + 4f));
                                Level.Add(SmallSmoke.New(k.x, k.y - 4f));
                            }
                            this.OpenBlock(true);
                            Level.Remove(k);
                        }
                    }
                }
            }
            else
            {
                this._depth = -0.5f;
            }
            base.Update();
        }

        public void OpenBlock(bool starting)
        {
            if (starting)
            {
                if (Network.isActive)
                {
                    this._netOpenSound.Play();
                }
                SFX.Play("deedleBeep");
            }
            if (this._blockSlave != null)
            {
                this._blockSlave.Kill();
            }
            if (Network.isActive)
            {
                Send.Message(new NMOpenLockedBlock(this));
            }
            this.ChainBlocks(base.x + 16f, base.y);
            this.ChainBlocks(base.x - 16f, base.y);
            this.ChainBlocks(base.x, base.y + 16f);
            this.ChainBlocks(base.x, base.y - 16f);
            this._open = true;
        }

        public void ChainBlocks(float xx, float yy)
        {
            LockedBlock i = Level.CheckPoint<LockedBlock>(xx, yy);
            if (i != null && !i._open)
            {
                i._open = true;
                i.OpenBlock(false);
            }
        }
        protected List<Key> _nearKeys = new List<Key>();
        public StateBinding _netOpenSoundBinding = new NetSoundBinding("_netOpenSound");
        public NetSoundEffect _netOpenSound = new NetSoundEffect(new string[]
        {
            "deedleBeep"
        })
        {
            volume = 1f
        };
        public StateBinding _openBinding = new StateBinding("_open", -1, false, false);
    }
}