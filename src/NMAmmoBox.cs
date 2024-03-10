using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    class NMAmmoBox : NMDuckNetworkEvent
    {
        public int direction;
        public AmmoBox ammoBox;
        public NMAmmoBox()
        {
        }

        public NMAmmoBox(int dir, AmmoBox ab)
        {
            this.direction = dir;
            this.ammoBox = ab;
        }

        public override void Activate()
        {
            if (ammoBox != null)
            {
                AmmoPopUp popup = new AmmoPopUp(ammoBox.x, ammoBox.y - 8f);
                Level.Add(popup);
                int i;
                for (i = 0; i < 10; i += 1)
                {
                    Level.Add(new MagnumShell(ammoBox.x, ammoBox.y)
                    {
                        hSpeed = -direction * (1.5f + Rando.Float(1f))
                    });
                }
                Level.Remove(ammoBox);
            }
            SFX.Play(Mod.GetPath<RSMod>("SFX\\ammoBox"));
            base.Activate();
        }
    }
}
