using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    [EditorGroup("Random Stuff|Blocks")]
    class EMPBlock : BlockBase
    {
        public EMPBlock(float xpos, float ypos) : base(xpos, ypos)
        {
            this.sprite = new SpriteMap(Mod.GetPath<RSMod>("empblock"), 16, 16);
            this.graphic = sprite;
            this._canFlip = false;
            this._editorName = "EMP Trigger";
            this._netHitSound = new NetSoundEffect(new string[]
            {
                Mod.GetPath<RSMod>("SFX\\empExplosion")
            });
        }

        public override void Activate(MaterialThing with)
        {
            EMPExplosion emp = new EMPExplosion(base.x, base.y);
            Level.Add(emp);
            base.Fondle(emp);
            foreach (Gun g in Level.current.things[typeof(Gun)])
            {
                if (g.active)
                {
                    if (g.owner != null && g.owner is Duck)
                    {
                        Duck d = g.owner as Duck;
                        d.ThrowItem();
                        if (Network.isActive)
                        {
                            Send.Message(new NMThrowItem(d));
                        }
                    }
                    g.hSpeed = Rando.Float(-3f, 3f);
                    g.vSpeed = Rando.Float(-2f, -5f);
                    Level.Add(new EMPAction(0, 0, g));
                }
            }
            base.Activate(with);
        }
    }
}
