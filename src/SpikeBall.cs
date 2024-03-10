using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Xml.Linq;
using DuckGame;

namespace DuckGame.RSModDEV
{
    [EditorGroup("Random Stuff|Hazards")]
    class SpikeBall : Thing
    {
        public EditorProperty<int> distance = new EditorProperty<int>(30, null, 1f, 298f, 1f, null, false, false);
        public EditorProperty<int> speed = new EditorProperty<int>(30, null, 0f, 98f, 1f, null, false, false);
        public EditorProperty<int> startpos = new EditorProperty<int>(0, null, 0f, 359f, 1f, null, false, false);
        public EditorProperty<bool> direction = new EditorProperty<bool>(false, null, 0f, 1f, 0.1f, null, false, false);
        public EditorProperty<bool> collider = new EditorProperty<bool>(false, null, 0f, 1f, 0.1f, null, false, false);
        public SpikeBall(float xpos, float ypos) : base(xpos, ypos)
        {
            this.graphic = new Sprite(Mod.GetPath<RSMod>("spikeBall"));
            base.depth = 3f;
            this.center = new Vec2(9.5f, 9.5f);
            this.collisionSize = new Vec2(19f, 19f);
            this.collisionOffset = new Vec2(-9.5f, -9.5f);
            this.alpha = 0.5f;
        }

        public override void Draw()
        {
            if (Level.current is Editor)
            {
                base.Draw();
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            if (Level.current is Editor)
            {
                return;
            }
            if (this.collider)
            {
                DelayBlockHitter dd = new DelayBlockHitter(base.x, base.y, this);
                Level.Add(dd);
            } 
            SpikeBallObject sb = new SpikeBallObject(this.x, this.y);
            sb.parent = this;
            sb._timer = this.startpos;
            float sp = this.speed;
            if (this.direction)
            {
                this.speed = -this.speed;
            }
            sb.speed = (this.speed / 1000f);
            sb.distance = this.distance;
            if (!Network.isActive || (Network.isActive && Network.isServer))
            {
                Level.Add(sb);
                base.Fondle(sb);
            }
        }

        /*
        public override BinaryClassChunk Serialize()
        {
            BinaryClassChunk binaryClassChunk = base.Serialize();
            binaryClassChunk.AddProperty("speed", this.speed);
            return binaryClassChunk;
        }

        public override bool Deserialize(BinaryClassChunk node)
        {
            base.Deserialize(node);
            this.speed = node.GetProperty<float>("speed");
            return true;
        }


        public override XElement LegacySerialize()
        {
            XElement xelement = base.LegacySerialize();
            xelement.Add(new XElement("speed", Change.ToString(this.speed)));
            return xelement;
        }

        public override bool LegacyDeserialize(XElement node)
        {
            base.LegacyDeserialize(node);
            XElement xelement = node.Element("speed");
            if (xelement != null)
            {
                this.speed = Convert.ToSingle(xelement.Value);
            }
            return true;
        }

        public override ContextMenu GetContextMenu()
        {
            EditorGroupMenu editorGroupMenu = base.GetContextMenu() as EditorGroupMenu;
            editorGroupMenu.AddItem(new ContextSlider("speed", null, new FieldBinding(this, "speed", 0f, 10f, 0.1f), 0.1f, null, false, null));
            return editorGroupMenu;
        }
        */
    }
}
