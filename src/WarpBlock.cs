using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    [EditorGroup("Random Stuff|Blocks")]
    class WarpBlock : BlockBase
    {
        public WarpBlock(float xpos, float ypos) : base(xpos, ypos)
        {
            this.sprite = new SpriteMap(Mod.GetPath<RSMod>("warpBlock"), 16, 16);
            this.graphic = sprite;
            this._canFlip = false;
            this._editorName = "Warp Trigger";
            this._netHitSound = new NetSoundEffect(new string[]
            {
                Mod.GetPath<RSMod>("SFX\\warp")
            });
        }

        public override void Activate(MaterialThing with)
        {
            foreach (PhysicsObject p in Level.current.things[typeof(PhysicsObject)])
            {
                if (p != null)
                {
                    this.Warp(p, p.x, p.y);
                }
            }
            EMPExplosion emp = new EMPExplosion(base.x, base.y);
            Level.Add(emp);
            base.Activate(with);
        }

        public void Warp(PhysicsObject p, float oldx, float oldy)
        {
            bool goodPosition = false;
            bool neverTped = false;
            int tries = 0;
            while (!goodPosition)
            {
                if (tries >= 10)
                {
                    this.position = new Vec2(oldx, oldy);
                    neverTped = true;
                    goodPosition = true;
                }
                float xx = Rando.Float(Level.activeLevel.topLeft.x, Level.activeLevel.bottomRight.x);
                float yy = Rando.Float(Level.activeLevel.topLeft.y - (p.bottom - p.top), Level.activeLevel.bottomRight.y);
                p.position = new Vec2(xx, yy);
                List<Block> checkObjects = Level.CheckRectAll<Block>(p.topLeft, p.bottomRight).ToList<Block>();
                if (checkObjects.Count == 0)
                {
                    goodPosition = true;
                }
                tries++;
            }
            if (!neverTped)
            {
                if (isServerForObject)
                {
                    this.WarpEffect(oldx, oldy);
                    if (Network.isActive)
                    {
                        Send.Message(new NMWarpParticles(oldx, oldy));
                    }
                }
            }
            p.sleeping = false;
        }

        public void WarpEffect(float xx, float yy)
        {
            for (int i = 0; i < 15; i++)
            {
                WarpParticle wp = new WarpParticle(xx, yy);
                Level.Add(wp);
                base.Fondle(wp);
            }
        }
    }
}
