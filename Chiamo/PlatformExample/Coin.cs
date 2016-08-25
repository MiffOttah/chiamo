using MiffTheFox.Chiamo;
using MiffTheFox.Chiamo.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformExample
{
    public class Coin : PickUpActor
    {
        public Coin() : base(32, 32)
        {
        }

        public override void Draw(GameDrawArgs e)
        {
            int frame = Convert.ToInt32((e.Game.GameTime >> 1) & 3);
            DrawSprite(e, "Coin", 0, frame);
        }

        public override void OnPickUp(Scene s, Actor player)
        {
            if (s is PxLevel)
            {
                ((PxLevel)s).Score += 15;
            }

            s.Game.Sounds?.PlaySound("Coin");
        }
    }
}
