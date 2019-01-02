using MiffTheFox.Chiamo.Tiles;

namespace PlatformExample
{
    public enum Tile : byte
    {
        Sky = 0,
        Wall = 1,
        Pipe = 2,
        Ladder = 3,
        ClumpWall = 4
    }

    public class PxTileset : Tileset
    {
        public PxTileset() : base(40, 40, "Tiles")
        {
            Overrides.Add(new ClumpingTileRendering(4, "ExampleClumpTiles"));
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
