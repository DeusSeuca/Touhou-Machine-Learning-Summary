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

            AgainstInfo.isMyTurn = AgainstInfo.isPlayer1;
            AgainstInfo.cardSet = new CardSet();
            Info.StateInfo.TaskManager = new System.Threading.CancellationTokenSource();
            MainThread.Run(() =>
            {
                foreach (var item in GameObject.FindGameObjectsWithTag("SingleInfo"))
                {
                    SingleRowInfo singleRowInfo = item.GetComponent<SingleRowInfo>();
                    AgainstInfo.cardSet.singleRowInfos.Add(singleRowInfo);
                }
            });
            await Task.Delay(500);
            if (AgainstInfo.isPVE)
            {
                AllPlayerInfo.UserInfo = new NetInfoModel.PlayerInfo(
                    "gezi", "yaya",
                    new List<CardDeck>
                    {
                        new CardDeck("gezi", 10001, new List<int>
                        {
                            //10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,
                            10002, 10003, 10004, 10005, 10002, 10012, 10012, 10012, 10010, 10011, 10012, 10013, 10014, 10015, 10016, 10012, 10013, 10014, 10015, 10016, 10012, 10013, 10014, 10015, 10016
                            //10002, 10003, 10004, 10005, 10006, 10007, 10008, 10009, 10010, 10011, 10012, 10013, 10014, 10015, 10016, 10012, 10013, 10014, 10015, 10016, 10012, 10013, 10014, 10015, 10016
                            //1004, 1004, 1004, 1004, 1004, 1004, 1004, 1004, 1004, 1004, 1004, 1004, 1004, 1004, 1004, 1004, 1004, 1004, 1004, 1004, 1004, 1004, 1004, 1004, 1004

                        })
                    });
                AllPlayerInfo.OpponentInfo = new NetInfoModel.PlayerInfo(
                    "gezi", "yaya",
                    new List<CardDeck>
                    {
                        new CardDeck("gezi", 10001, new List<int>
                        {
                            //10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,
                             10002, 10003, 10004, 10005, 10006, 10007, 10008, 10009, 10010, 10011, 10012, 10013, 10014, 10015, 10016, 10012, 10013, 10014, 10015, 10016, 10012, 10013, 10014, 10015, 10016
                            //10002, 10003, 10004, 10005, 10006, 10007, 10008, 10009, 10010, 10011, 10012, 10013, 10014, 10015, 10016, 10012, 10013, 10014, 10015, 10016, 10012, 10013, 10014, 10015, 10016
                        })
                    });
            }
            RowCommand.SetAllRegionSelectable(RegionTypes.None);
            await Task.Run(async () =>
            {
                //await Task.Delay(500);
                Debug.Log("对战开始".TransUiText());
                GameUI.UiCommand.SetNoticeBoardTitle("对战开始".TransUiText());
                await GameUI.UiCommand.NoticeBoardShow();
                //初始化我方领袖卡
                Card MyLeaderCard = await CardCommand.CreatCard(AllPlayerInfo.UserInfo.UseDeck.LeaderId);
                AgainstInfo.cardSet[Orientation.Down][RegionTypes.Leader].Add(MyLeaderCard);
                MyLeaderCard.SetCardSeeAble(true);
                //初始化敌方领袖卡
                Card OpLeaderCard = await CardCommand.CreatCard(AllPlayerInfo.OpponentInfo.UseDeck.LeaderId);
                AgainstInfo.cardSet[Orientation.Up][RegionTypes.Leader].Add(OpLeaderCard);
                OpLeaderCard.SetCardSeeAble(true);
                //初始双方化牌组
                CardDeck Deck = AllPlayerInfo.UserInfo.UseDeck;
                for (int i = 0; i < Deck.CardIds.Count; i++)
                {
                    Card NewCard = await CardCommand.CreatCard(Deck.CardIds[i]);
                    AgainstInfo.cardSet[Orientation.Down][RegionTypes.Deck].Add(NewCard);
                }
                Deck = AllPlayerInfo.OpponentInfo.UseDeck;

                for (int i = 0; i < Deck.CardIds.Count; i++)
                {
                    Card NewCard = await CardCommand.CreatCard(Deck.CardIds[i]);
                    AgainstInfo.cardSet[Orientation.Up][RegionTypes.Deck].Add(NewCard);
                }
                await Task.Delay(000);
            });
        }
        public static async Task BattleEnd(bool IsSurrender = false, bool IsWin = true)
        {
            //Debug.Log("回合结束");
            await Task.Run(async () =>
            {
                GameUI.UiCommand.SetNoticeBoardTitle($"对战终止\n{AgainstInfo.ShowScore.MyScore}:{AgainstInfo.ShowScore.OpScore}");
                await GameUI.UiCommand.NoticeBoardShow();
                await Task.Delay(2000);
                MainThread.Run(() =>
                {
                    Debug.Log("释放线程资源");
                    StateInfo.TaskManager.Cancel();
                    SceneManager.LoadSceneAsync(1);
                });
            });
        }
        public static async Task RoundStart(int num)
        {
            await Task.Run(async () =>
            {
                GameUI.UiCommand.ReSetPassState();
                GameUI.UiCommand.SetNoticeBoardTitle($"第{num + 1}小局开始");
                await GameUI.UiCommand.NoticeBoardShow();
                //await Task.Delay(2000);
                switch (num)
                {
                    case (0):
                        {
                            Info.AgainstInfo.ExChangeableCardNum = 0;
                            Info.GameUI.UiInfo.CardBoardTitle = "剩余抽卡次数为".TransUiText() + Info.AgainstInfo.ExChangeableCardNum;
                            for (int i = 0; i < 10; i++)
                            {
                                await CardCommand.DrawCard(IsPlayerDraw: true, isOrder: false);
                            }
                            for (int i = 0; i < 10; i++)
                            {
                                await CardCommand.DrawCard(IsPlayerDraw: false, isOrder: false);
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
                await WaitForSelectBoardCard(AgainstInfo.cardSet[Orientation.Down][RegionTypes.Hand].cardList, CardBoardMode.ExchangeCard);
            });
        }
        public static async Task RoundEnd(int num)
        {
            await Task.Run(async () =>
            {
                GameUI.UiCommand.SetNoticeBoardTitle($"第{num + 1}小局结束\n{PointInfo.TotalDownPoint}:{PointInfo.TotalUpPoint}\n{((PointInfo.TotalDownPoint > PointInfo.TotalUpPoint) ? "Win" : "Lose")}");
                await GameUI.UiCommand.NoticeBoardShow();
                //await Task.Delay(2000);
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
                await GameSystem.TurnSystem.WhenRoundEnd();
            });
        }
        public static async Task TurnStart()
        {
            await Task.Run(async () =>
            {
                GameUI.UiCommand.SetNoticeBoardTitle(AgainstInfo.isMyTurn ? "我方回合开始".TransUiText() : "对方回合开始".TransUiText());
                await GameUI.UiCommand.NoticeBoardShow();
                //await Task.Delay(000);
                AgainstInfo.IsCardEffectCompleted = false;
                RowCommand.SetPlayCardMoveFree(AgainstInfo.isMyTurn);
                await Task.Delay(000);
            });
        }
        public static async Task TurnEnd()
        {
            await Task.Run(async () =>
            {
                GameUI.UiCommand.SetNoticeBoardTitle(AgainstInfo.isMyTurn ? "我方回合结束".TransUiText() : "对方回合结束".TransUiText());
                await GameUI.UiCommand.NoticeBoardShow();
                //await Task.Delay(000);
                RowCommand.SetPlayCardMoveFree(false);
                await Task.Delay(1000);
                AgainstInfo.isMyTurn = !AgainstInfo.isMyTurn;
            });
        }
        public static async Task WaitForPlayerOperation()
        {
            bool isFirstOperation = true;
            //Timer.SetIsTimerStart(60);
            Timer.SetIsTimerStart(6000);
            await Task.Run(async () =>
            {
                //Debug.Log("出牌");
                //当出牌,弃牌,pass时结束
                while (true)
                {
                    StateInfo.TaskManager.Token.ThrowIfCancellationRequested();
                    if (Timer.isTimeout)
                    {
                        Debug.Log("超时");
                    }
                    if (AgainstInfo.isAIControl && isFirstOperation)
                    {
                        isFirstOperation = false;
                        Debug.Log("自动出牌");
                        await AiCommand.TempOperationPlayCard();
                    }
                    if (AgainstInfo.IsCardEffectCompleted)
                    {
                        AgainstInfo.IsCardEffectCompleted = false;
                        break;
                    }
                    if (AgainstInfo.isCurrectPass)
                    {
                        AgainstInfo.IsCardEffectCompleted = false;
                        break;
                    }
                    await Task.Delay(1000);
                }
            });
            Timer.SetIsTimerClose();
        }
        public static async Task WaitForSelectProperty()
        {
            //放大硬币
            CoinControl.ScaleUp();
            await Task.Delay(1000);
            CoinControl.Unfold();
            AgainstInfo.IsWaitForSelectProperty = true;
            AgainstInfo.SelectProperty = Region.None;
            //暂时设为1秒，之后还原成10秒
            Timer.SetIsTimerStart(1);
            //AgainstInfo.SelectRegion = null;
            await Task.Run(async () =>
            {
                // Debug.Log("等待选择属性");
                while (AgainstInfo.SelectProperty == Region.None)
                {
                    StateInfo.TaskManager.Token.ThrowIfCancellationRequested();
                    if (AgainstInfo.isAIControl)
                    {
                        //Debug.Log("自动选择属性");
                        int rowRank = AiCommand.GetRandom(0, 4);
                        CoinControl.ChangeProperty((Region)rowRank);
                    }
                    await Task.Delay(1000);
                }
            });
            Command.Network.NetCommand.AsyncInfo(NetAcyncType.SelectProperty);
            Timer.SetIsTimerClose();
            AgainstInfo.IsWaitForSelectProperty = false;
            await Task.Delay(1000);
            CoinControl.ScaleDown();
        }
        public static async Task WaitForSelectRegion()
        {
            AgainstInfo.IsWaitForSelectRegion = true;
            Info.AgainstInfo.SelectRegion = null;
            await Task.Run(() =>
            {
                while (Info.AgainstInfo.SelectRegion == null)
                {
                    StateInfo.TaskManager.Token.ThrowIfCancellationRequested();
                }
            });
            Command.Network.NetCommand.AsyncInfo(NetAcyncType.SelectRegion);
            AgainstInfo.IsWaitForSelectRegion = false;
        }
        public static async Task WaitForSelectLocation(Card card)
        {
            AgainstInfo.IsWaitForSelectLocation = true;
            //Debug.Log("等待选择部署位置");
            RowCommand.SetAllRegionSelectable((RegionTypes)(card.region + 5), card.territory);
            AgainstInfo.SelectLocation = -1;
            // Debug.Log("开始进入部署位置");
            await Task.Run(async () =>
            {
                while (AgainstInfo.SelectLocation < 0)
                {
                    StateInfo.TaskManager.Token.ThrowIfCancellationRequested();
                    if (AgainstInfo.isAIControl)
                    {
                        await Task.Delay(1000);
                        List<SingleRowInfo> rows = AgainstInfo.cardSet.singleRowInfos.Where(row => row.CanBeSelected).ToList();
                        int rowRank = AiCommand.GetRandom(0, rows.Count());
                        AgainstInfo.SelectRegion = rows[rowRank];//设置部署区域
                        AgainstInfo.SelectLocation = 0;//设置部署次序
                    }
                }
            });
            // Debug.Log("选择部署位置完毕");
            Network.NetCommand.AsyncInfo(NetAcyncType.SelectLocation);
            RowCommand.SetAllRegionSelectable(RegionTypes.None);
            AgainstInfo.IsWaitForSelectLocation = false;
        }
        public static async Task WaitForSelecUnit(Card OriginCard, List<Card> Cards, int num,bool isAuto)
        {
            //可选列表中移除自身
            Cards.Remove(OriginCard);
            AgainstInfo.ArrowStartCard = OriginCard;
            AgainstInfo.IsWaitForSelectUnits = true;
            AgainstInfo.AllCardList.ForEach(card => card.isGray = true);
            Cards.ForEach(card => card.isGray = false);
            AgainstInfo.SelectUnits.Clear();
            await Task.Run(async () =>
            {
                //await Task.Delay(500);
                if (Info.AgainstInfo.isMyTurn&& !isAuto)
                {
                    GameUI.UiCommand.CreatFreeArrow();
                }
                int selectableNum = Math.Min(Cards.Count, num);
                while (AgainstInfo.SelectUnits.Count < selectableNum)
                {
                    StateInfo.TaskManager.Token.ThrowIfCancellationRequested();
                    if (AgainstInfo.isAIControl||isAuto)
                    {
                        //自动选择场上单位
                        Cards = Cards.OrderBy(x => AiCommand.GetRandom(0, 514)).ToList();
                        Info.AgainstInfo.SelectUnits = Cards.Take(selectableNum).ToList();
                    }
                    await Task.Delay(100);
                }
                Debug.Log("选择单位完毕" + Math.Min(Cards.Count, num));
                Network.NetCommand.AsyncInfo(NetAcyncType.SelectUnites);
                GameUI.UiCommand.DestoryAllArrow();
                await Task.Delay(250);
            });
            AgainstInfo.AllCardList.ForEach(card => card.isGray = false);
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
            //Debug.Log("进入选择模式");
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
        public static async Task Surrender()
        {
            Debug.Log("投降");
            await Task.Run(async () =>
            {
                Command.Network.NetCommand.AsyncInfo(NetAcyncType.Surrender);
                await BattleEnd(true, false);
            });
        }
    }
}

