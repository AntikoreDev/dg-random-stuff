using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    [EditorGroup("Random Stuff|Misc")]
    class ClimbingBlock : MaterialThing, IPlatform
    {
        public EditorProperty<bool> notop = new EditorProperty<bool>(false, null, 0f, 1f, 0.1f, null, false, false);
        public ClimbingBlock(float xpos, float ypos) : base(xpos, ypos)
        {
            this.graphic = new Sprite(Mod.GetPath<RSMod>("climbingBlock"));
            this.center = new Vec2(8f, 8f);
            this.collisionSize = new Vec2(16f, 16f);
            this.collisionOffset = new Vec2(-8f, -8f);
            base.depth = 0.5f;
            this._canFlip = false;
            this._canHaveChance = false;
            this._solid = false;
        }

        public override void Update()
        {
            ClimbingBlock c = Level.CheckPoint<ClimbingBlock>(new Vec2(this.x, this.y - 16), this, null);
            if (c != null || this.notop)
            {
                this._solid = false;
            }
            else
            {
                this._solid = true;
            }
            this.ducks = Level.CheckRectAll<Duck>(new Vec2(this.topLeft.x + 2f, this.topLeft.y + 2f), new Vec2(this.bottomRight.x - 2f, this.bottomRight.y - 2f)).ToList<Duck>();
            foreach (Duck d in ducks)
            {
                if (d != null && d.ragdoll == null)
                {
                    if (d.inputProfile.Pressed("JUMP"))
                    {
                        if (Network.isActive)
                        {
                            this._netJumpSound.Play();
                        }
                        SFX.Play("jump", 0.5f, 0.1f);
                        d.vSpeed = -4f;
                    }
                }
            }
            base.Update();
        }

        public override void Draw()
        {
            if (!(Level.current is Editor))
            {
                return;
            }
            base.Draw();
        }

        public List<Duck> ducks = new List<Duck>();
        public StateBinding _netJumpSoundBinding = new NetSoundBinding("_netJumpSound");
        public NetSoundEffect _netJumpSound = new NetSoundEffect(new string[]
        {
             "jump"
        })
        {
            volume = 0.5f,
            pitch = 0.1f
        };
    }
}
