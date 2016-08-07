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
            var col = TryMove(s, this.XMomentum, this.YMomentum);
            if (col.HasCollision)
            {
                OnCollision(e, s, col);
            }

            base.Tick(e, s);
        }

        public virtual void OnCollision(GameTickArgs e, Scene s, CollisionInfo collision)
        {
            if (collision.HasFlag(CollisionEdge.Left) || collision.HasFlag(CollisionEdge.Right)) XMomentum = 0;
            if (collision.HasFlag(CollisionEdge.Top) || collision.HasFlag(CollisionEdge.Bottom)) YMomentum = 0;
        }
    }
}
