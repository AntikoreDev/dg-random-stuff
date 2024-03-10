using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    [EditorGroup("Random Stuff|Misc")]
    class WallDoor : Thing
    {
        public SpriteMap _sprite;
        public Duck duckusing;
        public EditorProperty<int> id = new EditorProperty<int>(1, null, 1f, 28f, 1f, null, false, false);
        public EditorProperty<int> sprite = new EditorProperty<int>(0, null, 0f, 3f, 1f, null, false, false);
        public EditorProperty<int> side = new EditorProperty<int>(1, null, 1f, 3f, 1f, null, false, false);
        public bool _using;
        public int delay;
        public WallDoor(float xpos, float ypos) : base(xpos, ypos)
        {
            this._sprite = new SpriteMap(Mod.GetPath<RSMod>("doors"), 16, 32);
            this.graphic = this._sprite;
            this._sprite.frame = 0;
            this.center = new Vec2(8f, 16f);
            this.collisionOffset = new Vec2(-8f, -16f);
            this.collisionSize = new Vec2(16f, 32f);
            this.editorOffset = new Vec2(0f, 8f);
            this.depth = -3f;
            this._editorName = "Wall Door";
        }

        public override void Initialize()
        {
            if (Network.isActive)
            {
                Rando.generator = new Random(NetRand.currentSeed);
            }
            base.Initialize();
            this.UpdateSprite();
        }

        public override void Update()
        { 
            base.Update();
            if (!this._using)
            {
                List<Duck> ducks = Level.CheckRectAll<Duck>(base.topLeft, base.bottomRight).ToList<Duck>();
                foreach (Duck d in ducks)
                {
                    if (d != null) 
                    {
                        if (d.inputProfile.Pressed("UP") && d.grounded && side.value != 2)
                        {
                            SFX.Play(Mod.GetPath<RSMod>("SFX\\wallDoor"));
                            if (Network.isActive)
                            {
                                this.netOpenSound.Play();
                            }
                            this._using = true;
                            this.delay = 40;
                            this.duckusing = d;
                            d.hSpeed = 0;
                            d.vSpeed = 0;
                            d.sleeping = true;
                            d.immobilized = true;
                            d.x = this.x;
                            d.y = this.y + 1;
                        }
                    }
                }
            }
            else
            {
                if (this.duckusing == null)
                {
                    this._using = false;
                    this.delay = 0;
                    return;
                }
                if (delay > 0)
                {
                    this.delay--;
                    if (delay <= 0)
                    {
                        if (isServerForObject)
                        {
                            this.Teleport(this.duckusing);
                            this.duckusing = null;
                            this._using = false;
                            this.delay = 0;
                        }
                    }
                }
            }
        }

        public override void EditorUpdate()
        {
            this.UpdateSprite();
            base.EditorUpdate();
        }

        public void UpdateSprite()
        {
            this._sprite.frame = this.sprite;
            if (this.sprite == 3 && !(Level.current is Editor))
            {
                this.alpha = 0f;
            }
        }

        public void Teleport(Duck duck)
        {
            if (duck != null)
            {
                float xx = duck.x;
                float yy = duck.y;
                List<WallDoor> available = new List<WallDoor>();
                foreach (WallDoor d in Level.current.things[typeof(WallDoor)])
                {
                    if (d != this && this.id.value == d.id.value && d.side != 3)
                    {
                        available.Add(d);
                    }
                }
                if (available.Count > 0)
                {
                    int rng = Rando.Int(0, available.Count - 1);
                    xx = available[rng].x;
                    yy = available[rng].y;
                }
                if (Network.isActive)
                {
                    Send.Message(new NMWallDoorFix(duck, new Vec2(xx, yy)));
                }
                duck.immobilized = false;
                duck.sleeping = false;
                duck.x = xx;
                duck.y = yy;
            }
        }

        public StateBinding netOpenSoundBinding = new NetSoundBinding("netOpenSound");
        public NetSoundEffect netOpenSound = new NetSoundEffect(new string[]
        {
            Mod.GetPath<RSMod>("SFX\\wallDoor")
        });
        public StateBinding usingBinding = new StateBinding("_using", -1, false, false);
        public StateBinding duckBinding = new StateBinding("duckusing", -1, false, false);
        public StateBinding delayBinding = new StateBinding("delay", -1, false, false);
    }
}
