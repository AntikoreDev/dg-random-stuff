using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    [EditorGroup("Random Stuff|Guns")]
    class DizzyShot : Gun
    {
        public DizzyShot(float xpos, float ypos) : base(xpos, ypos)
        {
            this.graphic = new Sprite(Mod.GetPath<RSMod>("dizzyShot"));
            this.ammo = 36;
            this._ammoType = new AT9mm();
            this._ammoType.range = 200f;
            this._ammoType.accuracy = 0.8f;
            this._ammoType.penetration = 1f;
            this.center = new Vec2(14f, 4f);
            this.collisionOffset = new Vec2(-14f, -4f);
            this.collisionSize = new Vec2(26f, 10f);
            this._barrelOffsetTL = new Vec2(25f, 3f);
            this._kickForce = 1.5f;
            this._fireSound = "deepMachineGun2";
            this._fireWait = 2f;
        }

        public override void Update()
        {
            if (this._bursting)
            {
                this._burstWait = Maths.CountDown(this._burstWait, 0.16f, 0f);
                if (this._burstWait <= 0f)
                {
                    this._burstWait = 1f;
                    if (base.isServerForObject)
                    {
                        this.Fire();
                        NMFireGun gunEvent = new NMFireGun(this, this.firedBullets, this.bulletFireIndex, false, Convert.ToByte((base.duck != null) ? base.duck.netProfileIndex : 4), true);
                        Send.Message(gunEvent, NetMessagePriority.Urgent, null as DuckGame.NetworkConnection);
                        this.firedBullets.Clear();
                    }
                    this._wait = 0f;
                    this._burstNum++;
                }
                if (this._burstNum == 3)
                {
                    this._burstNum = 0;
                    this._burstWait = 0f;
                    this._bursting = false;
                    this._wait = this._fireWait;
                }
            }
            base.Update();
        }

        public override void OnPressAction()
        {
            if (this.receivingPress && this.hasFireEvents && this.onlyFireAction)
            {
                this.Fire();
            }
            if (!this._bursting && this._wait == 0f)
            {
                this._bursting = true;
            }
        }

        public override void OnHoldAction()
        {
        }

        public StateBinding _burstingBinding = new StateBinding("_bursting", -1, false, false);
        public StateBinding _burstNumBinding = new StateBinding("_burstNum", -1, false, false);
        public float _burstWait;
        public bool _bursting;
        public int _burstNum;
    }
}
