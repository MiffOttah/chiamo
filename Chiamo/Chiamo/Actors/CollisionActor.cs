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

        private static CollisionInfo _CheckCollisionWithSceneEdge(Scene scene, Rectangle hb)
        {
            var ct = CollisionEdge.None;

            if (hb.Left < 0) ct |= CollisionEdge.Left;
            if (hb.Right >= scene.Width) ct |= CollisionEdge.Right;
            if (hb.Top < 0) ct |= CollisionEdge.Top;
            if (hb.Bottom >= scene.Height) ct |= CollisionEdge.Bottom;

            return new CollisionInfo(ct, (ct == CollisionEdge.None) ? CollisionWith.None : CollisionWith.SceneEdge);
        }

        private CollisionInfo _CheckCollisionWithOtherActor(Scene scene, Rectangle hb1)
        {
            foreach (var actor in scene.Actors.Where(_ => _ is CollisionActor && _.Guid != this.Guid).Cast<CollisionActor>())
            {
                var hb2 = actor.HitBox;
                var isect = Rectangle.Intersect(hb1, hb2);

                if (isect.Width > 0 || isect.Height > 0)
                {
                    CollisionEdge ce;
                    if (isect.Height > isect.Width)
                    {
                        ce = hb1.X < hb2.X ? CollisionEdge.Right : CollisionEdge.Left;
                    }
                    else
                    {
                        ce = hb1.Y < hb2.Y ? CollisionEdge.Bottom : CollisionEdge.Top;
                    }
                    return new CollisionInfo(ce, CollisionWith.Actor) { OtherActor = actor };
                }
            }

            return CollisionInfo.None;
        }

        private CollisionInfo _CheckCollisionWithTilemap(Scene scene, Rectangle hb)
        {
            foreach (var tm in scene.TileMaps)
            {
                var tcol = CollisionEdge.None;

                // check the tiles around the edges of the actor
                for (int i = hb.Left + 1; i <= hb.Right - 2; i++)
                {
                    tcol |= _CheckCollisionWithTileAt(i, hb.Top, tm, CollisionEdge.Top);
                    tcol |= _CheckCollisionWithTileAt(i, hb.Bottom, tm, CollisionEdge.Bottom);
                }
                for (int i = hb.Top + 1; i <= hb.Bottom - 2; i++)
                {
                    tcol |= _CheckCollisionWithTileAt(hb.Left, i, tm, CollisionEdge.Left);
                    tcol |= _CheckCollisionWithTileAt(hb.Right, i, tm, CollisionEdge.Right);
                }

                if (tcol != CollisionEdge.None) return new CollisionInfo(tcol, CollisionWith.Tile);
            }

            return CollisionInfo.None;
        }

        private CollisionEdge _CheckCollisionWithTileAt(int x, int y, TileMap tm, CollisionEdge checking)
        {
            var t = tm.GetTileAtSceneCoords(x, y);
            return tm.Tileset.IsTilePassable(t) ? CollisionEdge.None : checking;
        }

        private CollisionInfo _CollisionWithAnything(Scene scene, Rectangle hb)
        {
            return _CheckCollisionWithSceneEdge(scene, hb) | _CheckCollisionWithOtherActor(scene, hb) | _CheckCollisionWithTilemap(scene, hb);
        }

        public CollisionInfo CollisionWithAnything(Scene scene)
        {
            var hb = this.HitBox;
            return _CollisionWithAnything(scene, hb);
        }

        public CollisionInfo PossibleCollisionWithAnything(Scene scene, int movementX, int movementY)
        {
            var hb = this.HitBox;
            var altHitbox = new Rectangle(hb.X + movementX, hb.Y + movementY, hb.Width, hb.Height);
            return _CollisionWithAnything(scene, altHitbox);
        }

        public CollisionInfo TryMove(Scene s, int movementX, int movementY)
        {
            int xmsign = Math.Sign(movementX);
            int ymsign = Math.Sign(movementY);

            var collision1 = this.PossibleCollisionWithAnything(s, movementX, 0);
            if (!collision1.HasCollision)
            {
                this.X += movementX;
            }
            else
            {
                // move as far as we can without generating a collision
                while (true)
                {
                    collision1 = this.PossibleCollisionWithAnything(s, xmsign, 0);
                    if (collision1.HasCollision) break;
                    this.X += xmsign;
                }
            }

            var collision2 = this.PossibleCollisionWithAnything(s, 0, movementY);
            if (!collision2.HasCollision)
            {
                this.Y += movementY;
            }
            else
            {
                // move as far as we can without generating a collision
                while (true)
                {
                    collision2 = this.PossibleCollisionWithAnything(s, 0, ymsign);
                    if (collision2.HasCollision) break;
                    this.Y += ymsign;
                }
            }

            var col = collision1 | collision2;

            if (col.HasCollision)
            {
                OnCollision(s, col);
                if (col.HasFlag(CollisionWith.Actor) && col.OtherActor is CollisionActor)
                {
                    ((CollisionActor)col.OtherActor).OnHitByMovingActor(s, col, this);
                }
            }

            return col;
        }

        public virtual void OnCollision(Scene s, CollisionInfo collision)
        {
        }

        public virtual void OnHitByMovingActor(Scene s, CollisionInfo otherActorCollisionInfo, Actor otherActor)
        {
        }
    }

    [Flags]
    public enum CollisionEdge
    {
        None = 0,
        Top = 1,
        Left = 2,
        Bottom = 4,
        Right = 8
    }

    [Flags]
    public enum CollisionWith
    {
        None = 0,
        SceneEdge = 1,
        Actor = 2,
        Tile = 4
    }

    public class CollisionInfo
    {
        public CollisionEdge Edge { get; set; }
        public CollisionWith With { get; set; }
        public Actor OtherActor { get; set; }

        public bool HasCollision {  get { return Edge != CollisionEdge.None; } }

        public static CollisionInfo None { get { return new CollisionInfo(CollisionEdge.None, CollisionWith.None); } }

        public CollisionInfo(CollisionEdge edge, CollisionWith with)
        {
            Edge = edge;
            With = with;
            OtherActor = null;
        }

        public static explicit operator bool(CollisionInfo ci)
        {
            return ci.HasCollision;
        }

        public static CollisionInfo operator |(CollisionInfo a, CollisionInfo b)
        {
            var c = new CollisionInfo(a.Edge | b.Edge, a.With | b.With);
            if (a.OtherActor != null) c.OtherActor = a.OtherActor;
            else if (b.OtherActor != null) c.OtherActor = b.OtherActor;
            return c;
        }

        public bool HasFlag(CollisionEdge edge)
        {
            return Edge.HasFlag(edge);
        }
        public bool HasFlag(CollisionWith with)
        {
            return With.HasFlag(with);
        }

        public override string ToString()
        {
            if (Edge == CollisionEdge.None || With == CollisionWith.None)
            {
                return "Collision: None";
            }
            else
            {
                return $"Collision: {With} / {Edge}";
            }
        }
    }
}
