using System;
using System.Globalization;

namespace MiffTheFox.Chiamo
{
    public abstract class Tileset
    {
        public int TileWidth { get; private set; }
        public int TileHeight { get; private set; }
        public string TileSprites { get; private set; }
        
        protected Tileset(int tileWidth, int tileHeight, string tileSprites)
        {
            TileWidth = tileWidth;
            TileHeight = tileHeight;
            TileSprites = tileSprites;
        }

        public void DrawTitle(GameDrawArgs e, int canvasX, int canvasY, byte tileId)
        {
            var si = e.Game.Sprites[TileSprites];

            int maxPerRow = si.Width / TileWidth;

            int col = tileId % maxPerRow;
            int row = (tileId - col) / maxPerRow;

            e.Canvas.DrawSprite(si, canvasX, canvasY, TileWidth, TileHeight, row, col);
        }

        public abstract bool IsTilePassable(byte tileId);
    }
}
