using Extension;
using GameEnum;
using System.Linq;
namespace Info
{
    public static class PointInfo
    {
        //public static int DownWaterPoint => AgainstInfo.AllCardList.InRogin( Orientation.Down, RegionTypes.Water).Where(card => !card.IsGray).Select(card => card.CardPoint).Sum();
        //public static int DownFirePoint => RowsInfo.GetDownCardList(RegionTypes.Fire).Where(card => !card.IsGray).Select(card => card.CardPoint).Sum();
        //public static int DownWindPoint => RowsInfo.GetDownCardList(RegionTypes.Wind).Where(card => !card.IsGray).Select(card => card.CardPoint).Sum();
        //public static int DownSoilPoint => RowsInfo.GetDownCardList(RegionTypes.Soil).Where(card => !card.IsGray).Select(card => card.CardPoint).Sum();

        //public static int UpWaterPoint => RowsInfo.GetUpCardList(RegionTypes.Water).Where(card => !card.IsGray).Select(card => card.CardPoint).Sum();
        //public static int UpFirePoint => RowsInfo.GetUpCardList(RegionTypes.Fire).Where(card => !card.IsGray).Select(card => card.CardPoint).Sum();
        //public static int UpWindPoint => RowsInfo.GetUpCardList(RegionTypes.Wind).Where(card => !card.IsGray).Select(card => card.CardPoint).Sum();
        //public static int UpSoilPoint => RowsInfo.GetUpCardList(RegionTypes.Soil).Where(card => !card.IsGray).Select(card => card.CardPoint).Sum();
        //public static int TotalUpPoint => UpWaterPoint + UpFirePoint + UpWindPoint + UpSoilPoint;
        //public static int TotalDownPoint => DownWaterPoint + DownFirePoint + DownWindPoint + DownSoilPoint;
        public static int TotalUpPoint => AgainstInfo.cardSet[Orientation.Up][ RegionTypes.Battle].cardList.Sum(card=>card.point);
        public static int TotalDownPoint => AgainstInfo.cardSet[Orientation.Down][ RegionTypes.Battle].cardList.Sum(card => card.point);
        public static int TotalPlayer1Point => AgainstInfo.IsPlayer1 ? TotalDownPoint : TotalUpPoint;
        public static int TotalPlayer2Point => AgainstInfo.IsPlayer1 ? TotalUpPoint : TotalDownPoint;
    }

}
