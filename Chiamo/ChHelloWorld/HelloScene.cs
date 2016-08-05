using MiffTheFox.Chiamo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChHelloWorld
{
    public class HelloScene : Scene
    {
        public override void Initalize()
        {
            Actors.Add(new BallActor() { X = 10, Y = 10 });
        }
    }
}
