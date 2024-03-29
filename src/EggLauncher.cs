﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    [EditorGroup("Random Stuff|Guns|Explosive")]
    class EggLauncher : Gun
    {
        public override float angle
        {
            get
            {
                return base.angle + this._aimAngle;
            }
            set
            {
                this._angle = value;
            }
        }

        public EggLauncher(float xval, float yval) : base(xval, yval)
        {
            this.ammo = 15;
            this._type = "gun";
            this.graphic = new Sprite(GetPath("eggLauncher"), 0f, 0f);
            this.center = new Vec2(16f, 16f);
            this.collisionOffset = new Vec2(-6f, -4f);
            this.collisionSize = new Vec2(16f, 7f);
            this._barrelOffsetTL = new Vec2(28f, 14f);
            this._kickForce = 3f;
            this._holdOffset = new Vec2(4f, 0f);
        }

        public override void Update()
        {
            base.Update();
            if (this._aiming && this._aimWait <= 0f && this._fireAngle < 90f)
            {
                this._fireAngle += 5f;
            }
            if (this._aimWait > 0f)
            {
                this._aimWait -= 0.9f;
            }
            if ((double)this._cooldown > 0.0)
            {
                this._cooldown -= 0.1f;
            }
            else
            {
                this._cooldown = 0f;
            }
            if (this.owner != null)
            {
                this._aimAngle = -Maths.DegToRad(this._fireAngle);
                if (this.offDir < 0)
                {
                    this._aimAngle = -this._aimAngle;
                }
            }
            else
            {
                this._aimWait = 0f;
                this._aiming = false;
                this._aimAngle = 0f;
                this._fireAngle = 0f;
            }
            if (this._raised)
            {
                this._aimAngle = 0f;
            }
        }

        public override void OnPressAction()
		{
			if (this._cooldown == 0f)
			{
				if (this.ammo > 0)
				{
					this._aiming = true;
					this._aimWait = 1f;
					return;
				}
				SFX.Play("click", 1f, 0f, 0f, false);
			}
		}

        public override void OnReleaseAction()
        {
            if (this._cooldown == 0f && this.ammo > 0)
            {
                this._aiming = false;
                this.Fire();
                this._cooldown = 1f;
                this.angle = 0f;
                this._fireAngle = 0f;
            }
        }

        public override void Fire()
        {
            if (this.ammo > 0)
            {
                this.ammo--;
                base.ApplyKick();
                Vec2 pos = this.Offset(base.barrelOffset);
                SFX.Play(Mod.GetPath<RSMod>("SFX\\eggPop"));
                if (Network.isActive)
                {
                    this._netShootSound.Play();
                }
                if (isServerForObject)
                {       
                    Egg e = new Egg(pos.x, pos.y - 2f);
                    e.vSpeed = this.barrelVector.y * 10f;
                    e.hSpeed = this.barrelVector.x * 10f;
                    Level.Add((Thing)e);
                }
            }
            else
            {
                base.DoAmmoClick();
            }
        }

        public StateBinding _netShootSoundBinding = new NetSoundBinding("_netShootSound");
        public NetSoundEffect _netShootSound = new NetSoundEffect(new string[]
        {
            Mod.GetPath<RSMod>("SFX\\eggPop")
        })
        {
            volume = 1f
        };
        public StateBinding _fireAngleState = new StateBinding("_fireAngle", -1, false, false);
        public StateBinding _aimAngleState = new StateBinding("_aimAngle", -1, false, false);
        public StateBinding _aimWaitState = new StateBinding("_aimWait", -1, false, false);
        public StateBinding _aimingState = new StateBinding("_aiming", -1, false, false);
        public StateBinding _cooldownState = new StateBinding("_cooldown", -1, false, false);
        public float _fireAngle;
        public float _aimAngle;
        public float _aimWait;
        public bool _aiming;
        public float _cooldown;
    }
}
