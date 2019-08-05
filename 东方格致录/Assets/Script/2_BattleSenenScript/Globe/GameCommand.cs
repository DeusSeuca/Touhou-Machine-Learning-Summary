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
            if (GlobalBattleInfo.PlayerFocusRegion.ThisRowCards.Count < 5)
            {
                Card TargetCard = GlobalBattleInfo.PlayerPlayCard;
                TargetCard.IsPrePrepareToPlay = false;
                //RowsInfo.Instance.SingleOtherInfos[RegionName_Other.My_Hand].ThisRowCard.Remove(TargetCard);
                GlobalBattleInfo.PlayerFocusRegion.ThisRowCards.Add(TargetCard);
                GlobalBattleInfo.IsCardEffectCompleted = true;
            }
        }
        public static void PlayCardToGraveyard()
        {

        }
        // 限制手牌被打出
        public static void PlayCardLimit(bool IsLimit)
        {
            RowsInfo.GetRegionCardList(RegionName_Other.My_Hand).ThisRowCards.ForEach(card => card.IsLimit = IsLimit);
        }
        //待扩展
        public static List<Card> GetCardList(LoadRangeOnBattle OnBattle = LoadRangeOnBattle.None, LoadRangeOnOther OnOther = LoadRangeOnOther.None)
        {
            List<Card> CardList = new List<Card>();
            CardList.AddRange(OnBattle.Contain(LoadRangeOnBattle.My_Water) ? RowsInfo.GetMyCardList(RegionTypes.Water) : new List<Card>());
            CardList.AddRange(OnBattle.Contain(LoadRangeOnBattle.My_Fire) ? RowsInfo.GetMyCardList(RegionTypes.Fire) : new List<Card>());
            CardList.AddRange(OnBattle.Contain(LoadRangeOnBattle.My_Wind) ? RowsInfo.GetMyCardList(RegionTypes.Wind) : new List<Card>());
            CardList.AddRange(OnBattle.Contain(LoadRangeOnBattle.My_Soil) ? RowsInfo.GetMyCardList(RegionTypes.Soil) : new List<Card>());
            CardList.AddRange(OnBattle.Contain(LoadRangeOnBattle.Op_Water) ? RowsInfo.GetOpCardList(RegionTypes.Water) : new List<Card>());
            CardList.AddRange(OnBattle.Contain(LoadRangeOnBattle.Op_Fire) ? RowsInfo.GetOpCardList(RegionTypes.Fire) : new List<Card>());
            CardList.AddRange(OnBattle.Contain(LoadRangeOnBattle.Op_Wind) ? RowsInfo.GetOpCardList(RegionTypes.Wind) : new List<Card>());
            CardList.AddRange(OnBattle.Contain(LoadRangeOnBattle.Op_Soil) ? RowsInfo.GetOpCardList(RegionTypes.Soil) : new List<Card>());
            return CardList;
        }

    }
}

