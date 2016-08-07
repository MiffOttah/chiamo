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

        private static CollisionType _CheckCollisionWithSceneEdge(Scene scene, Rectangle hb)
        {
            var ct = CollisionType.None;

            if (hb.Left < 0) ct |= CollisionType.Left;
            if (hb.Right >= scene.Width) ct |= CollisionType.Right;
            if (hb.Top < 0) ct |= CollisionType.Top;
            if (hb.Bottom >= scene.Height) ct |= CollisionType.Bottom;

            return ct;
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

        private CollisionType _CheckCollisionWithTilemap(Scene scene, Rectangle hb)
        {
            foreach (var tm in scene.TileMaps)
            {
                var tcol = CollisionType.None;

                // check the tiles around the edges of the actor
                for (int i = hb.Left; i <= hb.Right; i++)
                {
                    tcol |= _CheckCollisionWithTileAt(i, hb.Top, tm, CollisionType.Top);
                    tcol |= _CheckCollisionWithTileAt(i, hb.Bottom, tm, CollisionType.Bottom);
                }
                for (int i = hb.Top; i <= hb.Bottom; i++)
                {
                    tcol |= _CheckCollisionWithTileAt(hb.Left, i, tm, CollisionType.Top);
                    tcol |= _CheckCollisionWithTileAt(hb.Right, i, tm, CollisionType.Bottom);
                }

                if (tcol != CollisionType.None) return tcol;
            }

            return CollisionType.None;
        }

        private CollisionType _CheckCollisionWithTileAt(int x, int y, TileMap tm, CollisionType checking)
        {
            var t = tm.GetTileAtSceneCoords(x, y);
            return tm.Tileset.IsTilePassable(t) ? CollisionType.None : checking;
        }

        public CollisionType CollisionWithAnything(Scene scene)
        {
            var hb = this.HitBox;
            return _CheckCollisionWithSceneEdge(scene, hb) | _CheckCollisionWithOtherActor(scene, hb) | _CheckCollisionWithTilemap(scene, hb);
        }

        public CollisionType PossibleCollisionWithAnything(Scene scene, int movementX, int movementY)
        {
            var hb = this.HitBox;
            var altHitbox = new Rectangle(hb.X + movementX, hb.Y + movementY, hb.Width, hb.Height);

            return _CheckCollisionWithSceneEdge(scene, altHitbox) | _CheckCollisionWithOtherActor(scene, altHitbox) | _CheckCollisionWithTilemap(scene, altHitbox);
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
