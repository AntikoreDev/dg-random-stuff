using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.RSModDEV
{
    [EditorGroup("Random Stuff|Guns|Grenades")]
    class Dynamite : Holdable
    {
        public int timer = 180;
        public bool triggered;
        public bool didExplode;
        public bool impacted;
        public Dynamite(float xval, float yval) : base(xval, yval)
        {
            this._graphic = new Sprite(Mod.GetPath<RSMod>("dynamiteTest"));
            this.center = new Vec2(8f, 8f);
            this._bouncy = 0.4f;
            this.friction = 0.05f;
            this.collisionOffset = new Vec2(-8f, -8f);
            this.collisionSize = new Vec2(16f, 16f);
        }

        public override void OnPressAction()
        {
            this.triggered = true;
            base.OnPressAction();
            SFX.PlaySynchronized("pullPin");
        }

        public override void OnImpact(MaterialThing with, ImpactedFrom from)
        {
            if (!impacted && with is Block)
            {
                timer = 30;
                impacted = true;
            } 
            base.OnImpact(with, from);
        }

        public override void Update()
        {
            if (triggered)
            {
                this.timer--;
                if (timer <= 0 && !this.didExplode)
                {
                    this.Explode(this.position, 50f);
                }
            }   
            base.Update();
        }

        public void DoExplosionAnim(Vec2 pos)
        {
            float cx = pos.x;
            float cy = pos.y - 2f;
            Level.Add(new ExplosionPart(cx, cy, true));
            int num = 6;
            if (Graphics.effectsLevel < 2)
            {
                num = 3;
            }
            for (int i = 0; i < num; i++)
            {
                float dir = (float)i * 60f + Rando.Float(-10f, 10f);
                float dist = Rando.Float(12f, 20f);
                Level.Add(new ExplosionPart(cx + (float)(Math.Cos((double)Maths.DegToRad(dir)) * (double)dist), cy - (float)(Math.Sin((double)Maths.DegToRad(dir)) * (double)dist), true));
            }
            SFX.Play("explode", 1f, 0f, 0f, false);
            RumbleManager.AddRumbleEvent(pos, new RumbleEvent(RumbleIntensity.Heavy, RumbleDuration.Short, RumbleFalloff.Medium, RumbleType.Gameplay));
        }

        //This should happen when the dynamite is going to explode.
        public void Explode(Vec2 pos, float radius)
        {
            didExplode = true;
            this.DoExplosionAnim(pos);
            SFX.PlaySynchronized("explode");
            RumbleManager.AddRumbleEvent(pos, new RumbleEvent(RumbleIntensity.Heavy, RumbleDuration.Short, RumbleFalloff.Medium, RumbleType.Gameplay));
            ATMissileShrapnel shrap = new ATMissileShrapnel();
            shrap.MakeNetEffect(pos, false);
            Random rand = null;
            if (Network.isActive && this.isLocal)
            {
                rand = Rando.generator;
                Rando.generator = new Random(NetRand.currentSeed);
            }
            List<Bullet> firedBullets = new List<Bullet>();
            for (int i = 0; i < 12; i++)
            {
                float dir = (float)i * 30f - 10f + Rando.Float(20f);
                shrap = new ATMissileShrapnel();
                shrap.range = 15f + Rando.Float(5f);
                Vec2 shrapDir = new Vec2((float)Math.Cos((double)Maths.DegToRad(dir)), (float)Math.Sin((double)Maths.DegToRad(dir)));
                Bullet bullet = new Bullet(pos.x + shrapDir.x * 8f, pos.y - shrapDir.y * 8f, shrap, dir, null, false, -1f, false, true);
                bullet.firedFrom = this;
                firedBullets.Add(bullet);
                Level.Add(bullet);
                Level.Add(Spark.New(pos.x + Rando.Float(-8f, 8f), pos.y + Rando.Float(-8f, 8f), shrapDir + new Vec2(Rando.Float(-0.1f, 0.1f), Rando.Float(-0.1f, 0.1f)), 0.02f));
                Level.Add(SmallSmoke.New(pos.x + shrapDir.x * 8f + Rando.Float(-8f, 8f), pos.y + shrapDir.y * 8f + Rando.Float(-8f, 8f)));
            }
            if (Network.isActive && this.isLocal)
            {
                Send.Message(new NMFireGun(null, firedBullets, 0, false, 4, false), NetMessagePriority.ReliableOrdered);
                firedBullets.Clear();
            }
            if (Network.isActive && this.isLocal)
            {
                Rando.generator = rand;
            }
			foreach (Window w in Level.CheckCircleAll<Window>(pos, radius - 20f))
            {
                Thing.Fondle(w, DuckNetwork.localConnection);
                if (Level.CheckLine<Block>(pos, w.position, w) == null)
                {
                    w.Destroy(new DTImpact(this));
                }
            }
            foreach (PhysicsObject p in Level.CheckCircleAll<PhysicsObject>(pos, radius + 20f))
            {
                if (this.isLocal && this.owner == null)
                {
                    Thing.Fondle(p, DuckNetwork.localConnection);
                }
                if ((p.position - pos).length < 30f)
                {
                    p.Destroy(new DTImpact(this));
                }
                p.sleeping = false;
                p.vSpeed = -2f;
            }
			int idd = 0;
			HashSet<ushort> idx = new HashSet<ushort>();
			foreach (BlockGroup block in Level.CheckCircleAll<BlockGroup>(pos, radius))
			{
				if (block != null)
				{
                    BlockGroup group = block;
					new List<Block>();
					foreach (Block bl in group.blocks)
					{
						if (Collision.Circle(pos, radius - 22f, bl.rectangle))
						{
							bl.shouldWreck = true;
							if (bl is AutoBlock && !(bl as AutoBlock).indestructable)
							{
								idx.Add((bl as AutoBlock).blockIndex);
								if (false && idd % 10 == 0)
								{
                                    Level.Add(new ExplosionPart(bl.x, bl.y, true));
                                    Level.Add(SmallFire.New(bl.x, bl.y, Rando.Float(-2f, 2f), Rando.Float(-2f, 2f), false, null, true, null, false));
								}
								idd++;
							}
						}
					}
					group.Wreck();
				}
			}
			foreach (Block block2 in Level.CheckCircleAll<Block>(pos, radius - 22f))
			{
				if (block2 is AutoBlock && !(block2 as AutoBlock).indestructable)
				{
					block2.skipWreck = true;
					block2.shouldWreck = true;
					idx.Add((block2 as AutoBlock).blockIndex);
					//if (pExplode)
					//{
					if (idd % 10 == 0)
					{
                        Level.Add(new ExplosionPart(block2.x, block2.y, true));
                        Level.Add(SmallFire.New(block2.x, block2.y, Rando.Float(-2f, 2f), Rando.Float(-2f, 2f), false, null, true, null, false));
					}
					idd++;
					//}
				}
				else if (block2 is Door || block2 is VerticalDoor)
				{
                    Level.Remove(block2);
					block2.Destroy(new DTRocketExplosion(null));
				}
			}
			if (Network.isActive && this.isLocal && idx.Count > 0)
			{
                Send.Message(new NMDestroyBlocks(idx));
			}
			foreach (Thing thing in Level.current.things[typeof(ILight)])
			{
				((ILight)thing).Refresh();
			}
            Level.Remove(this);		
        }
    }
}
