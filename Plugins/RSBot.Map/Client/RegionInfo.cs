namespace RSBot.Map.Client
{
    internal class RegionInfo
    {
        public RegionInfoGroup Group;
        public byte XSector;
        public byte YSector;
        public RegionBoundaryType Boundary;
        public int BoundaryX1, BoundaryX2, BoundaryY1, BoundaryY2;
    }

    internal enum RegionBoundaryType
    {
        All,
        Rectangle
    }
}