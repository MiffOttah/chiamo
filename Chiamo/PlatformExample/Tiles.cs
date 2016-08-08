﻿using MiffTheFox.Chiamo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformExample
{
    public enum Tile : byte
    {
        Sky = 0,
        Wall = 1,
        Pipe = 2,
        Ladder = 3
    }

    public class PxTileset : Tileset
    {
        public PxTileset() : base(40, 40, "Tiles")
        {
        }

        public override TileType GetTileType(byte tileId)
        {
            switch ((Tile)tileId)
            {
                case Tile.Sky:
                    return TileType.Background;

                case Tile.Ladder:
                    return TileType.Ladder;

                default:
                    return TileType.Wall;
            }
        }
    }
}
