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
            Sprites.AddSprite("EndGame", Properties.Resources.endgame);
            Sprites.AddSprite("EndCastle", Properties.Resources.endcastle);

            Music?.AddSong("Theme", Properties.Resources.chiamo_platformexample_theme);

            Sounds?.AddSound("Jump", Properties.Resources.jump02);
            Sounds?.AddSound("Coin", Properties.Resources.cash_register_purchase);

            PushScene(new TitleScreen());
        }
    }
}
