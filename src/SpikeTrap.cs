using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    [EditorGroup("Random Stuff|Blocks")]
    class SpikeTrap : Block, IPathNodeBlocker
    {
        public int _moment;
        private int _maxMom = 60;
        private int _minMom = 0;
        public SpikeBlock _spikes;
        public int framesOpen = 0;
        public int framesTotOpen = 0;
        public SpikeTrap(float xpos, float ypos) : base(xpos, ypos)
        {
            this.graphic = new Sprite(GetPath("spikeTrap"));
            this.center = new Vec2(8f, 8f);
            this.collisionSize = new Vec2(16f, 16f);
            this.collisionOffset = new Vec2(-8f, -8f);
            this._canFlip = false;
            this._editorName = "Spike Trap";
            this._moment = 0;
            this.depth = 2f;
            this._spikes = null;
        }

        public override void Initialize()
        {
            if (!(Level.current is Editor))
            {
                if (this._spikes == null)
                {
                    this._spikes = new SpikeBlock(0, 0);
                    this._spikes.position = new Vec2(this.x, this.y - 16);
                    this._spikes.myOwner = this;
                    Level.Add(_spikes);
                }
            }
            base.Initialize();
        }
        public override void Update()
        {
            this._aboveList = Level.CheckRectAll<PhysicsObject>(base.topLeft + new Vec2(-1f, -4f), base.bottomRight + new Vec2(1f, -12f)).ToList<PhysicsObject>();
            int q = 0;
            foreach (PhysicsObject p in _aboveList)
            {
                if (p is IAmADuck)
                {
                    q++;
                }
            }
            bool b = (q > 0);
            if (!b)
            {
                if (this._moment < 30 && this._moment > 15)
                {
                    this._moment = 15;
                }
                this._moment--;
            }
            else
            {
                this._moment++;
            }
            if (this._moment > 50)
            {
                this._spikes.canKill = true;
                this.framesTotOpen++;
            }
            else
            {
                this._spikes.canKill = false;
                this.framesTotOpen = 0;
            }
            if (_moment > 0)
            {
                this.framesOpen++;
            }
            else
            {
                this.framesOpen = 0;
            }
            this.PlaySound();
            if (this._moment > this._maxMom) { this._moment = this._maxMom; }
            if (this._moment < this._minMom) { this._moment = this._minMom; }
            base.Update();
        }

        public void PlaySound()
        {
            if (!Network.isActive || (Network.isActive && Network.isServer))
            {
                if (this.framesOpen == 1)
                {
                    SFX.Play(Mod.GetPath<RSMod>("SFX\\spikeTrap"), 0.5f, -0.1f);
                    if (Network.isActive)
                    {
                        this._netShowSound.Play();
                    }
                }
                if (this.framesTotOpen == 1)
                {
                    SFX.Play(Mod.GetPath<RSMod>("SFX\\spikeTrap"), 1f, 0f);
                    if (Network.isActive)
                    {
                        this._netOpenSound.Play();
                    }
                }
            }
        }
 
        private List<PhysicsObject> _aboveList = new List<PhysicsObject>();
        public StateBinding _netOpenSoundBinding = new NetSoundBinding("_netOpenSound");
        public NetSoundEffect _netOpenSound = new NetSoundEffect(new string[]
        {
            Mod.GetPath<RSMod>("SFX\\spikeTrap")
        });
        public StateBinding _netShowSoundBinding = new NetSoundBinding("_netShowSound");
        public NetSoundEffect _netShowSound = new NetSoundEffect(new string[]
        {
            Mod.GetPath<RSMod>("SFX\\spikeTrap")
        })
        {
            volume = 0.5f,
            pitch = -0.1f
        };
    }
}
