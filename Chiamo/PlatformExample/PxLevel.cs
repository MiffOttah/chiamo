using MiffTheFox.Chiamo;
using MiffTheFox.Chiamo.Tiles;
using MiffTheFox.Chiamo.TMX;
using System.Drawing;

namespace PlatformExample
{
    public class PxLevel : Scene
    {
        const int TILESIZE = 40;
        const int TILE_W = 40;
        const int TILE_H = 20;

        const int GRAVITY = 2;

        public int Score { get; set; } = 0;

        public override void Initalize()
        {
            this.Width = TILESIZE * TILE_W;
            this.Height = TILESIZE * TILE_H;

            var player = new Player
            {
                X = 40,
                Y = 40,
                CameraFollows = true,
                Gravity = GRAVITY
            };
            this.Actors.Add(player);

            var tilemap = new TileMap(new PxTileset(), TILE_W, TILE_H);

            var tmx = new TmxParser(Properties.Resources.level1);
            tmx.ParseTileLayer("Tile Layer 1", tilemap);
            tmx.ParseObjectLayer("Object Layer 1", o =>
            {
                switch (o.Name)
                {
                    case "Player":
                        player.X = o.X;
                        player.Y = o.Y;
                        break;

                    case "Enemy":
                        Actors.Add(new Enemy { X = o.X, Y = o.Y, XMomentum = 10, Gravity = GRAVITY });
                        break;

                    case "Coin":
                        Actors.Add(new Coin { X = o.X, Y = o.Y });
                        break;

                    case "EndCastle":
                        Actors.Add(new EndCastle { X = o.X, Y = o.Y });
                        break;
                }
            });

            TileMaps.Add(tilemap);

            Game.Music?.PlaySong("Theme");
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

        public override void Draw(GameDrawArgs e)
        {
            base.Draw(e);

            e.Canvas.DrawString(Game.Fonts["sans-serif"], $"Score: {Score}", Color.Black, 22, 0, 0, 300, 100, false, false, StringAlignment.Near, StringAlignment.Near);
        }
    }
}
