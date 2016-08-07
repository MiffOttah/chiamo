using MiffTheFox.Chiamo;
using MiffTheFox.Chiamo.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformExample
{
    public class Enemy : GravityActor
    {
        public Enemy() : base(30, 34)
        {
        }

        public override void Draw(GameDrawArgs e)
        {
            DrawSprite(e, "Enemy");
        }


        public override void OnCollision(GameTickArgs e, Scene s, CollisionInfo collision)
        {
            if (collision.HasFlag(CollisionWith.Actor) && collision.OtherActor is Player)
            {
                // kill the player
                e.Game.PopScene();
                e.Game.PushScene(new TitleScreen());
            }
            else if (collision.HasFlag(CollisionEdge.Left) || collision.HasFlag(CollisionEdge.Right))
            {
                this.XMomentum *= -1;
            }
        }
    }
}
