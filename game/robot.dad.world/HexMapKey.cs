namespace robot.dad.world
{
    /// <summary>
    /// We are going HEX!
    /// </summary>
    public class HexMapKey
    {
        public HexMapKey(long x, long y)
        {
            X = x;
            Y = y;
        }



        public readonly long X;
        public readonly long Y;
        public override bool Equals(object obj)
        {
            var objAsMapKey = obj as HexMapKey;
            if (objAsMapKey == null)
                return false;
            return objAsMapKey.X == X && objAsMapKey.Y == Y;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }
    }

    public struct HexKey
    {
        public long Q;
        public long R;
    }

    public struct CubeKey
    {
        public long X;
        public long Y;
        public long Z;
    }
}
