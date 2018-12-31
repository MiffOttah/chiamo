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
        public HelloSaveData SaveData { get; private set; }

        public HelloGame() : base(640, 480, "Hello world")
        {
            this.EnableMouseCursor = true;
            SaveData = new HelloSaveData();
        }

        public override void Initalize()
        {
            // Load sprites
            Sprites.AddSprite("ball", Properties.Resources.ball);
            Sprites.AddSprite("ball2", Properties.Resources.ball2);
            Sprites.AddSprite("testplayer", Properties.Resources.testplayer);

            // Load fonts
            Fonts.AddFont("ArchitectsDaughter", Properties.Resources.ArchitectsDaughter);

            // Push initial scene
            PushScene(new HelloScene());
        }
    }
}
