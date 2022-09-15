namespace RSBot.Map.Client
{
    internal class RegionInfoGroup
    {
        /// <summary>
        /// The type
        /// </summary>
        public RegionType Type;
        
        /// <summary>
        /// The minimap folder in case of dungeons
        /// </summary>
        public string MinimapFolder;
    }

    internal enum RegionType
    {
        Town,
        Field,
    }
}