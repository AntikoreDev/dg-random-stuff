using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    class NMOpenLockedBlock : NMDuckNetworkEvent
    {
        public LockedBlock lockblock;
        public NMOpenLockedBlock()
        {
        }

        public NMOpenLockedBlock(LockedBlock lb)
        {
            this.lockblock = lb;
        }

        public override void Activate()
        {
            if (lockblock != null)
            {
                if (lockblock._blockSlave != null)
                {
                    lockblock._blockSlave.Kill();
                }
            }
            base.Activate();
        }
    }
}
