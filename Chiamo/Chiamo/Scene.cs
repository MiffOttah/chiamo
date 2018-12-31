using MiffTheFox.Chiamo.Graphics;
using MiffTheFox.Chiamo.Tiles;
using System;
using System.Drawing;
using System.Linq;

namespace MiffTheFox.Chiamo
{
    public abstract class Scene
    {
        public virtual bool IsTransparent { get; set; } = false;
        public virtual Game Game { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        private readonly Camera _Camera = new Camera();
        public Camera Camera => _Camera;

        public ActorCollection Actors { get; private set; } = new ActorCollection();
        public TileMapCollection TileMaps { get; private set; } = new TileMapCollection();

        public event EventHandler Popped;

        public abstract void Initalize();

        public virtual void Tick(GameTickArgs e)
        {
            foreach (var actor in Actors.ToArray())
            {
                actor.Tick(e, this);
            }
        }

        public virtual void Draw(GameDrawArgs e)
        {
            var viewPort = _Camera.RecalcuateViewPort(this);

            var oc = new OffsetCanvas(e.Canvas, -viewPort.X, -viewPort.Y);
            var oa = new GameDrawArgs(oc) { Game = e.Game };

            foreach (var tm in TileMaps.OrderBy(t => t.ZIndex))
            {
                DrawTileMap(viewPort, oa, tm);
            }

            foreach (var actor in Actors.OrderBy(_ => _.ZIndex).ToArray())
            {
                if (actor.Bounds.IntersectsWith(viewPort))
                {
                    actor.Draw(oa);
                }
            }
        }

        private static void DrawTileMap(Rectangle viewPort, GameDrawArgs oa, TileMap tm)
        {
            Rectangle tileDrawRegion = new Rectangle(
                viewPort.X - tm.Tileset.TileWidth,
                viewPort.Y - tm.Tileset.TileHeight,
                viewPort.Width + tm.Tileset.TileWidth * 2,
                viewPort.Height + tm.Tileset.TileHeight * 2
            );

            int i = Math.Max(0, tileDrawRegion.X / tm.Tileset.TileWidth);
            int jstart = Math.Max(0, tileDrawRegion.Y / tm.Tileset.TileHeight);
            int iend = Math.Min(tileDrawRegion.Right / tm.Tileset.TileWidth + 1, tm.Width);
            int jend = Math.Min(tileDrawRegion.Bottom / tm.Tileset.TileHeight + 1, tm.Height);

            for (; i < iend; i++)
            {
                for (int j = jstart; j < jend; j++)
                {
                    int canvasX = i * tm.Tileset.TileWidth;
                    int canvasY = j * tm.Tileset.TileHeight;
                    tm.Tileset.DrawTitle(oa, canvasX, canvasY, tm[i, j]);
                }
            }
        }

        public virtual void OnPopped()
        {
            Popped?.Invoke(this, new EventArgs());
        }
    }
}