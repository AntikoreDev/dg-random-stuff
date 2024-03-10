using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    [EditorGroup("Random Stuff|Blocks")]
    class Gravel : Block, IPathNodeBlocker
    {
        public SpriteMap _sprite;
        public GravelSpawner parent;
        public bool touched;
        public bool falling;
        public int _timer;
        public float vsp;
        public EditorProperty<bool> spawner = new EditorProperty<bool>(false, null, 0f, 1f, 0.1f, null, false, false);
        public EditorProperty<float> delay = new EditorProperty<float>(10f, null, 1f, 100f, 0.25f, null, false, false);
        public Gravel(float xpos, float ypos) : base (xpos, ypos)
        {
            this._sprite = new SpriteMap(GetPath("gravel"), 16, 16);
            this.graphic = _sprite;
            this._sprite.frame = 0;
            this.center = new Vec2(8f, 8f);
            this.collisionSize = new Vec2(16f, 16f);
            this.collisionOffset = new Vec2(-8f, -8f);
            this._canFlip = false;
        }
        public override void Initialize()
        {
            base.Initialize();
            if (Level.current is Editor)
            {
                return;
            }
            if (!Network.isActive || (Network.isActive && Network.isServer))
            {
                if (this.spawner && this.parent == null)
                {
                    GravelSpawner gs = new GravelSpawner(this.x, this.y);
                    gs.delay = this.delay * 60;
                    Level.Add(gs);
                    base.Fondle(gs);
                    Level.Remove(this);
                }
            }
        }
        public override void Update()
        {
            base.Update();
            List<PhysicsObject> objects = Level.CheckRectAll<PhysicsObject>(base.topLeft + new Vec2(-1f, -4f), base.bottomRight + new Vec2(1f, -12f)).ToList<PhysicsObject>();
            if (falling)
            {
                this.Fall();
                return;
            }
            if (touched)
            {
                this.Shake();
                return;
            }
            if (this.alpha < 1f)
            {
                this.alpha += 0.1f;
            }
            if (objects.Count > 0)
            {
                SFX.Play(Mod.GetPath<RSMod>("SFX\\impactGravel"), 1f, 0f, 0f, false);
                if (Network.isActive)
                {
                    Send.Message(new NMGravelFall(this));
                }  
                this.alpha = 1f;
                this._sprite.frame = 1;
                this.touched = true;
            }
        }
        public void Shake()
        {
            this._timer++;
            this.ShakeAnim();
            if (this._timer >= 45)
            {
                this.graphic.x = base.x;
                this.graphic.y = base.y;
                List<PhysicsObject> objects = Level.CheckRectAll<PhysicsObject>(base.topLeft + new Vec2(-1f, -4f), base.bottomRight + new Vec2(1f, -12f)).ToList<PhysicsObject>();
                foreach (PhysicsObject o in objects)
                {
                    o._sleeping = false;
                }
                if (Network.isActive)
                {
                    this.netFallSound.Play();
                }
                SFX.Play(Mod.GetPath<RSMod>("SFX\\fallingGravel"), 1f, 0f, 0f, false);
                this.solid = false;
                this.falling = true;
            }
        }
        public void Fall()
        {
            this.alpha -= 0.05f;
            this.vsp += 0.2f;
            this.y += this.vsp;
            if (this.alpha <= 0)
            {
                if (this.parent != null)
                {
                    this.parent.myGravel = null;
                }
                Level.Remove(this);
            }
        }

        public void ShakeAnim()
        {
            this.graphic.position.x = (base.x + Convert.ToSingle(Rando.Int(-1, 1)));
            this.graphic.position.y = (base.y + Convert.ToSingle(Rando.Int(-1, 1)));
        }
        public override void Draw()
        {
            if (this._graphic != null)
            {
                if (!this.touched || this.falling)
                {
                    this._graphic.position = this.position;
                }
                this._graphic.alpha = base.alpha;
                this._graphic.angle = this.angle;
                this._graphic.depth = base.depth;
                this._graphic.scale = base.scale;
                this._graphic.center = this.center;
                this._graphic.Draw();
            }
        }

        public StateBinding positionBinding = new StateBinding("position", -1, false, false);
        public StateBinding netFallSoundBinding = new NetSoundBinding("netFallSound");
        public NetSoundEffect netFallSound = new NetSoundEffect(new string[]
        {
            Mod.GetPath<RSMod>("SFX\\fallingGravel")
        });
        public StateBinding netCrackSoundBinding = new NetSoundBinding("netCrackSound");
        public NetSoundEffect netCrackSound = new NetSoundEffect(new string[]
        {
            Mod.GetPath<RSMod>("SFX\\impactGravel")
        });
    }
}