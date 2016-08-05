using MiffTheFox.Chiamo;
using MiffTheFox.Chiamo.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ChHelloWorld
{
    public class BallActor : MomentumCollisionActor
    {
        public BallActor() : base(40, 40)
        {
        }

        public override Rectangle HitBox
        {
            get
            {
                return new Rectangle(X + 5, Y + 5, 30, 30);
            }
        }

        public override void Draw(GameDrawArgs e)
        {
            this.DrawSprite(e, "ball");
        }

        public override void OnCollision(GameTickArgs e, Scene s, CollisionType collision)
        {
            if (collision.HasFlag(CollisionType.X))
            {
                this.XMomentum *= -1;
            }

            if (collision.HasFlag(CollisionType.Y))
            {
                this.YMomentum *= -1;
            }
        }
    }
}
