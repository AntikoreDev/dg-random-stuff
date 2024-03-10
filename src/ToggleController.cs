using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    [EditorGroup("Random Stuff|Misc|Controllers")]
    class ToggleController : Thing
    {
        public EditorProperty<float> delay = new EditorProperty<float>(1f, null, 1f, 100f, 0.25f, null, false, false);
        public EditorProperty<float> delay2 = new EditorProperty<float>(1f, null, 1f, 100f, 0.25f, null, false, false);
        public EditorProperty<bool> sound = new EditorProperty<bool>(false, null, 0f, 1f, 0.1f, null, false, false);
        public EditorProperty<bool> startalter = new EditorProperty<bool>(false, null, 0f, 1f, 0.1f, null, false, false);
        public float _timer;
        public float rawdelay1;
        public float rawdelay2;
        public bool delayer;
        public ToggleController(float xpos, float ypos) : base(xpos, ypos)
        {
            this.graphic = new Sprite("swirl");
            this.center = new Vec2(8f, 8f);
            this.collisionSize = new Vec2(16f, 16f);
            this.collisionOffset = new Vec2(-8f, -8f);
        }

        public override void Initialize()
        {
            if (!(Level.current is Editor))
            {
                this.rawdelay1 = this.delay * 60f;
                this.rawdelay2 = this.delay2 * 60f;
                this.delayer = false;
            }
        }

        public override void Update()
        {
            if ((Network.isActive && Network.isServer) || !Network.isActive)
            {
                this._timer++;
                if ((this._timer > this.rawdelay1 && !this.delayer) || (this._timer > this.rawdelay2 && this.delayer))
                {
                    this._timer = 0;
                    if (this.delayer)
                    {
                        this.delayer = false;
                    }
                    else
                    {
                        this.delayer = true;
                    }
                    ToggleSwitch.Toggle();
                    if (sound)
                    {
                        SFX.Play(Mod.GetPath<RSMod>("SFX\\toggleSwitch"));
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
    }
}
