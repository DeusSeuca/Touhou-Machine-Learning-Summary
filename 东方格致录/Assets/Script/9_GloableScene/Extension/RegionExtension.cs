using GameEnum;
using Info;
using System.Collections.Generic;
using System.Linq;
namespace Extension
{
    public static class RegionExtension
    {
        public static List<SingleRowInfo> InRogin(this List<SingleRowInfo> rows, params RegionTypes[] regions)
        {
            List<SingleRowInfo> targetRows;
            if (regions.Contains(RegionTypes.Battle))
            {
                targetRows = rows.Where(row =>
                 row.region == RegionTypes.Water ||
                 row.region == RegionTypes.Fire ||
                 row.region == RegionTypes.Wind ||
                 row.region == RegionTypes.Soil
                ).ToList();
            }
            else
            {
                targetRows = rows.Where(row => regions.Contains(row.region)).ToList();
            }
            return targetRows;
        }
        public static List<SingleRowInfo> At(this List<SingleRowInfo> rows, GameEnum.Orientation orientation)
        {
            return rows.Where(row => row.orientation == orientation).ToList(); ;
        }
    }
}
