using CardSpace;
using Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Command
{
    public class CardCommand
    {
        public static Card CreatCardMain(int id)
        {
            GameObject NewCard;
            NewCard = GameObject.Instantiate(CardLibrary.Instance.Card_Model);
            NewCard.name = "Card" + Info.GlobalBattleInfo.CreatCardRank++;
            var CardStandardInfo = CardLibrary.GetCardStandardInfo(id);
            NewCard.AddComponent(Type.GetType("Card" + id));
            Card NewCardScript = NewCard.GetComponent<Card>();
            NewCardScript.CardId = CardStandardInfo.CardId;
            NewCardScript.CardPoint = CardStandardInfo.Point;
            NewCardScript.icon = CardStandardInfo.Icon;
            NewCardScript.CardProperty = CardStandardInfo.CardProperty;
            NewCardScript.CardTerritory = CardStandardInfo.CardTerritory;
            NewCardScript.GetComponent<Renderer>().material.SetTexture("_Front", NewCardScript.icon);
            NewCardScript.Init();
            //print("异步生成卡牌");
            return NewCardScript;
        }
        public static async Task<Card> CreatCard(int id)
        {
            GameObject NewCard;
            Card NewCardScript = null;
            MainThread.Run(() =>
            {
                NewCard = GameObject.Instantiate(CardLibrary.Instance.Card_Model);
                NewCard.name = "Card" + Info.GlobalBattleInfo.CreatCardRank++;
                var CardStandardInfo = CardLibrary.GetCardStandardInfo(id);
                NewCard.AddComponent(Type.GetType("Card" + id));
                Card card = NewCard.GetComponent<Card>();
                card.CardId = CardStandardInfo.CardId;
                card.CardPoint = CardStandardInfo.Point;
                card.icon = CardStandardInfo.Icon;
                card.CardProperty = CardStandardInfo.CardProperty;
                card.CardTerritory = CardStandardInfo.CardTerritory;
                card.GetComponent<Renderer>().material.SetTexture("_Front", card.icon);
                card.Init();
                NewCardScript = card;
            });
            await Task.Run(() => { while (NewCardScript == null) { } });
            //print("异步生成卡牌");
            return NewCardScript;
        }
        //public static async Task<Card> CreatCard(int id)
        //{
        //    Info.GlobalBattleInfo.TargetCardID = id;
        //    Info.GlobalBattleInfo.IsCreatCard = true;
        //    await Task.Run(() => { while (Info.GlobalBattleInfo.CreatedCard == null) { } });
        //    Card NewCard = Info.GlobalBattleInfo.CreatedCard;
        //    Info.GlobalBattleInfo.CreatedCard = null;
        //    //print("异步生成卡牌");
        //    return NewCard;
        //}
        public static async Task ExchangeCard(bool IsPlayerWash = true)
        {
            Debug.Log("交换卡牌");
            await WashCard();
            await DrawCard();
            CardBoardCommand.LoadCardList(RowsInfo.GetMyCardList(RegionTypes.Hand));
            //UiCommand.CardBoardReload();
        }

        internal static Task RebackCard()
        {
            throw new NotImplementedException();
        }

        public static async Task DrawCard(bool IsPlayerDraw = true, bool ActiveBlackList = false)
        {
            EffectCommand.AudioEffectPlay(0);
            Card TargetCard = IsPlayerDraw ? RowsInfo.GetDownCardList(RegionTypes.Deck)[0] : RowsInfo.GetUpCardList(RegionTypes.Deck)[0];
            TargetCard.IsCanSee = IsPlayerDraw;
            if (IsPlayerDraw)
            {
                RowsInfo.GetDownCardList(RegionTypes.Deck).Remove(TargetCard);
                RowsInfo.GetDownCardList(RegionTypes.Hand).Add(TargetCard);

            }
            else
            {
                RowsInfo.GetUpCardList(RegionTypes.Deck).Remove(TargetCard);
                RowsInfo.GetUpCardList(RegionTypes.Hand).Add(TargetCard);


            }
            await OrderCard();
            await Task.Delay(100);
        }
        //洗回牌库
        public static async Task WashCard(bool IsPlayerWash = true)
        {
            Debug.Log("洗回卡牌");
            if (IsPlayerWash)
            {
                int MaxCardRank = Info.RowsInfo.GetMyCardList(RegionTypes.Deck).Count;
                int CardRank = AiCommand.GetRandom(0, MaxCardRank);
                GlobalBattleInfo.SelectLocation = CardRank;
                GlobalBattleInfo.SelectRegion = RowsInfo.GetRegionCardList(RegionName_Other.My_Deck);
                GlobalBattleInfo.TargetCard = GlobalBattleInfo.SingleSelectCardOnBoard;
                GlobalBattleInfo.TargetCard.IsCanSee = false;
                await MoveCard();
            }
            else
            {
                //int MaxCardRank = Info.RowsInfo.GetDownCardList(RegionTypes.Hand).Count;
                //int CardRank = AiCommand.GetRandom(0, MaxCardRank);
                //GlobalBattleInfo.SelectLocation = CardRank;
                //GlobalBattleInfo.SelectRegion = RowsInfo.GetRegionCardList(RegionName_Other.My_Hand);

                //await MoveCard();



            }
            await Task.Delay(500);
        }
        public static async Task OrderCard(bool IsPlayerWash = true)
        {
            RowsInfo.GlobalCardList[1] = RowsInfo.GlobalCardList[1].OrderBy(card => card.CardPoint).ToList();
            RowsInfo.GlobalCardList[3] = RowsInfo.GlobalCardList[3].OrderBy(card => card.CardPoint).ToList();
            RowsInfo.GlobalCardList[10] = RowsInfo.GlobalCardList[10].OrderBy(card => card.CardPoint).ToList();
            RowsInfo.GlobalCardList[12] = RowsInfo.GlobalCardList[12].OrderBy(card => card.CardPoint).ToList();

        }
        public static async Task MoveCard()
        {
            Debug.Log("移动卡牌");

            Card TargetCard = GlobalBattleInfo.TargetCard;
            List<Card> OriginRow = RowsInfo.GetRow(TargetCard);
            List<Card> TargetRow = GlobalBattleInfo.SelectRegion.ThisRowCard;
            Debug.Log("移动卡牌从" + OriginRow.Count + "到" + TargetRow.Count);
            OriginRow.Remove(TargetCard);
            TargetRow.Insert(GlobalBattleInfo.SelectLocation, TargetCard);
            //GlobalBattleInfo.SelectLocation
            //GlobalBattleInfo.SelectRegion = RowsInfo.GetRegionCardList(RegionName_Other.My_Hand);
        }
        public static async Task PlayCard()
        {
            Debug.Log("打出卡牌");
            Command.EffectCommand.AudioEffectPlay(0);
            GameCommand.PlayCardLimit(true);
            Card TargetCard = GlobalBattleInfo.PlayerPlayCard;
            TargetCard.IsPrePrepareToPlay = false;
            Command.NetCommand.AsyncInfo(NetAcyncType.PlayCard);
            TargetCard.IsCanSee = true;
            Debug.LogError("移除卡牌");
            RowsInfo.GetMyCardList(RegionTypes.Hand).Remove(TargetCard);
            Debug.LogError("加入卡牌");
            RowsInfo.GetMyCardList(RegionTypes.Uesd).Add(TargetCard);
            //if (Info.GlobalBattleInfo.IsMyTurn)
            //{
            //    Command.NetCommand.AsyncInfo(GameEnum.NetAcyncType.PlayCard);
            //    RowsInfo.GetMyCardList(RegionTypes.Hand).Remove(TargetCard);
            //    RowsInfo.GetMyCardList(RegionTypes.Uesd).Add(TargetCard);
            //}
            //else
            //{
            //    RowsInfo.GetOpCardList(RegionTypes.Hand).Remove(TargetCard);
            //    RowsInfo.GetOpCardList(RegionTypes.Uesd).Add(TargetCard);
            //}
            GlobalBattleInfo.PlayerPlayCard = null;
            TargetCard.Trigger<TriggerType.PlayCard>();
        }
        public static async Task DisCard(Card card = null)
        {
            //print("丢弃卡牌");
            Card TargetCard = card == null ? GlobalBattleInfo.PlayerPlayCard : card;
            TargetCard.IsPrePrepareToPlay = false;
            TargetCard.Row.Remove(TargetCard);
            RowsInfo.GetMyCardList(RegionTypes.Grave).Add(TargetCard);
            TargetCard.Trigger<TriggerType.Discard>();
            //await CardEffectStackControl.TriggerEffect<TriggerType.Discard>(TargetCard);
            //GlobeBattleInfo.IsCardEffectCompleted = true;
        }

    }
}