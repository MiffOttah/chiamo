using System;

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
                return _TilemapData[x, y];
            }
            set
            {
                _TilemapData[x, y] = value;
            }
        }

        public byte GetTileAtSceneCoords(int x, int y)
        {
            int tx = x / Tileset.TileWidth;
            int ty = y / Tileset.TileHeight;
            return this[tx, ty];
        }
    }
    
}
