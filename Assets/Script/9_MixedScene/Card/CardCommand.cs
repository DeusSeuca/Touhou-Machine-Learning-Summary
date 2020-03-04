using CardModel;
using CardSpace;
using Command.CardInspector;
using Control;
using Extension;
using GameEnum;
using Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Thread;
using UnityEngine;

namespace Command
{
    //具体实现，还需进一步简化
    public static class CardCommand
    {
        public static void OrderCard() => AgainstInfo.cardSet[RegionTypes.Hand].Order();
        public static void RemoveCard(Card card) => card.belongCardList.Remove(card);
        public static async Task<Card> CreatCard(int id)
        {
            //GameObject NewCard;
            Card NewCardScript = null;
            MainThread.Run(() =>
            {
                GameObject NewCard = GameObject.Instantiate(Info.CardInfo.cardModel);
                NewCard.transform.SetParent(GameObject.FindGameObjectWithTag("Card").transform);
                NewCard.name = "Card" + Info.CardInfo.CreatCardRank++;
                var CardStandardInfo = CardLibraryCommand.GetCardStandardInfo(id);
                NewCard.AddComponent(Type.GetType("CardSpace.Card" + id));
                Card card = NewCard.GetComponent<Card>();
                card.CardId = CardStandardInfo.cardId;
                card.basePoint = CardStandardInfo.point;
                card.icon = CardStandardInfo.icon;
                card.region = CardStandardInfo.cardProperty;
                card.territory = CardStandardInfo.cardTerritory;
                card.cardTag = CardStandardInfo.cardTag;
                card.GetComponent<Renderer>().material.SetTexture("_Front", card.icon);
                card.Init();
                NewCardScript = card;
            });
            await Task.Run(() => { while (NewCardScript == null) { } });
            return NewCardScript;
        }
        public static async Task BanishCard(Card card)
        {
            MainThread.Run(() => { card.GetComponent<CardControl>().CreatGap(); });
            await Task.Delay(800);
            MainThread.Run(() => { card.GetComponent<CardControl>().FoldGap(); });
            await Task.Delay(800);
            MainThread.Run(() => { card.GetComponent<CardControl>().DestoryGap(); });
            RemoveCard(card);
        }
        //public static async Task DeployCard(Card targetCard, SingleRowInfo selectRegion, int selectLocation)
        //{
        //    //List<Card> OriginRow = RowsInfo.GetCardList(targetCard);
        //    List<Card> TargetRow = selectRegion.ThisRowCards;
        //    //OriginRow.Remove(targetCard);
        //    RemoveCard(targetCard);
        //    TargetRow.Insert(selectLocation, targetCard);
        //    targetCard.MoveSpeed = 0.1f;
        //    targetCard.isMoveStepOver = false;
        //    await Task.Delay(1000);
        //    targetCard.isMoveStepOver = true;
        //    targetCard.MoveSpeed = 0.1f;
        //    EffectCommand.AudioEffectPlay(1);
        //}
        public static async Task DeployCard(Card targetCard)
        {
            List<Card> TargetRow = AgainstInfo.SelectRegion.ThisRowCards;
            RemoveCard(targetCard);
            TargetRow.Insert(AgainstInfo.SelectLocation, targetCard);
            targetCard.moveSpeed = 0.1f;
            targetCard.isMoveStepOver = false;
            await Task.Delay(1000);
            targetCard.isMoveStepOver = true;
            targetCard.moveSpeed = 0.1f;
            EffectCommand.AudioEffectPlay(1);
        }
        public static async Task ExchangeCard(Card targetCard, bool IsPlayerExchange = true, int RandomRank = 0)
        {
            //Debug.Log("交换卡牌");
            await WashCard(targetCard, IsPlayerExchange, RandomRank);
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
            TargetCard.SetCardSeeAble(IsPlayerDraw);
            CardSet TargetCardtemp = AgainstInfo.cardSet[IsPlayerDraw ? Orientation.Down : Orientation.Up][RegionTypes.Deck];

            AgainstInfo.cardSet[IsPlayerDraw ? Orientation.Down : Orientation.Up][RegionTypes.Deck].Remove(TargetCard);
            AgainstInfo.cardSet[IsPlayerDraw ? Orientation.Down : Orientation.Up][RegionTypes.Hand].Add(TargetCard);
            if (isOrder)
            {
                OrderCard();
            }
            await Task.Delay(100);
        }
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
                TargetCard.SetCardSeeAble(false);
            }
            else
            {
                AgainstInfo.cardSet[Orientation.Up][RegionTypes.Hand].Remove(TargetCard);
                AgainstInfo.cardSet[Orientation.Up][RegionTypes.Deck].Add(TargetCard, InsertRank);
            }
            await Task.Delay(500);
        }
        public static async Task PlayCard(Card targetCard, bool IsAnsy = true)
        {
            //Debug.Log("打出一张牌2");
            EffectCommand.AudioEffectPlay(0);
            RowCommand.SetPlayCardMoveFree(false);
            targetCard.isPrepareToPlay = false;
            if (IsAnsy)
            {
                Network.NetCommand.AsyncInfo(NetAcyncType.PlayCard);
            }
            targetCard.SetCardSeeAble(true);
            RemoveCard(targetCard);
            //targetCard.Row.Remove(targetCard);
            AgainstInfo.cardSet[Orientation.My][RegionTypes.Uesd].Add(targetCard);
            AgainstInfo.PlayerPlayCard = null;
            //await targetCard.TriggerAsync<TriggerType.PlayCard>();
        }
        public static async Task DisCard(Card card)
        {
            card.isPrepareToPlay = false;
            card.SetCardSeeAble(false);
            RemoveCard(card);
            AgainstInfo.cardSet[Orientation.My][RegionTypes.Grave].Add(card);
            Info.AgainstInfo.PlayerPlayCard = null;
            //await card.TriggerAsync<TriggerType.Discard>();
        }
        public static async Task Gain(TriggerInfo triggerInfo)
        {
            //Debug.Log("增益弹幕");
            EffectCommand.Bullet_Gain(triggerInfo);
            EffectCommand.AudioEffectPlay(1);
            await Task.Delay(1000);
            triggerInfo.targetCard.changePoint += triggerInfo.point;
            await Task.Delay(1000);
        }
        //public static async Task Cure(TriggerInfo triggerInfo)
        //{
        //    triggerInfo.targetCard.changePoint = Math.Max(0, card.changePoint);
        //    EffectCommand.ParticlePlay(0, card);
        //    EffectCommand.AudioEffectPlay(1);
        //    triggerInfo.targetCard.changePoint = 0;
        //    await Task.Delay(1000);
        //}
        public static async Task Hurt(TriggerInfo triggerInfo)
        {
            Debug.Log("伤害弹幕");
            EffectCommand.Bullet_Hurt(triggerInfo);
            EffectCommand.AudioEffectPlay(1);
            await Task.Delay(1000);
            triggerInfo.targetCard.changePoint -= triggerInfo.point;
            await Task.Delay(1000);
        }

        public static async Task RemoveFromBattle(Card card, int Index = 0)
        {
            Orientation orientation = card.belong == Territory.My ? Orientation.Down : Orientation.Up;
            SingleRowInfo grave = AgainstInfo.cardSet[orientation][RegionTypes.Grave].singleRowInfos[0];
            card.isMoveStepOver = false;
            List<Card> OriginRow = RowsInfo.GetCardList(card);
            List<Card> TargetRow = grave.ThisRowCards;
            OriginRow.Remove(card);
            TargetRow.Insert(Index, card);
            card.SetCardSeeAble(false);
            card.moveSpeed = 0.1f;
            await Task.Delay(100);
            card.isMoveStepOver = true;
            card.moveSpeed = 0.1f;
            Command.EffectCommand.AudioEffectPlay(1);
        }
    }
}