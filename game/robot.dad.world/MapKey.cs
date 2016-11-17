namespace robot.dad.world
{
    public class MapKey
    {
        public MapKey(long x, long y)
        {
            X = x;
            Y = y;
        }
        public readonly long X;
        public readonly long Y;
        public override bool Equals(object obj)
        {
            var objAsMapKey = obj as MapKey;
            if (objAsMapKey == null)
                return false;
            return objAsMapKey.X == X && objAsMapKey.Y == Y;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }
    }
}
