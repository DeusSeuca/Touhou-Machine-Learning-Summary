﻿using GameEnum;
using System.Linq;
namespace Info
{
    public class PointInfo
    {
        public static int DownWaterPoint => Info.RowsInfo.GetDownCardList(RegionTypes.Water).Where(card => !card.IsGray).Select(card => card.CardPoint).Sum();
        public static int DownFirePoint => Info.RowsInfo.GetDownCardList(RegionTypes.Fire).Where(card => !card.IsGray).Select(card => card.CardPoint).Sum();
        public static int DownWindPoint => Info.RowsInfo.GetDownCardList(RegionTypes.Wind).Where(card => !card.IsGray).Select(card => card.CardPoint).Sum();
        public static int DownSoilPoint => Info.RowsInfo.GetDownCardList(RegionTypes.Soil).Where(card => !card.IsGray).Select(card => card.CardPoint).Sum();

        public static int UpWaterPoint => Info.RowsInfo.GetUpCardList(RegionTypes.Water).Where(card => !card.IsGray).Select(card => card.CardPoint).Sum();
        public static int UpFirePoint => Info.RowsInfo.GetUpCardList(RegionTypes.Fire).Where(card => !card.IsGray).Select(card => card.CardPoint).Sum();
        public static int UpWindPoint => Info.RowsInfo.GetUpCardList(RegionTypes.Wind).Where(card => !card.IsGray).Select(card => card.CardPoint).Sum();
        public static int UpSoilPoint => Info.RowsInfo.GetUpCardList(RegionTypes.Soil).Where(card => !card.IsGray).Select(card => card.CardPoint).Sum();
        public static int TotalUpPoint => UpWaterPoint + UpFirePoint + UpWindPoint + UpSoilPoint;
        public static int TotalDownPoint => DownWaterPoint + DownFirePoint + DownWindPoint + DownSoilPoint;
        public static int TotalPlayer1Point => AgainstInfo.IsPlayer1 ? TotalDownPoint : TotalUpPoint;
        public static int TotalPlayer2Point => AgainstInfo.IsPlayer1 ? TotalUpPoint : TotalDownPoint;
    }

}
