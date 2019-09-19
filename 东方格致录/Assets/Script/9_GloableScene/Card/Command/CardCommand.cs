using CardSpace;
using GameEnum;
using Info;
using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Thread;
using UnityEngine;
using static Network.NetInfoModel;

namespace Command
{
    public class CardCommand
    {      
        public static async Task<Card> CreatCard(int id)
        {
            GameObject NewCard;
            Card NewCardScript = null;
            MainThread.Run(() =>
            {
                NewCard = GameObject.Instantiate(global::CardLibraryCommand.Instance.Card_Model);
                NewCard.name = "Card" + Info.GlobalBattleInfo.CreatCardRank++;
                var CardStandardInfo = global::CardLibraryCommand.GetCardStandardInfo(id);
                NewCard.AddComponent(Type.GetType("Card" + id));
                Card card = NewCard.GetComponent<Card>();
                card.CardId = CardStandardInfo.cardId;
                card.CardPoint = CardStandardInfo.point;
                card.icon = CardStandardInfo.icon;
                card.CardProperty = CardStandardInfo.cardProperty;
                card.CardTerritory = CardStandardInfo.cardTerritory;
                card.GetComponent<Renderer>().material.SetTexture("_Front", card.icon);
                card.Init();
                NewCardScript = card;
            });
            await Task.Run(() => { while (NewCardScript == null) { } });
            return NewCardScript;
        }      
        public static async Task ExchangeCard(Card TargetCard, bool IsPlayerExchange = true,int RandomRank=0)
        {
            Debug.Log("交换卡牌");
            await WashCard(TargetCard, IsPlayerExchange, RandomRank);
            await DrawCard(IsPlayerExchange,true);
            if (IsPlayerExchange)
            {
                CardBoardCommand.LoadCardList(RowsInfo.GetMyCardList(RegionTypes.Hand));
            }
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
       
        public static async Task WashCard(Card TargetCard, bool IsPlayerWash = true, int InsertRank = 0)
        {
            Debug.Log("洗回卡牌");
            if (IsPlayerWash)
            {
                GlobalBattleInfo.TargetCard = TargetCard;
                int MaxCardRank = Info.RowsInfo.GetDownCardList(RegionTypes.Deck).Count;
                GlobalBattleInfo.RandomRank = AiCommand.GetRandom(0, MaxCardRank);
                Network.NetCommand.AsyncInfo(NetAcyncType.ExchangeCard);
                RowsInfo.GetDownCardList(RegionTypes.Hand).Remove(TargetCard);
                RowsInfo.GetDownCardList(RegionTypes.Deck).Insert(GlobalBattleInfo.RandomRank, TargetCard);
                TargetCard.IsCanSee = false;
            }
            else
            {
                RowsInfo.GetUpCardList(RegionTypes.Hand).Remove(TargetCard);
                RowsInfo.GetUpCardList(RegionTypes.Deck).Insert(InsertRank, TargetCard);
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
        [System.Obsolete("待废弃")]
        public static async Task PlayCard()
        {
            Command.EffectCommand.AudioEffectPlay(0);
            GameCommand.SetPlayCardLimit(true);
            Card TargetCard = GlobalBattleInfo.PlayerPlayCard;
            TargetCard.IsPrePrepareToPlay = false;
            NetCommand.AsyncInfo(NetAcyncType.PlayCard);
            TargetCard.IsCanSee = true;
            RowsInfo.GetMyCardList(RegionTypes.Hand).Remove(TargetCard);
            RowsInfo.GetMyCardList(RegionTypes.Uesd).Add(TargetCard);
            GlobalBattleInfo.PlayerPlayCard = null;
            TargetCard.Trigger<TriggerType.PlayCard>();
        }
        public static async Task PlayCard(bool IsAnsy)
        {
            Command.EffectCommand.AudioEffectPlay(0);
            GameCommand.SetPlayCardLimit(true);
            Card TargetCard = GlobalBattleInfo.PlayerPlayCard;
            TargetCard.IsPrePrepareToPlay = false;
            NetCommand.AsyncInfo(NetAcyncType.PlayCard);
            TargetCard.IsCanSee = true;
            RowsInfo.GetRow(TargetCard).Remove(TargetCard);
            RowsInfo.GetMyCardList(RegionTypes.Uesd).Add(TargetCard);
            GlobalBattleInfo.PlayerPlayCard = null;
            TargetCard.Trigger<TriggerType.PlayCard>();
        }
        public static async Task DisCard(Card card = null)
        {
            Card TargetCard = card == null ? GlobalBattleInfo.PlayerPlayCard : card;
            TargetCard.IsPrePrepareToPlay = false;
            TargetCard.Row.Remove(TargetCard);
            RowsInfo.GetMyCardList(RegionTypes.Grave).Add(TargetCard);
            TargetCard.Trigger<TriggerType.Discard>();
        }
    }
}