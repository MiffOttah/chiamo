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

            for (int j = TILE_H - 3; j < TILE_H; j++)
            {
                for (int i = 0; i < TILE_W; i++)
                {
                    tilemap[i, j] = 1;
                }
            }

            for (int x = 6; x < 16; x++)
            {
                for (int y = TILE_H - 3; y > TILE_H - (x - 3); y--)
                {
                    tilemap[x, y] = 1;
                }
            }

            TileMaps.Add(tilemap);
        }
    }
}
