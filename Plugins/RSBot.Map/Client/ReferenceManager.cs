using System.Collections.Generic;
using RSBot.Core;
using RSBot.Core.Client;
using RSBot.Core.Event;
using System.Linq;

namespace RSBot.Map.Client
{
    internal class ReferenceManager
    {
        private const string FileName = "regioninfo.txt";

        public static List<RegionInfo> RegionInfos { get; private set; }

        public static void SubscribeEvents()
        {
            EventManager.SubscribeEvent("OnLoadGameData", OnLoadGameData);
        }

        private static void Load()
        {
            if (!Game.MediaPk2.FileExists(FileName))
            {
                Log.Error($"[Map] {FileName} file does not exist!");

                return;
            }

            var file = Game.MediaPk2.GetFile(FileName);

            RegionInfoGroup currentRegionGroup = null;
            var content = file.ReadAllText();
            foreach (var line in content.Split('\n').Where(s => !string.IsNullOrEmpty(s)))
            {
                var parser = new ReferenceParser(line);

                if (line.StartsWith('#'))
                {
                    currentRegionGroup = new RegionInfoGroup
                    {
                        Type = line.StartsWith("#TOWN") ? RegionType.Town : RegionType.Field
                    };

                    parser.TryParse(2, out currentRegionGroup.MinimapFolder);
                }
                else
                {
                    if (currentRegionGroup == null)
                        continue;
                    
                    var regionInfo = new RegionInfo
                    {
                        Group = currentRegionGroup
                    };

                    parser.TryParse(0, out regionInfo.XSector);
                    parser.TryParse(1, out regionInfo.YSector);
                    parser.TryParse(2, out string boundaryType);

                    regionInfo.Boundary = boundaryType switch
                    {
                        "ALL" => RegionBoundaryType.All,
                        "RECT" => RegionBoundaryType.Rectangle,
                        _ => regionInfo.Boundary
                    };

                    if (regionInfo.Boundary == RegionBoundaryType.Rectangle)
                    {
                        parser.TryParse(3, out regionInfo.BoundaryX1);
                        parser.TryParse(4, out regionInfo.BoundaryX2);
                        parser.TryParse(5, out regionInfo.BoundaryY1);
                        parser.TryParse(6, out regionInfo.BoundaryY2);
                    }

                    RegionInfos.Add(regionInfo);
                }
            }

            Log.Debug($"[Map] Loaded {RegionInfos.Count} regions from {FileName}");
        }

        private static void OnLoadGameData()
        {
            RegionInfos = new List<RegionInfo>(1024);

            Load();
        }
    }
}