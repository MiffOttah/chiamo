using MiffTheFox.Chiamo;
using MiffTheFox.Chiamo.Actors;
using System.Drawing;

namespace ChHelloWorld
{
    public class Ball : MomentumCollisionActor, IClickableActor
    {
        public Ball() : base(40, 40)
        {
        }

        public override Rectangle HitBox
        {
            get
            {
                return new Rectangle(X + 5, Y + 5, 30, 30);
            }
        }

        public void Clicked(GameTickArgs e, Scene s)
        {
            if (e.Input.MouseButton == MouseButton.Right)
            {
                s.Actors.Remove(this);
            }
        }

        public override void Draw(GameDrawArgs e)
        {
            this.DrawSprite(e, "ball");
        }

        public override void OnCollision(Scene s, CollisionInfo collision)
        {
            if (collision.HasFlag(CollisionEdge.Left) || collision.HasFlag(CollisionEdge.Right))
            {
                this.XMomentum *= -1;
            }

            if (collision.HasFlag(CollisionEdge.Top) || collision.HasFlag(CollisionEdge.Bottom))
            {
                this.YMomentum *= -1;
            }
        }
    }
}
