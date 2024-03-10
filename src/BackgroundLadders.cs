using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    [EditorGroup("Random Stuff|Scenario|Background")]
    public class BackgroundLadders : BackgroundTile
    {
        public BackgroundLadders(float xpos, float ypos) : base(xpos, ypos)
        {
            this.graphic = new SpriteMap(Mod.GetPath<RSMod>("ladders"), 16, 16, false);
            this.center = new Vec2(8f, 8f);
            this.collisionSize = new Vec2(16f, 16f);
            this.collisionOffset = new Vec2(-8f, -8f);
            base.depth = 1.2f;
            this._editorName = "Ladders";
        }
    }
}
