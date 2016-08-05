using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiffTheFox.Chiamo.Actors
{
    public abstract class MomentumCollisionActor : CollisionActor
    {
        public int XMomentum { get; set; }
        public int YMomentum { get; set; }

        protected MomentumCollisionActor(int width, int height) : base(width, height)
        {
            XMomentum = 0;
            YMomentum = 0;
        }

        public override void Tick(GameTickArgs e, Scene s)
        {
            this.X += XMomentum;
            this.Y += YMomentum;

            int xmSign = Math.Sign(XMomentum);
            int ymSign = Math.Sign(XMomentum);
            //CollisionType hasCol = CollisionType.None;
            //int overload = 0;
            
            var collision = this.CollisionWithGameEdge(e.Game) | this.CollisionWithOtherActor(s);

            if (collision != CollisionType.None)
            {
                OnCollision(e, s, collision);
            }

            base.Tick(e, s);
        }

        public virtual void OnCollision(GameTickArgs e, Scene s, CollisionType collision)
        {
        }
    }
}
