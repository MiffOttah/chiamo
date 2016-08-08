using MiffTheFox.Chiamo;
using MiffTheFox.Chiamo.Actors;
using System.Drawing;

namespace ChHelloWorld
{
    public class GravityBall : GravityActor, IClickableActor
    {
        public GravityBall() : base(40, 40)
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
            this.DrawSprite(e, "ball2");
        }

        public override void OnCollision(Scene s, CollisionInfo collision)
        {
            if (collision.HasFlag(CollisionEdge.Left) || collision.HasFlag(CollisionEdge.Right))
            {
                this.XMomentum *= -1;
            }

            if (collision.HasFlag(CollisionEdge.Top))
            {
                this.YMomentum *= -1;
            }
            else if (collision.HasFlag(CollisionEdge.Bottom))
            {
                this.YMomentum = 0;
                while (this.Y > 0 && this.CollisionWithAnything(s).HasFlag(CollisionEdge.Bottom))
                {
                    this.Y--;
                }
            }
        }
    }
}
