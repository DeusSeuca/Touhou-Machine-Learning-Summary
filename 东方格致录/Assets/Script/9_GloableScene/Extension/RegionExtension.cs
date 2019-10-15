using GameEnum;
using Info;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Extension
{
    public static class RegionExtension
    {
        public static List<SingleRowInfo> InRogin(this List<SingleRowInfo> rows, params RegionTypes[] regions)
        {
            List<SingleRowInfo> targetRows = new List<SingleRowInfo>();
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
        public static List<SingleRowInfo> At(this List<SingleRowInfo> rows, Orientation orientation)
        {
           
            return rows.Where(row => row.orientation == orientation).ToList() ;
        }
        public static void AddCard(this List<SingleRowInfo> rows, Orientation orientation)
        {

        }
    }
}
