using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    class NMToggleSwitch : NMDuckNetworkEvent
    {
        public bool tog;
        public NMToggleSwitch()
        {
        }

        public NMToggleSwitch(bool b)
        {
            this.tog = b;
        }

        public override void Activate()
        {
            ToggleSwitch.isOn = this.tog;
            base.Activate();
        }
    }
}
