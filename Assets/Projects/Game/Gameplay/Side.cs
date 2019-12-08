namespace Game.Gameplay {
    public enum Side : byte {
        Top,
        Bottom
    }

    public static class SideUtils {
        public static Side Opposite(this Side side) {
            return side == Side.Top ? Side.Bottom : Side.Top;
        }
    }
}