﻿using MiffTheFox.Chiamo;
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

        public override void OnCollision(GameTickArgs e, Scene s, CollisionType collision)
        {
            if (collision.HasFlag(CollisionType.Left) || collision.HasFlag(CollisionType.Right))
            {
                this.XMomentum *= -1;
            }

            if (collision.HasFlag(CollisionType.Top))
            {
                this.YMomentum *= -1;
            }
            else if (collision.HasFlag(CollisionType.Bottom))
            {
                this.YMomentum = 0;
                while (this.Y > 0 && this.CollisionWithAnything(s).HasFlag(CollisionType.Bottom))
                {
                    this.Y--;
                }
            }
        }
    }
}