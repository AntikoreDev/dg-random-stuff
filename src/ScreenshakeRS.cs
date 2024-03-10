using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
	public class ScreenshakeRS : Thing
	{
		public ScreenshakeRS(float l, float m) : base(0f, 0f, null)
		{
			this.length = l;
			this.magnitude = m;
		}
		public override void Update()
		{
			bool flag = this.length > 0f;
			if (flag)
			{
				Level.current.camera.position -= this.lastShake;
				this.lastShake = new Vec2(Rando.Float(-this.magnitude, this.magnitude), Rando.Float(-this.magnitude, this.magnitude));
				Level.current.camera.position += this.lastShake;
				this.length -= 1f;
				bool flag2 = this.length == 0f;
				if (flag2)
				{
					Level.current.camera.position -= this.lastShake;
					this.lastShake = Vec2.Zero;
				}
			}
			base.Update();
		}

		public float length;
		public float magnitude;
		public Vec2 lastShake;
	}
}
