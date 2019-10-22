﻿using CardModel;
using CardSpace;
using Extension;
using GameEnum;
using Info;
using Model;
using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Thread;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Command
{
    public static class StateCommand
    {
        public static async Task BattleStart()
        {
            if (!Info.AgainstInfo.IsPVP)
            {
                //Info.AllPlayerInfo.UserInfo = new NetInfoModel.PlayerInfo("gezi", "yaya", new List<CardDeck> { new CardDeck("gezi", 0, new List<int> { 1031, 1032, 1033, 1034, 1035, 1036, 1037, 1038, 1039, 10310, 10311, 10311, 10311, 10312, 10312, 10312, 10313, 10313, 10313, 10314, 10314, 10314}) });
                //Info.AllPlayerInfo.OpponentInfo = new NetInfoModel.PlayerInfo("gezi", "yaya", new List<CardDeck> { new CardDeck("gezi", 0, new List<int> { 1031, 1032, 1033, 1034, 1035, 1036, 1037, 1038, 1039, 10310, 10311, 10311, 10311, 10312, 10312, 10312, 10313, 10313, 10313, 10314, 10314, 10314})});
                Info.AllPlayerInfo.UserInfo = new NetInfoModel.PlayerInfo("gezi", "yaya", new List<CardDeck> { new CardDeck("gezi", 1001, new List<int> { 1002, 1003, 1004, 1005, 1006, 1007, 1008, 1009, 1010, 1011, 1012, 1013, 1014, 1015 }) });
                Info.AllPlayerInfo.OpponentInfo = new NetInfoModel.PlayerInfo("gezi", "yaya", new List<CardDeck> { new CardDeck("gezi", 1001, new List<int> { 1002, 1003, 1004, 1005, 1006, 1007, 1008, 1009, 1010, 1011, 1012, 1013, 1014, 1015 }) });
            }
            //RowCommand.SetAllRegionSelectable(false);
            RowCommand.SetAllRegionSelectable(RegionTypes.None);
            await Task.Run(async () =>
            {
                //await Task.Delay(500);
                GameUI.UiCommand.SetNoticeBoardTitle("对战开始");
                //GameUI.UiCommand.NoticeBoardShow();
                await Task.Delay(000);
                //初始化领袖卡
                Card MyLeaderCard = await CardCommand.CreatCard(AllPlayerInfo.UserInfo.UseDeck.LeaderId);
                //AgainstInfo.AllCardList.InRogin(Orientation.Down, RegionTypes.Leader).Add(MyLeaderCard);
                CardSet cardSet1 = AgainstInfo.cardSet[Orientation.Down];
                CardSet cardSet = cardSet1[RegionTypes.Leader];
                cardSet.Add(MyLeaderCard);
                MyLeaderCard.IsCanSee = true;
                Card OpLeaderCard = await CardCommand.CreatCard(AllPlayerInfo.OpponentInfo.UseDeck.LeaderId);
                // AgainstInfo.AllCardList.InRogin(Orientation.Up, RegionTypes.Leader).Add(OpLeaderCard);
                AgainstInfo.cardSet[Orientation.Up][RegionTypes.Leader].Add(OpLeaderCard);

                OpLeaderCard.IsCanSee = true;



                CardDeck Deck = AllPlayerInfo.UserInfo.UseDeck;
                for (int i = 0; i < Deck.CardIds.Count; i++)
                {
                    Card NewCard = await CardCommand.CreatCard(Deck.CardIds[i]);
                    //AgainstInfo.AllCardList.InRogin(Orientation.Down, RegionTypes.Deck).Add(NewCard);
                    AgainstInfo.cardSet[Orientation.Down][RegionTypes.Deck].Add(NewCard);

                }
                Deck = AllPlayerInfo.OpponentInfo.UseDeck;
                for (int i = 0; i < Deck.CardIds.Count; i++)
                {
                    Card NewCard = await CardCommand.CreatCard(Deck.CardIds[i]);
                    //AgainstInfo.AllCardList.InRogin(Orientation.Up, RegionTypes.Deck).Add(NewCard);
                    AgainstInfo.cardSet[Orientation.Up][RegionTypes.Deck].Add(NewCard);

                }
                //AgainstInfo.cardSet.Init();
                await Task.Delay(000);
            });
        }
        public static async Task BattleEnd(bool IsSurrender = false, bool IsWin = true)
        {
            //Debug.Log("回合结束");
            await Task.Run(async () =>
            {
                GameUI.UiCommand.SetNoticeBoardTitle($"对战终止\n{AgainstInfo.ShowScore.MyScore}:{AgainstInfo.ShowScore.OpScore}");
                GameUI.UiCommand.NoticeBoardShow();
                await Task.Delay(2000);
                MainThread.Run(() =>
                {
                    SceneManager.LoadSceneAsync(1);
                });
                //Info.GlobalBattleInfo.IsBattleEnd = true;
            });
        }
        public static async Task RoundStart(int num)
        {
            await Task.Run(async () =>
            {
                GameUI.UiCommand.ReSetPassState();
                GameUI.UiCommand.SetNoticeBoardTitle($"第{num + 1}小局开始");
                GameUI.UiCommand.NoticeBoardShow();
                await Task.Delay(2000);
                switch (num)
                {
                    case (0):
                        {
                            Info.AgainstInfo.ExChangeableCardNum += 3;
                            Info.GameUI.UiInfo.CardBoardTitle = "剩余抽卡次数为" + Info.AgainstInfo.ExChangeableCardNum;
                            for (int i = 0; i < 10; i++)
                            {
                                await CardCommand.DrawCard(IsPlayerDraw: true, isOrder: false);
                            }
                            for (int i = 0; i < 10; i++)
                            {
                                await CardCommand.DrawCard(IsPlayerDraw: false,isOrder:false);
                            }
                            CardCommand.OrderCard();
                            break;
                        }
                    case (1):
                        {
                            Info.AgainstInfo.ExChangeableCardNum += 1;
                            Info.GameUI.UiInfo.CardBoardTitle = "剩余抽卡次数为" + Info.AgainstInfo.ExChangeableCardNum;
                            await CardCommand.DrawCard();
                            await CardCommand.DrawCard(false);
                            break;
                        }
                    case (2):
                        {
                            Info.AgainstInfo.ExChangeableCardNum += 1;
                            Info.GameUI.UiInfo.CardBoardTitle = "剩余抽卡次数为" + Info.AgainstInfo.ExChangeableCardNum;
                            await CardCommand.DrawCard();
                            await CardCommand.DrawCard(false);
                            break;
                        }
                    default:
                        break;
                }
                await Task.Delay(2500);
                await WaitForSelectBoardCard(AgainstInfo.cardSet[Orientation.Down] [RegionTypes.Hand].cardList, CardBoardMode.ExchangeCard);
            });
        }
        public static async Task RoundEnd(int num)
        {
            await Task.Run(async () =>
            {
                GameUI.UiCommand.SetNoticeBoardTitle($"第{num + 1}小局结束\n{PointInfo.TotalDownPoint}:{PointInfo.TotalUpPoint}\n{((PointInfo.TotalDownPoint > PointInfo.TotalUpPoint) ? "Win" : "Lose")}");
                GameUI.UiCommand.NoticeBoardShow();
                await Task.Delay(2000);
                int result = 0;
                if (PointInfo.TotalPlayer1Point > PointInfo.TotalPlayer2Point)
                {
                    result = 1;
                }
                else if (PointInfo.TotalPlayer1Point < PointInfo.TotalPlayer2Point)
                {
                    result = 2;
                }
                AgainstInfo.PlayerScore.P1Score += result == 0 || result == 1 ? 1 : 0;
                AgainstInfo.PlayerScore.P2Score += result == 0 || result == 2 ? 1 : 0;
                await Task.Delay(3500);
            });
        }
        public static async Task TurnStart()
        {
            await Task.Run(async () =>
            {
                GameUI.UiCommand.SetNoticeBoardTitle((AgainstInfo.IsMyTurn ? "我方" : "敌方") + "回合开始");
                GameUI.UiCommand.NoticeBoardShow();
                await Task.Delay(000);
                AgainstInfo.IsCardEffectCompleted = false;
                RowCommand.SetPlayCardLimit(!AgainstInfo.IsMyTurn);
                await Task.Delay(000);
            });
        }
        public static async Task TurnEnd()
        {
            await Task.Run(async () =>
            {
                GameUI.UiCommand.SetNoticeBoardTitle((AgainstInfo.IsMyTurn ? "我方" : "敌方") + "回合结束");
                GameUI.UiCommand.NoticeBoardShow();
                await Task.Delay(000);
                RowCommand.SetPlayCardLimit(true);
                await Task.Delay(000);
                AgainstInfo.IsMyTurn = !AgainstInfo.IsMyTurn;
            });
        }
        public static async Task WaitForPlayerOperation()
        {
            await Task.Run(async () =>
            {
                //print("出牌");
                if (!Info.AgainstInfo.IsPVP && !Info.AgainstInfo.IsMyTurn)
                {
                    await AiCommand.TempOperationAsync();
                }
                //当出牌,弃牌,pass时结束
                while (true)
                {
                    if (Info.AgainstInfo.IsCardEffectCompleted)
                    {
                        Info.AgainstInfo.IsCardEffectCompleted = false;
                        break;
                    }
                    if (Info.AgainstInfo.IsCurrectPass)
                    {
                        Info.AgainstInfo.IsCardEffectCompleted = false;
                        break;
                    }
                }
            });
        }
        public static async Task WaitForSelectRegion()
        {
            AgainstInfo.IsWaitForSelectRegion = true;
            Info.AgainstInfo.SelectRegion = null;
            await Task.Run(() =>
            {
                while (Info.AgainstInfo.SelectRegion == null) { }
            });
            Command.Network.NetCommand.AsyncInfo(NetAcyncType.FocusRegion);
            AgainstInfo.IsWaitForSelectRegion = false;
        }
        public static async Task WaitForSelectLocation(Card card)
        {
            AgainstInfo.IsWaitForSelectLocation = true;
            Debug.Log("等待选择部署位置");
            RowCommand.SetAllRegionSelectable((RegionTypes)(card.property + 5), card.territory);
            AgainstInfo.SelectLocation = -1;
            // Debug.Log("开始进入部署位置");
            await Task.Run(() =>
            {
                while (Info.AgainstInfo.SelectLocation < 0) { }
            });
            // Debug.Log("选择部署位置完毕");
            Network.NetCommand.AsyncInfo(NetAcyncType.FocusLocation);
            RowCommand.SetAllRegionSelectable(RegionTypes.None);
            AgainstInfo.IsWaitForSelectLocation = false;
        }
        public static async Task WaitForSelecUnit(Card OriginCard, List<Card> Cards, int num)
        {
            //Debug.Log("选择场上数量" + Cards.Count);
            //Debug.Log("选择场上单位" + Math.Min(Cards.Count, num));
            Info.AgainstInfo.ArrowStartCard = OriginCard;
            AgainstInfo.IsWaitForSelectUnits = true;
            Info.AgainstInfo.AllCardList.ForEach(card => card.IsGray = true);
            Cards.ForEach(card => card.IsGray = false);
            AgainstInfo.SelectUnits.Clear();
            await Task.Run(async () =>
            {
                await Task.Delay(500);
                GameUI.UiCommand.SetArrowShow();
                while (Info.AgainstInfo.SelectUnits.Count < Math.Min(Cards.Count, num)) { }
                Debug.Log("选择单位完毕" + Math.Min(Cards.Count, num));
                Command.Network.NetCommand.AsyncInfo(GameEnum.NetAcyncType.SelectUnites);
                await Task.Delay(250);
                GameUI.UiCommand.SetArrowDestory();
            });
            //Debug.Log("选择单位完毕");
            AgainstInfo.AllCardList.ForEach(card => card.IsGray = false);
            AgainstInfo.IsWaitForSelectUnits = false;

        }
        public static async Task WaitForSelectBoardCard<T>(List<T> CardIds, CardBoardMode Mode = CardBoardMode.Select, int num = 1)
        {
            AgainstInfo.SelectBoardCardIds = new List<int>();
            GameUI.UiCommand.SetCardBoardShow();
            if (typeof(T) == typeof(Card))
            {
                GameUI.CardBoardCommand.LoadCardList(CardIds.Cast<Card>().ToList());
            }
            else
            {
                GameUI.CardBoardCommand.LoadCardList(CardIds.Cast<int>().ToList());
            }
            Debug.Log("进入选择模式");
            await Task.Run(async () =>
            {
                switch (Mode)
                {
                    case CardBoardMode.Select:
                        while (AgainstInfo.SelectBoardCardIds.Count < Mathf.Min(CardIds.Count, num) && !AgainstInfo.IsFinishSelectBoardCard) { }
                        break;
                    case CardBoardMode.ExchangeCard:
                        {
                            while (Info.AgainstInfo.ExChangeableCardNum != 0 && !Info.AgainstInfo.IsSelectCardOver)
                            {
                                if (Info.AgainstInfo.SelectBoardCardIds.Count > 0)
                                {
                                    List<Card> CardLists = CardIds.Cast<Card>().ToList();
                                    await CardCommand.ExchangeCard(CardLists[AgainstInfo.SelectBoardCardIds[0]]);
                                    Info.AgainstInfo.ExChangeableCardNum--;
                                    Info.AgainstInfo.SelectBoardCardIds.Clear();
                                    GameUI.UiCommand.SetCardBoardTitle("剩余抽卡次数为" + Info.AgainstInfo.ExChangeableCardNum);
                                }
                            }
                            Info.AgainstInfo.IsSelectCardOver = false;
                            break;
                        }
                    case CardBoardMode.ShowOnly:
                        break;
                    default:
                        break;
                }
            });
            GameUI.UiCommand.SetCardBoardHide();
        }
        public static void SetPassState(bool IsPlayer1, bool IsActive)
        {
            if (IsPlayer1)
            {
                Debug.Log("我方pass啦" + IsActive);
                Info.GameUI.UiInfo.Instance.MyPass.SetActive(IsActive);
            }
            else
            {
                Debug.Log("敌方pass啦" + IsActive);
                Info.GameUI.UiInfo.Instance.OpPass.SetActive(IsActive);
            }
        }
        public static async Task Surrender()
        {
            Debug.Log("投降");
            await Task.Run(async () =>
            {
                Command.Network.NetCommand.Surrender();
                await BattleEnd(true, false);
            });
        }
    }

}

