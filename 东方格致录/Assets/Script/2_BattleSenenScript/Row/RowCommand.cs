using CardSpace;
using Extension;
using GameEnum;
using Info;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Command
{
    public static class RowCommand
    {
        public static void InitRowsInfo()
        {
            RowsInfo.globalCardList.Clear();
            for (int i = 0; i < 18; i++)
            {
                RowsInfo.globalCardList.Add(new List<Card>());
            }
        }
        public static async Task CreatTempCard(SingleRowInfo SingleInfo)
        {
            Card modelCard = AgainstInfo.AllCardList.InRogin(Orientation.My, RegionTypes.Uesd)[0];
            SingleInfo.TempCard = await CardCommand.CreatCard(modelCard.CardId);
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
        public static void RefreshHandCard(List<Card> cardList)
        {
            cardList.ForEach(card => card.IsPrePrepareToPlay = (AgainstInfo.PlayerFocusCard != null && card == AgainstInfo.PlayerFocusCard && !card.IsLimit));
        }
        public static void SetPlayCardLimit(bool IsLimit)
        {
            AgainstInfo.AllCardList.InRogin(Orientation.Down, RegionTypes.Leader, RegionTypes.Hand).ForEach(card => card.IsLimit = IsLimit);
        }
        public static void SetAllRegionSelectable(RegionTypes region, Territory territory = Territory.All)
        {
            if (region == RegionTypes.None)
            {
                AgainstInfo.AllRegionList.InRogin(RegionTypes.Battle).ForEach(row => row.SetRegionSelectable(false));
            }
            else
            {
                AgainstInfo.AllRegionList.InRogin(region).ForEach(row => row.SetRegionSelectable(true));
            }
        }
    }
}


