using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiffTheFox.Chiamo.Actors
{
    /// <summary>
    /// Repersents an actor that will remove itself when a PlayerActor collides into it.
    /// </summary>
    public abstract class PickUpActor : CollisionActor
    {
        protected PickUpActor(int width, int height) : base(width, height)
        {
        }

        public override void OnHitByMovingActor(Scene s, CollisionInfo otherActorCollisionInfo, Actor otherActor)
        {
            base.OnHitByMovingActor(s, otherActorCollisionInfo, otherActor);

            if (otherActor is PlayerActor)
            {
                s.Actors.Remove(this.Guid);
                OnPickUp(s, otherActor);
            }
        }

        public abstract void OnPickUp(Scene s, Actor player);
    }
}
