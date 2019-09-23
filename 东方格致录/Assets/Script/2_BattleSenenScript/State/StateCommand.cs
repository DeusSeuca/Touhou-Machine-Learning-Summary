﻿using CardSpace;
using GameEnum;
using Info;
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
    public class StateCommand
    {
        public static async Task BattleStart()
        {
            if (!Info.GlobalBattleInfo.IsPVP)
            {
                //Info.AllPlayerInfo.UserInfo = new NetInfoModel.PlayerInfo("gezi", "yaya", new List<CardDeck> { new CardDeck("gezi", 0, new List<int> { 1031, 1032, 1033, 1034, 1035, 1036, 1037, 1038, 1039, 10310, 10311, 10311, 10311, 10312, 10312, 10312, 10313, 10313, 10313, 10314, 10314, 10314}) });
                //Info.AllPlayerInfo.OpponentInfo = new NetInfoModel.PlayerInfo("gezi", "yaya", new List<CardDeck> { new CardDeck("gezi", 0, new List<int> { 1031, 1032, 1033, 1034, 1035, 1036, 1037, 1038, 1039, 10310, 10311, 10311, 10311, 10312, 10312, 10312, 10313, 10313, 10313, 10314, 10314, 10314})});
                Info.AllPlayerInfo.UserInfo = new NetInfoModel.PlayerInfo("gezi", "yaya", new List<CardDeck> { new CardDeck("gezi", 1, new List<int> { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 }) });
                Info.AllPlayerInfo.OpponentInfo = new NetInfoModel.PlayerInfo("gezi", "yaya", new List<CardDeck> { new CardDeck("gezi", 1, new List<int> { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 }) });
            }
            RowCommand.SetAllRegionSelectable(false);
            await Task.Run(async () =>
            {
                //await Task.Delay(500);
                GameUI.UiCommand.SetNoticeBoardTitle("对战开始");
                GameUI.UiCommand.NoticeBoardShow();
                await Task.Delay(000);
                //初始化领袖卡
                Card MyLeaderCard = await CardCommand.CreatCard(Info.AllPlayerInfo.UserInfo.UseDeck.LeaderId);
                RowsInfo.GetDownCardList(RegionTypes.Leader).Add(MyLeaderCard);
                MyLeaderCard.IsCanSee = true;
                Card OpLeaderCard = await CardCommand.CreatCard(Info.AllPlayerInfo.OpponentInfo.UseDeck.LeaderId);
                RowsInfo.GetUpCardList(RegionTypes.Leader).Add(OpLeaderCard);
                OpLeaderCard.IsCanSee = true;



                CardDeck Deck = AllPlayerInfo.UserInfo.UseDeck;
                for (int i = 0; i < Deck.CardIds.Count; i++)
                {
                    Card NewCard = await CardCommand.CreatCard(Deck.CardIds[i]);
                    RowsInfo.GetDownCardList(RegionTypes.Deck).Add(NewCard);
                }
                Deck = AllPlayerInfo.OpponentInfo.UseDeck;
                for (int i = 0; i < Deck.CardIds.Count; i++)
                {
                    Card NewCard = await CardCommand.CreatCard(Deck.CardIds[i]);
                    RowsInfo.GetUpCardList(RegionTypes.Deck).Add(NewCard);
                }
                await Task.Delay(000);
            });
        }
        public static async Task BattleEnd(bool IsSurrender = false, bool IsWin = true)
        {
            //Debug.Log("回合结束");
            await Task.Run(async () =>
            {
                GameUI.UiCommand.SetNoticeBoardTitle($"对战终止\n{GlobalBattleInfo.ShowScore.MyScore}:{GlobalBattleInfo.ShowScore.OpScore}");
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
                            Info.GlobalBattleInfo.ExChangeableCardNum += 3;
                            Info.GameUI.UiInfo.CardBoardTitle = "剩余抽卡次数为" + Info.GlobalBattleInfo.ExChangeableCardNum;
                            for (int i = 0; i < 10; i++)
                            {
                                await CardCommand.DrawCard();
                            }
                            for (int i = 0; i < 10; i++)
                            {
                                await CardCommand.DrawCard(false);
                            }
                            break;
                        }
                    case (1):
                        {
                            Info.GlobalBattleInfo.ExChangeableCardNum += 1;
                            Info.GameUI.UiInfo.CardBoardTitle = "剩余抽卡次数为" + Info.GlobalBattleInfo.ExChangeableCardNum;
                            await CardCommand.DrawCard();
                            await CardCommand.DrawCard(false);
                            break;
                        }
                    case (2):
                        {
                            Info.GlobalBattleInfo.ExChangeableCardNum += 1;
                            Info.GameUI.UiInfo.CardBoardTitle = "剩余抽卡次数为" + Info.GlobalBattleInfo.ExChangeableCardNum;
                            await CardCommand.DrawCard();
                            await CardCommand.DrawCard(false);
                            break;
                        }
                    default:
                        break;
                }
                await Task.Delay(2500);
                await WaitForSelectBoardCard(Info.RowsInfo.GetDownCardList(RegionTypes.Hand), CardBoardMode.ExchangeCard); ;
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
                GlobalBattleInfo.PlayerScore.P1Score += result == 0 || result == 1 ? 1 : 0;
                GlobalBattleInfo.PlayerScore.P2Score += result == 0 || result == 2 ? 1 : 0;
                await Task.Delay(3500);
            });
        }
        public static async Task TurnStart()
        {
            await Task.Run(async () =>
            {
                GameUI.UiCommand.SetNoticeBoardTitle((GlobalBattleInfo.IsMyTurn ? "我方" : "敌方") + "回合开始");
                GameUI.UiCommand.NoticeBoardShow();
                await Task.Delay(000);
                GlobalBattleInfo.IsCardEffectCompleted = false;
                GameCommand.SetPlayCardLimit(!GlobalBattleInfo.IsMyTurn);
                await Task.Delay(000);
            });
        }
        public static async Task TurnEnd()
        {
            await Task.Run(async () =>
            {
                GameUI.UiCommand.SetNoticeBoardTitle((GlobalBattleInfo.IsMyTurn ? "我方" : "敌方") + "回合结束");
                GameUI.UiCommand.NoticeBoardShow();
                await Task.Delay(000);
                GameCommand.SetPlayCardLimit(true);
                await Task.Delay(000);
                GlobalBattleInfo.IsMyTurn = !GlobalBattleInfo.IsMyTurn;
            });
        }
        public static async Task WaitForPlayerOperation()
        {
            await Task.Run(async () =>
            {
                //print("出牌");
                if (!Info.GlobalBattleInfo.IsPVP && !Info.GlobalBattleInfo.IsMyTurn)
                {
                    await AiCommand.TempOperationAsync();
                }
                //当出牌,弃牌,pass时结束
                while (true)
                {
                    if (Info.GlobalBattleInfo.IsCardEffectCompleted)
                    {
                        Info.GlobalBattleInfo.IsCardEffectCompleted = false;
                        break;
                    }
                    if (Info.GlobalBattleInfo.IsCurrectPass)
                    {
                        Info.GlobalBattleInfo.IsCardEffectCompleted = false;
                        break;
                    }
                    //if (Info.GlobalBattleInfo.IsCardEffectCompleted)
                    //{
                    //    Info.GlobalBattleInfo.IsCardEffectCompleted = false;
                    //    break;
                    //}
                }
            });
        }
        public static async Task WaitForSelectRegion()
        {
            GlobalBattleInfo.IsWaitForSelectRegion = true;
            Info.GlobalBattleInfo.SelectRegion = null;
            await Task.Run(() =>
            {
                while (Info.GlobalBattleInfo.SelectRegion == null) { }
            });
            Command.Network.NetCommand.AsyncInfo(NetAcyncType.FocusRegion);
            GlobalBattleInfo.IsWaitForSelectRegion = false;
        }
        public static async Task WaitForSelectLocation()
        {
            GlobalBattleInfo.IsWaitForSelectLocation = true;
            //Debug.Log("开始进入部署位置");
            RowCommand.SetAllRegionSelectable(true);
            GlobalBattleInfo.SelectLocation = -1;
            // Debug.Log("开始进入部署位置");
            await Task.Run(() =>
            {
                while (Info.GlobalBattleInfo.SelectLocation < 0) { }
            });
            // Debug.Log("选择部署位置完毕");
            Network.NetCommand.AsyncInfo(NetAcyncType.FocusLocation);
            RowCommand.SetAllRegionSelectable(false);
            GlobalBattleInfo.IsWaitForSelectLocation = false;
        }
        public static async Task WaitForSelecUnit(Card OriginCard, List<Card> Cards, int num)
        {
            //Debug.Log("选择场上数量" + Cards.Count);
            //Debug.Log("选择场上单位" + Math.Min(Cards.Count, num));
            Info.GlobalBattleInfo.ArrowStartCard = OriginCard;
            GlobalBattleInfo.IsWaitForSelectUnits = true;
            GameCommand.GetCardList(LoadRangeOnBattle.All).ForEach(card => card.IsGray = true);
            Cards.ForEach(card => card.IsGray = false);
            GlobalBattleInfo.SelectUnits.Clear();
            await Task.Run(async () =>
            {
                await Task.Delay(500);
                GameUI.UiCommand.SetArrowShow();
                while (Info.GlobalBattleInfo.SelectUnits.Count < Math.Min(Cards.Count, num)) { }
                Debug.Log("选择单位完毕" + Math.Min(Cards.Count, num));
                Command.Network.NetCommand.AsyncInfo(GameEnum.NetAcyncType.SelectUnites);
                await Task.Delay(250);
                GameUI.UiCommand.SetArrowDestory();
            });
            //Debug.Log("双方数量" + Info.GlobalBattleInfo.SelectUnits.Count + ":" + Math.Min(Cards.Count, num));
            //Debug.Log("选择单位完毕");
            GameCommand.GetCardList(LoadRangeOnBattle.All).ForEach(card => card.IsGray = false);
            GlobalBattleInfo.IsWaitForSelectUnits = false;

        }
        public static async Task WaitForSelectBoardCard<T>(List<T> CardIds, CardBoardMode Mode = CardBoardMode.Select, int num = 1)
        {
            GlobalBattleInfo.SelectBoardCardIds = new List<int>();
            //GlobalBattleInfo.IsWaitForSelectBoardCard = true;
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
                        while (GlobalBattleInfo.SelectBoardCardIds.Count < Mathf.Min(CardIds.Count, num) && !GlobalBattleInfo.IsFinishSelectBoardCard) { }
                        break;
                    case CardBoardMode.ExchangeCard:
                        {
                            while (Info.GlobalBattleInfo.ExChangeableCardNum != 0 && !Info.GlobalBattleInfo.IsSelectCardOver)
                            {
                                if (Info.GlobalBattleInfo.SelectBoardCardIds.Count > 0)
                                {
                                    List<Card> CardLists = CardIds.Cast<Card>().ToList();
                                    await CardCommand.ExchangeCard(CardLists[GlobalBattleInfo.SelectBoardCardIds[0]]);
                                    Info.GlobalBattleInfo.ExChangeableCardNum--;
                                    Info.GlobalBattleInfo.SelectBoardCardIds.Clear();
                                    GameUI.UiCommand.SetCardBoardTitle("剩余抽卡次数为" + Info.GlobalBattleInfo.ExChangeableCardNum);
                                }
                            }
                            Info.GlobalBattleInfo.IsSelectCardOver = false;
                            break;
                        }
                    case CardBoardMode.ShowOnly:
                        break;
                    default:
                        break;
                }
            });
            GameUI.UiCommand.SetCardBoardHide();
            //GlobalBattleInfo.IsWaitForSelectBoardCard = false;
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

