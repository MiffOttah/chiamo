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
            
            foreach (var actor in scene.Actors.Where(_ => _ is CollisionActor && _.Guid != this.Guid).Cast<CollisionActor>())
            {
                var hb2 = actor.HitBox;
                var isect = Rectangle.Intersect(hb1, hb2);
                
                if (isect.Width > 0 || isect.Height > 0)
                {
                    if (isect.Height > isect.Width)
                    {
                        return hb1.X < hb2.X ? CollisionType.Left : CollisionType.Right;
                    }
                    else
                    {
                        return hb1.Y < hb2.Y ? CollisionType.Top : CollisionType.Bottom;
                    }
                }
            }
            
            return CollisionType.None;
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
