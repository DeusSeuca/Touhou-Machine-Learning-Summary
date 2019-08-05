using CardSpace;
using Info;
using System.Collections.Generic;
using System.Linq;
namespace Command
{
    public class RowCommand
    {
        public static void SetAllRegionSelectable(bool CanBeSelected)
        {
            //singleRow.CanBeSelected = CanBeSelected;
            if (CanBeSelected)
            {
                List<SingleRowInfo> TargetSingleRow = new List<SingleRowInfo>();
                Card DeployCard = RowsInfo.GetMyCardList(RegionTypes.Uesd)[0];
                bool IsMyTerritory = (DeployCard.CardTerritory == Territory.My);
                switch (DeployCard.CardProperty)
                {
                    case Property.Water:
                        {
                            TargetSingleRow.Add(Info.RowsInfo.GetSingleRowInfo(RegionTypes.Water, IsMyTerritory));
                            break;
                        }
                    case Property.Fire:
                        {
                            TargetSingleRow.Add(Info.RowsInfo.GetSingleRowInfo(RegionTypes.Fire, IsMyTerritory));

                            break;
                        }
                    case Property.Wind:
                        {
                            TargetSingleRow.Add(Info.RowsInfo.GetSingleRowInfo(RegionTypes.Wind, IsMyTerritory));

                            break;
                        }
                    case Property.Soil:
                        {
                            TargetSingleRow.Add(Info.RowsInfo.GetSingleRowInfo(RegionTypes.Soil, IsMyTerritory));

                            break;
                        }
                    case Property.None:
                        break;
                    default:
                        break;
                }
                TargetSingleRow.ForEach(SingleRow => SingleRow.SetRegionSelectable(true));
            }
            else
            {
                RowsInfo.Instance.SingleBattleInfos.Values.ToList().ForEach(row => row.SetRegionSelectable(false));
            }
        }
       
            
        //public static void SetRegionSelectable(SingleRowInfo singleRow, bool CanBeSelected)
        //{
        //    singleRow.CanBeSelected = CanBeSelected;

        //}
    }
    //[System.Obsolete("什么破垃圾代码")]
    //public static void SetRegionSelectable(bool CanBeSelected)
    //{

    //    if (CanBeSelected)
    //    {
    //        Card DeployCard = RowsInfo.GetMyCardList(RegionTypes.Uesd)[0];
    //        bool IsMyTerritory = (DeployCard.CardTerritory == Territory.My);
    //        switch (RowsInfo.GetMyCardList(RegionTypes.Uesd)[0].CardProperty)
    //        {
    //            case Property.Water:
    //                {

    //                    //SetRowShow(IsMyTerritory ? RegionName_Battle.My_Water : RegionName_Battle.Op_Water);
    //                    break;
    //                }
    //            case Property.Fire:
    //                {
    //                    SetRowShow(IsMyTerritory ? RegionName_Battle.My_Fire : RegionName_Battle.Op_Fire);
    //                    break;
    //                }
    //            case Property.Wind:
    //                {
    //                    SetRowShow(IsMyTerritory ? RegionName_Battle.My_Wind : RegionName_Battle.Op_Wind);
    //                    break;
    //                }
    //            case Property.Soil:
    //                {
    //                    SetRowShow(IsMyTerritory ? RegionName_Battle.My_Soil : RegionName_Battle.Op_Soil);
    //                    break;
    //                }
    //            case Property.None:
    //                break;
    //            default:
    //                break;
    //        }
    //    }
    //    else
    //    {
    //        RowsInfo.Instance.SingleBattleInfos.Values.ToList().ForEach(row => row.Control.SetSelectable(false));
    //    }
    //}
    //[System.Obsolete("过时啦")]
    //private static void SetRowShow(RegionName_Battle row) => RowsInfo.GetRegionCardList(row).Control.SetSelectable(true);
}


