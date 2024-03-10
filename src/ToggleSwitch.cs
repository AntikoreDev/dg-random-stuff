using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    [EditorGroup("Random Stuff|Blocks|Toggle")]
    class ToggleSwitch : BlockBase, IPathNodeBlocker
    {
        public ToggleSwitch(float xpos, float ypos) : base(xpos, ypos)
        {
            this.sprite = new SpriteMap(GetPath("toggleSwitch"), 16, 16);
            this.graphic = sprite;
            this._editorName = "Toggle Switch";
            this.maxUses = -1;
            this._netHitSound = new NetSoundEffect(new string[]
            {
                Mod.GetPath<RSMod>("SFX\\toggleSwitch")
            })
            {
                volume = 1f
            };
        }

        public override void Initialize()
        {
            ToggleSwitch.isOn = false;
            base.Initialize();
        }

        public override void Update()
        {
            base.Update();
            this.sprite.frame = (isOn ? 1 : 0);
        }

        public override void UpdateCharging()
        {
            this._hit = false;
        }

        public override void Activate(MaterialThing with)
        {
            ToggleSwitch.Toggle();
            SFX.Play(Mod.GetPath<RSMod>("SFX\\toggleSwitch"));
            base.Activate(with);
        }

        public static void Toggle()
        {
            if (isOn == true)
            {
                isOn = false;
                if (Network.isActive)
                {
                    Send.Message(new NMToggleSwitch(false));
                }
            }
            else
            {
                isOn = true;
                if (Network.isActive)
                {
                    Send.Message(new NMToggleSwitch(true));
                }
            }
        }

        public static bool isOn;
    }
}
