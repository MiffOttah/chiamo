using MiffTheFox.Chiamo.Graphics;
using System;
using System.Globalization;

namespace MiffTheFox.Chiamo.Tiles
{
    public abstract class Tileset
    {
        public int TileWidth { get; private set; }
        public int TileHeight { get; private set; }
        public string TileSprites { get; private set; }

        protected TileRenderOverrideCollection Overrides { get; } = new TileRenderOverrideCollection();

        protected Tileset(int tileWidth, int tileHeight, string tileSprites)
        {
            TileWidth = tileWidth;
            TileHeight = tileHeight;
            TileSprites = tileSprites;
        }

        [Obsolete]
        protected virtual void DrawTitle(GameDrawArgs e, int canvasX, int canvasY, byte tileId)
        {
            _DefaultDraw(e, canvasX, canvasY, tileId);
        }

        private void _DefaultDraw(GameDrawArgs e, int canvasX, int canvasY, byte tileId)
        {
            var si = e.Game.Sprites[TileSprites];

            int maxPerRow = si.Width / TileWidth;

            int col = tileId % maxPerRow;
            int row = (tileId - col) / maxPerRow;

            e.Canvas.DrawSprite(si, canvasX, canvasY, TileWidth, TileHeight, row, col);
        }

        public virtual void DrawTile(TileRenderArgs e)
        {
            byte id = e.TileID;
            if (Overrides.Contains(id))
            {
                Overrides[id].DrawTile(e);
            }
            else
            {
                _DefaultDraw(e.GameDrawArgs, e.TileX * TileWidth, e.TileY * TileHeight, e.TileID);
            }
        }
        
        public abstract TileType GetTileType(byte tileId);
    }

    public class TileRenderArgs
    {
        public GameDrawArgs GameDrawArgs { get; set; }
        public TileMap TileMap { get; set; }

        public int TileX { get; set; }
        public int TileY { get; set; }

        public byte TileID => TileMap[TileX, TileY];
        public Game Game => GameDrawArgs.Game;
        public Canvas Canvas => GameDrawArgs.Canvas;
        public Tileset Tileset => TileMap.Tileset;
    }

    public enum TileType
    {
        Background = 0,
        Wall = 1,
        Ladder = 2
    }
}
