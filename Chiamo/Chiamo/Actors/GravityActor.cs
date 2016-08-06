using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiffTheFox.Chiamo.Actors
{
    public abstract class GravityActor : MomentumCollisionActor
    {
        public int Gravity { get; set; } = 0;

        public GravityActor(int width, int height) : base(width, height)
        {
        }

        public override void Tick(GameTickArgs e, Scene s)
        {
            this.YMomentum += Gravity;

            base.Tick(e, s);
        }
    }
}
