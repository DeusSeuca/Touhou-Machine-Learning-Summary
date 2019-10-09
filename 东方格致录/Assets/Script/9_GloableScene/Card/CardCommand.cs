using CardSpace;
using GameEnum;
using Info;
using System;
using System.Linq;
using System.Threading.Tasks;
using Thread;
using UnityEngine;

namespace Command
{
    public static class CardCommand
    {
         static int CreatCardRank;//卡牌创建时的自增命名

        public static async Task<Card> CreatCard(int id)
        {
            GameObject NewCard;
            Card NewCardScript = null;
            MainThread.Run(() =>
            {
                NewCard = GameObject.Instantiate(Info.CardInfo.cardModel);
                NewCard.name = "Card" + CreatCardRank++;
                var CardStandardInfo = CardLibraryCommand.GetCardStandardInfo(id);
                NewCard.AddComponent(Type.GetType("CardSpace.Card" + id));
                Card card = NewCard.GetComponent<Card>();
                card.CardId = CardStandardInfo.cardId;
                card.CardPoint = CardStandardInfo.point;
                card.icon = CardStandardInfo.icon;
                card.property = CardStandardInfo.cardProperty;
                card.CardTerritory = CardStandardInfo.cardTerritory;
                card.GetComponent<Renderer>().material.SetTexture("_Front", card.icon);
                card.Init();
                NewCardScript = card;
            });
            await Task.Run(() => { while (NewCardScript == null) { } });
            return NewCardScript;
        }
        public static async Task ExchangeCard(Card TargetCard, bool IsPlayerExchange = true, int RandomRank = 0)
        {
            //Debug.Log("交换卡牌");
            await WashCard(TargetCard, IsPlayerExchange, RandomRank);
            await DrawCard(IsPlayerExchange, true);
            if (IsPlayerExchange)
            {
                GameUI.CardBoardCommand.LoadCardList(RowsInfo.GetMyCardList(RegionTypes.Hand));
            }
        }
        internal static Task RebackCard()
        {
            throw new NotImplementedException();
        }
        public static async Task DrawCard(bool IsPlayerDraw = true, bool ActiveBlackList = false)
        {
            //Debug.Log("抽卡");
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
            OrderCard();
            await Task.Delay(100);
        }
        //洗回牌库

        public static async Task WashCard(Card TargetCard, bool IsPlayerWash = true, int InsertRank = 0)
        {
            Debug.Log("洗回卡牌");
            if (IsPlayerWash)
            {
                AgainstInfo.TargetCard = TargetCard;
                int MaxCardRank = Info.RowsInfo.GetDownCardList(RegionTypes.Deck).Count;
                AgainstInfo.RandomRank = AiCommand.GetRandom(0, MaxCardRank);
                Network.NetCommand.AsyncInfo(NetAcyncType.ExchangeCard);
                RowsInfo.GetDownCardList(RegionTypes.Hand).Remove(TargetCard);
                RowsInfo.GetDownCardList(RegionTypes.Deck).Insert(AgainstInfo.RandomRank, TargetCard);
                TargetCard.IsCanSee = false;
            }
            else
            {
                RowsInfo.GetUpCardList(RegionTypes.Hand).Remove(TargetCard);
                RowsInfo.GetUpCardList(RegionTypes.Deck).Insert(InsertRank, TargetCard);
            }
            await Task.Delay(500);
        }
        public static void OrderCard(bool IsPlayerWash = true)
        {
            RowsInfo.globalCardList[1] = RowsInfo.globalCardList[1].OrderBy(card => card.CardPoint).ToList();
            RowsInfo.globalCardList[3] = RowsInfo.globalCardList[3].OrderBy(card => card.CardPoint).ToList();
            RowsInfo.globalCardList[10] = RowsInfo.globalCardList[10].OrderBy(card => card.CardPoint).ToList();
            RowsInfo.globalCardList[12] = RowsInfo.globalCardList[12].OrderBy(card => card.CardPoint).ToList();

        }
        public static async Task PlayCard(bool IsAnsy)
        {
            Debug.Log("打出一张牌2");
            Command.EffectCommand.AudioEffectPlay(0);
            RowCommand.SetPlayCardLimit(true);
            Card TargetCard = AgainstInfo.PlayerPlayCard;
            TargetCard.IsPrePrepareToPlay = false;
            Network.NetCommand.AsyncInfo(NetAcyncType.PlayCard);
            TargetCard.IsCanSee = true;
            RowsInfo.GetRow(TargetCard).Remove(TargetCard);
            RowsInfo.GetMyCardList(RegionTypes.Uesd).Add(TargetCard);
            AgainstInfo.PlayerPlayCard = null;
            TargetCard.Trigger<TriggerType.PlayCard>();
        }
        public static async Task DisCard(Card card)
        {
            //Card TargetCard = card == null ? AgainstInfo.PlayerPlayCard : card;
            card.IsPrePrepareToPlay = false;
            card.IsCanSee = false;
            card.Row.Remove(card);
            RowsInfo.GetMyCardList(RegionTypes.Grave).Add(card);
            card.Trigger<TriggerType.Discard>();
        }
        //强行移过来的
        public static void PlayCardToRegion()
        {
            if (AgainstInfo.PlayerFocusRegion.ThisRowCards.Count < 5)
            {
                Card TargetCard = AgainstInfo.PlayerPlayCard;
                TargetCard.IsPrePrepareToPlay = false;
                AgainstInfo.PlayerFocusRegion.ThisRowCards.Add(TargetCard);
                AgainstInfo.IsCardEffectCompleted = true;
            }
        }
        //强行移过来的
        public static void PlayCardToGraveyard()
        {
        }
    }
}