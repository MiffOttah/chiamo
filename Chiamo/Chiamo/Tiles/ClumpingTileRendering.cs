using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiffTheFox.Chiamo.Tiles
{
    public class ClumpingTileRendering : ITileRenderOverride
    {
        private static readonly ClumpNeighboring[] _ClumpMapping = new ClumpNeighboring[]
        {
            ClumpNeighboring.Right | ClumpNeighboring.Bottom,
            ClumpNeighboring.Left | ClumpNeighboring.Right | ClumpNeighboring.Bottom,
            ClumpNeighboring.Left | ClumpNeighboring.Bottom,
            ClumpNeighboring.Bottom,

            ClumpNeighboring.Top | ClumpNeighboring.Right | ClumpNeighboring.Bottom,
            ClumpNeighboring.Top | ClumpNeighboring.Right | ClumpNeighboring.Bottom | ClumpNeighboring.Left,
            ClumpNeighboring.Top | ClumpNeighboring.Bottom | ClumpNeighboring.Left,
            ClumpNeighboring.Top | ClumpNeighboring.Bottom,

            ClumpNeighboring.Right | ClumpNeighboring.Top,
            ClumpNeighboring.Left | ClumpNeighboring.Right | ClumpNeighboring.Top,
            ClumpNeighboring.Left | ClumpNeighboring.Top,
            ClumpNeighboring.Top,

            ClumpNeighboring.Right,
            ClumpNeighboring.Left | ClumpNeighboring.Right,
            ClumpNeighboring.Left,
            ClumpNeighboring.None
        };

        public byte TileId { get; }
        public string ClumpTexture { get; }

        public ClumpingTileRendering(byte tileId, string clumpTextrure)
        {
            TileId = tileId;
            ClumpTexture = clumpTextrure;
        }

        public void DrawTile(TileRenderArgs e)
        {
            var neighboring = ClumpNeighboring.None;

            if (e.TileMap[e.TileX - 1, e.TileY] == TileId) neighboring |= ClumpNeighboring.Left;
            if (e.TileMap[e.TileX + 1, e.TileY] == TileId) neighboring |= ClumpNeighboring.Right;
            if (e.TileMap[e.TileX, e.TileY - 1] == TileId) neighboring |= ClumpNeighboring.Top;
            if (e.TileMap[e.TileX, e.TileY + 1] == TileId) neighboring |= ClumpNeighboring.Bottom;

            int tileOffset = 0;
            while (tileOffset < 15 && _ClumpMapping[tileOffset] != neighboring) tileOffset++;

            var si = e.Game.Sprites[ClumpTexture];
            int col = tileOffset % 4;
            int row = (tileOffset - col) / 4;

            e.Canvas.DrawSprite(
                si,
                e.TileX * e.Tileset.TileWidth,
                e.TileY * e.Tileset.TileHeight,
                e.Tileset.TileWidth,
                e.Tileset.TileHeight,
                row,
                col
            );
        }
    }

    [Flags]
    public enum ClumpNeighboring : byte
    {
        None = 0,
        Left = 1,
        Right = 2,
        Bottom = 4,
        Top = 8
    }
}
