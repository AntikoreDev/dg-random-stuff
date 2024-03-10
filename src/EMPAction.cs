using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckGame;

namespace DuckGame.RSModDEV
{
    class EMPAction : Thing
    {
        public Gun _gun;
        public float dur;
        public float _timer;
        public EMPAction(float xval, float yval, Gun g) : base(xval, yval)
        {
            this._gun = g;
        }

        public override void Initialize()
        {
            if (this._gun != null)
            {
                if (Network.isActive)
                {
                    Rando.generator = new Random(NetRand.currentSeed);
                }
                if (this._gun is EMPGrenade)
                {
                    this.Action();
                }
                this.dur = Rando.Float(0.1f, 1.2f);
                this._timer = 0f;
            }       
            base.Initialize();
        }

        public override void Update()
        {
            this._timer += 0.1f;
            if (this._timer >= this.dur)
            {
                this.Action();
            }
            base.Update();
        }

        public void Action()
        {
            if (this._gun != null)
            {
                this._gun.OnPressAction();
                if (this._gun is Mine)
                {
                    (_gun as Mine)._armed = true;
                }
            }
            Level.Remove(this);
        }

        public StateBinding _gunBinding = new StateBinding("_gun", -1, false, false);
        public StateBinding _timerBinding = new StateBinding("_timer", -1, false, false);
        public StateBinding _durBinding = new StateBinding("dur", -1, false, false);
    }
}
