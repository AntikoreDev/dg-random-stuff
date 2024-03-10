using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    [EditorGroup("Random Stuff|Blocks")]
    class GoldenBlock : BlockBase, IPathNodeBlocker
    {
        public GoldenBlock(float xpos, float ypos) : base(xpos, ypos)
        {
            this.sprite = new SpriteMap(GetPath("goldenBlock"), 16, 16);
            this.graphic = sprite;
            this._canFlip = false;
            this.timesUsed = 0;
            this.maxUses = 1;
            this._editorName = "Golden Box";
        }

        public override void Activate(MaterialThing with)
        {
            bool a1 = (with is Quadastrophic);
            if (!a1)
            {
                if (with is Duck)
                {
                    Duck d = with as Duck;
                    Gun item = d.holdObject as Gun;
                    bool a2 = (item is Quadastrophic);
                    if (d != null && item != null && !a2)
                    {
                        item.infiniteAmmoVal = true;
                        item.infinite.value = true;
                        base.Activate(with);
                    }
                }
                else if (with is Gun)
                {
                    Gun item = with as Gun;
                    if (item != null)
                    {
                        item.infiniteAmmoVal = true;
                        item.infinite.value = true;
                        base.Activate(with);
                    }
                }
            } 
        }     
    }
}

