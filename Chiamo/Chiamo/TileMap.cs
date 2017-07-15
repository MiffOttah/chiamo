using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace MiffTheFox.Chiamo
{
    public class TileMap
    {
        private byte[,] _TilemapData;

        public Tileset Tileset { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int ZIndex { get; set; } = 0;

        public TileMap(Tileset tileset, int width, int height)
        {
            Tileset = tileset;
            Width = width;
            Height = height;

            _TilemapData = new byte[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    _TilemapData[i, j] = 0;
                }
            }
        }

        public byte this[int x, int y]
        {
            get
            {
                if (x < 0 || y < 0 || x >= Width || y >= Height) return 0;
                return _TilemapData[x, y];
            }
            set
            {
                if (x < 0 || y < 0 || x >= Width || y >= Height) return;
                _TilemapData[x, y] = value;
            }
        }

        public byte GetTileAtSceneCoords(int x, int y)
        {
            int tx = x / Tileset.TileWidth;
            int ty = y / Tileset.TileHeight;
            return this[tx, ty];
        }

        public byte[] GetTilesAtSceneRect(Rectangle rect)
        {
            var tiles = new HashSet<byte>();
            var skipX = this.Tileset.TileWidth / 2;
            var skipY = this.Tileset.TileHeight / 2;

            for (int x = rect.Left; x <= rect.Right; x += skipX)
            {
                for (int y = rect.Top; y <= rect.Bottom; y += skipY)
                {
                    tiles.Add(GetTileAtSceneCoords(x, y));
                }
            }

            return tiles.ToArray();
        }

        public void SetTilesAtSceneCoords(int x, int y, byte setTo)
        {
            int tx = x / Tileset.TileWidth;
            int ty = y / Tileset.TileHeight;
            this[tx, ty] = setTo;
        }

        public void SetTilesAtSceneRect(Rectangle rect, byte setTo)
        {
            int startX = rect.Left / Tileset.TileWidth;
            int endX = rect.Right / Tileset.TileWidth;
            int startY = rect.Top / Tileset.TileHeight;
            int endY = rect.Bottom / Tileset.TileHeight;

            for (int i = startX; i <= endX; i++)
            {
                for (int j = startY; j <= endY; j++)
                {
                    this[i, j] = setTo;
                }
            }
        }
    }
    
}
