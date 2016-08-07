using MiffTheFox.Chiamo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformExample
{
    public class PxGame : Game
    {
        public PxGame() : base(640, 480, "Platformer Example")
        {
        }

        public override void Initalize()
        {
            Sprites.AddSprite("Player", Properties.Resources.player);
            Sprites.AddSprite("Coin", Properties.Resources.coin);
            Sprites.AddSprite("Enemy", Properties.Resources.enemy);
            Sprites.AddSprite("Tiles", Properties.Resources.tiles);
            Sprites.AddSprite("Title", Properties.Resources.title);

            PushScene(new TitleScreen());
        }
    }
}
