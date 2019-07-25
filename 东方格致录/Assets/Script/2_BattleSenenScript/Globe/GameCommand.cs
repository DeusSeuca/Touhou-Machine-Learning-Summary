using CardSpace;
using Info;
using System.Collections.Generic;

namespace Command
{
    public class GameCommand
    {
        public static void InitDeck()
        {

        }
        public static void PlayCardToRegion()
        {
            if (GlobalBattleInfo.PlayerFocusRegion.ThisRowCard.Count < 5)
            {
                Card TargetCard = GlobalBattleInfo.PlayerPlayCard;
                TargetCard.IsPrePrepareToPlay = false;
                //RowsInfo.Instance.SingleOtherInfos[RegionName_Other.My_Hand].ThisRowCard.Remove(TargetCard);
                GlobalBattleInfo.PlayerFocusRegion.ThisRowCard.Add(TargetCard);
                GlobalBattleInfo.IsCardEffectCompleted = true;
            }
        }
        public static void PlayCardToGraveyard()
        {

        }

        /// <summary>
        /// 限制手牌被打出
        /// </summary>
        /// <param name="IsOpen"></param>
        public static void PlayCardLimit(bool IsLimit)
        {
            RowsInfo.GetRegionCardList(RegionName_Other.My_Hand).ThisRowCard.ForEach(card => card.IsLimit = IsLimit);
        }
        //待扩展
        public static List<Card> GetCardList(LoadRangeOnBattle OnBattle = LoadRangeOnBattle.None, LoadRangeOnOther OnOther = LoadRangeOnOther.None)
        {
            List<Card> CardList = new List<Card>();
            CardList.AddRange(OnBattle.Contain(LoadRangeOnBattle.My_Water) ? RowsInfo.GetDownCardList(RegionTypes.Water) : new List<Card>());
            CardList.AddRange(OnBattle.Contain(LoadRangeOnBattle.My_Fire) ? RowsInfo.GetDownCardList(RegionTypes.Fire) : new List<Card>());
            CardList.AddRange(OnBattle.Contain(LoadRangeOnBattle.My_Wind) ? RowsInfo.GetDownCardList(RegionTypes.Wind) : new List<Card>());
            CardList.AddRange(OnBattle.Contain(LoadRangeOnBattle.My_Soil) ? RowsInfo.GetDownCardList(RegionTypes.Soil) : new List<Card>());
            CardList.AddRange(OnBattle.Contain(LoadRangeOnBattle.Op_Water) ? RowsInfo.GetUpCardList(RegionTypes.Water) : new List<Card>());
            CardList.AddRange(OnBattle.Contain(LoadRangeOnBattle.Op_Fire) ? RowsInfo.GetUpCardList(RegionTypes.Fire) : new List<Card>());
            CardList.AddRange(OnBattle.Contain(LoadRangeOnBattle.Op_Wind) ? RowsInfo.GetUpCardList(RegionTypes.Wind) : new List<Card>());
            CardList.AddRange(OnBattle.Contain(LoadRangeOnBattle.Op_Soil) ? RowsInfo.GetUpCardList(RegionTypes.Soil) : new List<Card>());
            return CardList;
        }

    }
}

