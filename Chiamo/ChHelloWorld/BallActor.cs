using MiffTheFox.Chiamo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChHelloWorld
{
    public class BallActor : Actor
    {
        public BallActor() : base(40, 40)
        {
        }

        public override void Draw(GameDrawArgs e)
        {
            this.DrawSprite(e, "ball");
        }

        public override void Tick(GameTickArgs e)
        {
        }
    }
}
