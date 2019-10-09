using CardSpace;
using GameEnum;
using Info;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Command
{
    public static class RowCommand
    {
        public static async Task CreatTempCard(SingleRowInfo SingleInfo)
        {
            SingleInfo.TempCard = await CardCommand.CreatCard(RowsInfo.GetMyCardList(RegionTypes.Uesd)[0].CardId);
            SingleInfo.TempCard.IsGray = true;
            SingleInfo.TempCard.IsCanSee = true;
            SingleInfo.ThisRowCards.Insert(SingleInfo.Location, SingleInfo.TempCard);
            SingleInfo.TempCard.Init();
        }
        public static void DestoryTempCard(SingleRowInfo SingleInfo)
        {
            SingleInfo.ThisRowCards.Remove(SingleInfo.TempCard);
            GameObject.Destroy(SingleInfo.TempCard.gameObject);
            SingleInfo.TempCard = null;
        }
        public static void ChangeTempCard(SingleRowInfo SingleInfo)
        {
            SingleInfo.ThisRowCards.Remove(SingleInfo.TempCard);
            SingleInfo.ThisRowCards.Insert(SingleInfo.Location, SingleInfo.TempCard);
        }
        public static void RefreshHandCard(List<Card> ThisCardList, bool IsMyHandRegion)
        {
            if (IsMyHandRegion)
            {
                foreach (var item in ThisCardList)
                {
                    if (AgainstInfo.PlayerFocusCard != null && item == AgainstInfo.PlayerFocusCard && item.IsLimit == false)
                    {
                        item.IsPrePrepareToPlay = true;
                    }
                    else
                    {
                        item.IsPrePrepareToPlay = false;
                    }
                }
            }
        }
        public static void SetPlayCardLimit(bool IsLimit)
        {
            Info.AgainstInfo.AllCardList.At(Orientation.Down).InRogin(RegionTypes.Leader, RegionTypes.Hand).ForEach(card => card.IsLimit = IsLimit);
        }
        /// <summary>
        /// 显示可部署区域
        /// </summary>
        /// <param name="CanBeSelected"></param>
       
        public static void SetAllRegionSelectable(RegionTypes region, Territory territory = Territory.All)
        {
            if (region == RegionTypes.None)
            {
                Info.AgainstInfo.AllRegionList.InRogin(RegionTypes.Battle).ForEach(row => row.SetRegionSelectable(false));
            }
            else
            {
                Info.AgainstInfo.AllRegionList.InRogin(region).ForEach(row => row.SetRegionSelectable(false));
            }
        }
    }
}


