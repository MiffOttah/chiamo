using MiffTheFox.Chiamo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformExample
{
    public class PxLevel : Scene
    {
        const int TILESIZE = 40;
        const int TILE_W = 40;
        const int TILE_H = 20;

        const int GRAVITY = 2;

        public override void Initalize()
        {
            this.Width = TILESIZE * TILE_W;
            this.Height = TILESIZE * TILE_H;

            var player = new Player();
            player.X = 40;
            player.Y = 40;
            player.CameraFollows = true;
            player.Gravity = GRAVITY;
            this.Actors.Add(player);

            var tilemap = new TileMap(new PxTileset(), TILE_W, TILE_H);

            // fill the bottom of the map with bricks
            for (int j = TILE_H - 3; j < TILE_H; j++)
            {
                for (int i = 0; i < TILE_W; i++)
                {
                    tilemap[i, j] = 1;
                }
            }

            // create a "stair" shape
            for (int x = 6; x < 16; x++)
            {
                for (int y = TILE_H - 3; y > TILE_H - (x - 3); y--)
                {
                    tilemap[x, y] = 1;
                }
            }

            // now put pipes and an enemy
            tilemap[24, TILE_H - 4] = 2;
            tilemap[34, TILE_H - 4] = 2;
            var enemy = new Enemy() { X = 1090, Y = 600, Gravity = GRAVITY, XMomentum = 5 };
            this.Actors.Add(enemy);

            TileMaps.Add(tilemap);
        }

        public override void Tick(GameTickArgs e)
        {
            if (e.Input.JoyButton == JoyButton.Menu)
            {
                Game.PopScene();
                Game.PushScene(new TitleScreen());
            }

            base.Tick(e);
        }
    }
}
