﻿using CardModel;
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
        public static async Task SummonCard(Card targetCard)
        {
            //await Task.Delay(1000);
            RemoveCard(targetCard);
            List<Card> TargetRow = AgainstInfo
                .cardSet[(RegionTypes)targetCard.region][(Orientation)targetCard.territory]
                .singleRowInfos.First().ThisRowCards;
            TargetRow.Add(targetCard);
            targetCard.isCanSee = true;
            //targetCard.moveSpeed = 0.1f;
            targetCard.isMoveStepOver = false;
            await Task.Delay(1000);
            targetCard.isMoveStepOver = true;
            //targetCard.moveSpeed = 0.1f;
            EffectCommand.AudioEffectPlay(1);
        }

        public static async Task DeployCard(Card targetCard)
        {
            List<Card> TargetRow = AgainstInfo.SelectRegion.ThisRowCards;
            RemoveCard(targetCard);
            TargetRow.Insert(AgainstInfo.SelectLocation, targetCard);
            //targetCard.moveSpeed = 0.1f;
            targetCard.isMoveStepOver = false;
            await Task.Delay(1000);
            targetCard.isMoveStepOver = true;
            //targetCard.moveSpeed = 0.1f;
            EffectCommand.AudioEffectPlay(1);
        }
        public static async Task ExchangeCard(Card targetCard, bool IsPlayerExchange = true, int RandomRank = 0)
        {
            //Debug.Log("交换卡牌");
            await WashCard(targetCard, IsPlayerExchange, RandomRank);
            await DrawCard(IsPlayerExchange, true);
            if (IsPlayerExchange)
            {
                GameUI.CardBoardCommand.LoadCardList(AgainstInfo.cardSet[Orientation.My][RegionTypes.Hand].CardList);
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
            Card TargetCard = AgainstInfo.cardSet[IsPlayerDraw ? Orientation.Down : Orientation.Up][RegionTypes.Deck].CardList[0];
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
                int MaxCardRank = AgainstInfo.cardSet[Orientation.Down][RegionTypes.Deck].CardList.Count;
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
            EffectCommand.AudioEffectPlay(0);
            RowCommand.SetPlayCardMoveFree(false);
            targetCard.isPrepareToPlay = false;
            if (IsAnsy)
            {
                Network.NetCommand.AsyncInfo(NetAcyncType.PlayCard);
            }
            targetCard.SetCardSeeAble(true);
            RemoveCard(targetCard);
            AgainstInfo.cardSet[Orientation.My][RegionTypes.Uesd].Add(targetCard);
            AgainstInfo.PlayerPlayCard = null;
        }
        public static async Task DisCard(Card card)
        {
            card.isPrepareToPlay = false;
            card.SetCardSeeAble(false);
            RemoveCard(card);
            AgainstInfo.cardSet[Orientation.My][RegionTypes.Grave].Add(card);
            AgainstInfo.PlayerPlayCard = null;
        }

        public static async Task SealCard(Card card)
        {
            card.cardStates[CardState.Seal] = true;
        }

        public static async Task Gain(TriggerInfo triggerInfo)
        {
            EffectCommand.Bullet_Gain(triggerInfo);
            EffectCommand.AudioEffectPlay(1);
            await Task.Delay(1000);
            triggerInfo.targetCard.changePoint += triggerInfo.point;
            await Task.Delay(1000);
        }
        public static async Task Hurt(TriggerInfo triggerInfo)
        {
            EffectCommand.Bullet_Hurt(triggerInfo);
            EffectCommand.AudioEffectPlay(1);
            await Task.Delay(1000);
            triggerInfo.targetCard.changePoint -= triggerInfo.point;
            await Task.Delay(1000);
        }
        public static async Task RemoveFromBattle(Card card, int Index = 0)
        {
            Orientation orientation = card.belong == Territory.My ? Orientation.Down : Orientation.Up;
            RemoveCard(card);
            AgainstInfo.cardSet[orientation][RegionTypes.Grave].singleRowInfos[0].ThisRowCards.Insert(Index, card);
            card.SetCardSeeAble(false);
            card.changePoint = 0;
            card.isMoveStepOver = false;
            await Task.Delay(100);
            card.isMoveStepOver = true;
            EffectCommand.AudioEffectPlay(1);
        }
    }
}