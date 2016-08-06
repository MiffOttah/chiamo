using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiffTheFox.Chiamo.Actors
{
    public abstract class CollisionActor : Actor
    {
        public virtual Rectangle HitBox { get { return this.Bounds; } }

        protected CollisionActor(int width, int height) : base(width, height)
        {
        }

        public CollisionType CollisionWithGameEdge(Game game)
        {
            var hb = this.HitBox;
            return _CheckCollisionWithGameEdge(game, hb);
        }

        private static CollisionType _CheckCollisionWithGameEdge(Game game, Rectangle hb)
        {
            var ct = CollisionType.None;

            if (hb.Left < 0) ct |= CollisionType.Left;
            if (hb.Right >= game.Width) ct |= CollisionType.Right;
            if (hb.Top < 0) ct |= CollisionType.Top;
            if (hb.Bottom >= game.Height) ct |= CollisionType.Bottom;

            return ct;
        }

        public CollisionType CollisionWithOtherActor(Scene scene)
        {
            var hb1 = this.HitBox;
            return _CheckCollisionWithOtherActor(scene, hb1);
        }

        private CollisionType _CheckCollisionWithOtherActor(Scene scene, Rectangle hb1)
        {
            foreach (var actor in scene.Actors.Where(_ => _ is CollisionActor && _.Guid != this.Guid).Cast<CollisionActor>())
            {
                var hb2 = actor.HitBox;
                var isect = Rectangle.Intersect(hb1, hb2);

                if (isect.Width > 0 || isect.Height > 0)
                {
                    if (isect.Height > isect.Width)
                    {
                        return hb1.X < hb2.X ? CollisionType.Right : CollisionType.Left;
                    }
                    else
                    {
                        return hb1.Y < hb2.Y ? CollisionType.Bottom : CollisionType.Top;
                    }
                }
            }

            return CollisionType.None;
        }

        public CollisionType CollisionWithAnything(Scene scene)
        {
            return this.CollisionWithGameEdge(scene.Game) | this.CollisionWithOtherActor(scene);
        }

        public CollisionType PossibleCollisionWithAnything(Scene scene, int movementX, int movementY)
        {
            var hb = this.HitBox;
            var altHitbox = new Rectangle(hb.X + movementX, hb.Y + movementY, hb.Width, hb.Height);

            return _CheckCollisionWithGameEdge(scene.Game, altHitbox) | _CheckCollisionWithOtherActor(scene, altHitbox);
        }

        public CollisionType TryMove(Scene s, int movementX, int movementY)
        {
            int xmsign = Math.Sign(movementX);
            int ymsign = Math.Sign(movementY);

            var collision1 = this.PossibleCollisionWithAnything(s, movementX, 0);
            if (collision1 == CollisionType.None)
            {
                this.X += movementX;
            }
            else
            {
                // move as far as we can without generating a collision
                while (true)
                {
                    collision1 = this.PossibleCollisionWithAnything(s, xmsign, 0);
                    if (collision1 != CollisionType.None) break;
                    this.X += xmsign;
                }
            }

            var collision2 = this.PossibleCollisionWithAnything(s, 0, movementY);
            if (collision2 == CollisionType.None)
            {
                this.Y += movementY;
            }
            else
            {
                // move as far as we can without generating a collision
                while (true)
                {
                    collision2 = this.PossibleCollisionWithAnything(s, 0, ymsign);
                    if (collision2 != CollisionType.None) break;
                    this.Y += ymsign;
                }
            }

            return collision1 | collision2;
        }
    }

    [Flags]
    public enum CollisionType
    {
        None = 0,
        Top = 1,
        Left = 2,
        Bottom = 4,
        Right = 8
    }
}
