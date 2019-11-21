using CardModel;
using CardSpace;
using Command.CardInspector;
using Extension;
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
                card.point = CardStandardInfo.point;
                card.icon = CardStandardInfo.icon;
                card.property = CardStandardInfo.cardProperty;
                card.territory = CardStandardInfo.cardTerritory;
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
                GameUI.CardBoardCommand.LoadCardList(AgainstInfo.cardSet[Orientation.My][RegionTypes.Hand].cardList);
            }
        }
        internal static Task RebackCard()
        {
            throw new NotImplementedException();
        }
        public static async Task DrawCard(bool IsPlayerDraw = true, bool ActiveBlackList = false, bool isOrder = true)
        {
            //Debug.Log("抽卡");
            EffectCommand.AudioEffectPlay(0);
            Card TargetCard = AgainstInfo.cardSet[IsPlayerDraw ? Orientation.Down : Orientation.Up][RegionTypes.Deck].cardList[0];
            TargetCard.SetCardSee(IsPlayerDraw);
            CardSet TargetCardtemp = AgainstInfo.cardSet[IsPlayerDraw ? Orientation.Down : Orientation.Up][RegionTypes.Deck];

            AgainstInfo.cardSet[IsPlayerDraw ? Orientation.Down : Orientation.Up][RegionTypes.Deck].Remove(TargetCard);
            AgainstInfo.cardSet[IsPlayerDraw ? Orientation.Down : Orientation.Up][RegionTypes.Hand].Add(TargetCard);
            if (isOrder)
            {
                OrderCard();
            }
            await Task.Delay(100);
        }
        //洗回牌库

        public static async Task WashCard(Card TargetCard, bool IsPlayerWash = true, int InsertRank = 0)
        {
            Debug.Log("洗回卡牌");
            if (IsPlayerWash)
            {
                AgainstInfo.TargetCard = TargetCard;
                int MaxCardRank = AgainstInfo.cardSet[Orientation.Down][RegionTypes.Deck].cardList.Count;
                AgainstInfo.RandomRank = AiCommand.GetRandom(0, MaxCardRank);
                Network.NetCommand.AsyncInfo(NetAcyncType.ExchangeCard);
                AgainstInfo.cardSet[Orientation.Down][RegionTypes.Hand].Remove(TargetCard);
                AgainstInfo.cardSet[Orientation.Down][RegionTypes.Deck].Add(TargetCard, AgainstInfo.RandomRank);
                TargetCard.SetCardSee(false);
            }
            else
            {
                AgainstInfo.cardSet[Orientation.Up][RegionTypes.Hand].Remove(TargetCard);
                AgainstInfo.cardSet[Orientation.Up][RegionTypes.Deck].Add(TargetCard, InsertRank);
            }
            await Task.Delay(500);
        }
        public static void OrderCard()
        {
            AgainstInfo.cardSet[RegionTypes.Hand].Order();
        }
        public static async Task PlayCard(Card targetCard, bool IsAnsy = true)
        {
            //Debug.Log("打出一张牌2");
            EffectCommand.AudioEffectPlay(0);
            RowCommand.SetPlayCardLimit(true);
            //Card TargetCard = AgainstInfo.PlayerPlayCard;
            targetCard.IsPrePrepareToPlay = false;
            if (IsAnsy)
            {
                Network.NetCommand.AsyncInfo(NetAcyncType.PlayCard);
            }
            targetCard.SetCardSee(true);
            targetCard.Row.Remove(targetCard);
            AgainstInfo.cardSet[Orientation.My][RegionTypes.Uesd].Add(targetCard);
            AgainstInfo.PlayerPlayCard = null;
            targetCard.Trigger<TriggerType.PlayCard>();
        }
        public static async Task DisCard(Card card)
        {
            card.IsPrePrepareToPlay = false;
            card.SetCardSee(false);
            card.Row.Remove(card);
            AgainstInfo.cardSet[Orientation.My][RegionTypes.Grave].Add(card);
            Info.AgainstInfo.PlayerPlayCard = null;
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
    }
}