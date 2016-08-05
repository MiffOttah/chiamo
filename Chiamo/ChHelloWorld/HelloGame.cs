using MiffTheFox.Chiamo;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChHelloWorld
{
    public class HelloGame : Game
    {
        public HelloGame() : base(640, 480, "Hello world")
        {
        }

        public override void Initalize()
        {
            // Load sprites
            Sprites.AddSprite("ball", Properties.Resources.ball);

            // Push initial scene
            PushScene(new HelloScene());
        }
    }
}
