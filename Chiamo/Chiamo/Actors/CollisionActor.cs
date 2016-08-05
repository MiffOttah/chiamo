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

            if (hb.Left < 0 || hb.Right >= game.Width) ct |= CollisionType.X;
            if (hb.Top < 0 || hb.Bottom >= game.Height) ct |= CollisionType.Y;

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
                        return CollisionType.X;
                    }
                    else if (isect.Width > isect.Height)
                    {
                        return CollisionType.Y;
                    } else
                    {
                        return CollisionType.None;
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
        X = 1,
        Y = 2,
        XY = 3
    }
}
