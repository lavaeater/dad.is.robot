﻿namespace robot.dad.world
{
    public class MapTile
    {
        public MapTile(HexMapKey key, MapTileType tileType)
        {
            Key = key;
            TileType = tileType;
        }
        public readonly HexMapKey Key;
        public long X => Key.X;
        public long Y => Key.Y;
        public readonly MapTileType TileType;

        public override string ToString()
        {
            return $"{X}:{Y}|{TileType}";
        }
    }
}