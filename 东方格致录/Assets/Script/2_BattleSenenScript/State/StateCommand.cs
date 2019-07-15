using CardSpace;
using Control;
using Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Command
{
    public class StateCommand
    {
        public static async Task BattleStart()
        {
            //Info.GlobalBattleInfo.IsPVP = false;
            if (!Info.GlobalBattleInfo.IsPVP)
            {
                Info.AllPlayerInfo.UserInfo = new NetInfoModel.PlayerInfo("gezi", "yaya", new List<CardDeck> { new CardDeck("gezi", 0, new List<int> { 1000, 1001, 1002, 1001, 1000, 1002, 1000, 1001, 1000, 1001, 1001, 1000, 1002 }) });
                Info.AllPlayerInfo.OpponentInfo = new NetInfoModel.PlayerInfo("gezi", "yaya", new List<CardDeck> { new CardDeck("gezi", 0, new List<int> { 1001, 1001, 1000, 1000, 1001, 1001, 1000, 1000, 1001, 1001, 1001, 1000, 1002 }) });
            }
            RowCommand.SetRegionSelectable(false);
            await Task.Run(async () =>
            {
                Debug.Log("1");
                //await Task.Delay(500);
                UiCommand.SetNoticeBoardTitle("对战开始");
                UiCommand.NoticeBoardShow();
                await Task.Delay(2000);
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
                await Task.Delay(2000);
            });
        }
        public static async Task BattleEnd(bool IsSurrender=false,bool IsWin=true)
        {
            Debug.Log("回合结束");
            await Task.Run(async () =>
            {
                UiCommand.SetNoticeBoardTitle($"对战终止\n{GlobalBattleInfo.ShowScore.MyScore}:{GlobalBattleInfo.ShowScore.OpScore}");
                UiCommand.NoticeBoardShow();
                await Task.Delay(2000);
                //UiCommand.NoticeBoardHide();
                await Task.Delay(500);
                Info.GlobalBattleInfo.IsBattleEnd = true;
            });
        }
        public static async Task TurnStart()
        {
            await Task.Run(async () =>
            {
                UiCommand.SetNoticeBoardTitle((GlobalBattleInfo.IsMyTurn ? "我方" : "敌方") + "回合开始");
                UiCommand.NoticeBoardShow();
                await Task.Delay(000);
                //UiCommand.NoticeBoardHide();
                GlobalBattleInfo.IsCardEffectCompleted = false;
                GameCommand.PlayCardLimit(!GlobalBattleInfo.IsMyTurn);
                await Task.Delay(000);
            });
        }
        public static async Task TurnEnd()
        {
            await Task.Run(async () =>
            {
                UiCommand.SetNoticeBoardTitle((GlobalBattleInfo.IsMyTurn ? "我方" : "敌方") + "回合结束");
                UiCommand.NoticeBoardShow();
                await Task.Delay(000);
                //UiCommand.NoticeBoardHide();
                GameCommand.PlayCardLimit(true);
                await Task.Delay(000);
                GlobalBattleInfo.IsMyTurn = !GlobalBattleInfo.IsMyTurn;
            });
        }
        public static async Task RoundStart(int num)
        {
            await Task.Run(async () =>
            {
                GlobalBattleInfo.IsPlayer1Pass = false;
                GlobalBattleInfo.IsPlayer2Pass = false;
                UiCommand.ReSetPassState();
                UiCommand.SetNoticeBoardTitle($"第{num + 1}小局开始");
                UiCommand.NoticeBoardShow();
                await Task.Delay(2000);
                switch (num)
                {
                    case (0):
                        {
                            Info.GlobalBattleInfo.ExChangeableCardNum += 0;
                            Info.UiInfo.CardBoardTitle = "剩余抽卡次数为" + Info.GlobalBattleInfo.ExChangeableCardNum;
                            for (int i = 0; i < 5; i++)
                            {
                                await CardCommand.DrawCard();
                            }
                            for (int i = 0; i < 5; i++)
                            {
                                await CardCommand.DrawCard(false);
                            }
                            break;
                        }
                    case (1):
                        {
                            Info.GlobalBattleInfo.ExChangeableCardNum += 1;
                            Info.UiInfo.CardBoardTitle = "剩余抽卡次数为" + Info.GlobalBattleInfo.ExChangeableCardNum;
                            await CardCommand.DrawCard();
                            await CardCommand.DrawCard(false);
                            break;
                        }
                    case (2):
                        {
                            Info.GlobalBattleInfo.ExChangeableCardNum += 1;
                            Info.UiInfo.CardBoardTitle = "剩余抽卡次数为" + Info.GlobalBattleInfo.ExChangeableCardNum;
                            await CardCommand.DrawCard();
                            await CardCommand.DrawCard(false);
                            break;
                        }
                    default:
                        break;
                }
                await Task.Delay(2500);
                await WaitForSelectBoardCard(Info.RowsInfo.GetDownCardList(RegionTypes.Hand), GameEnum.CardBoardMode.ChangeCard); ;

                //while (Info.GlobalBattleInfo.ChangeableCardNum != 0 && !Info.GlobalBattleInfo.IsSelectCardOver)
                //{
                //    await WaitForSelectBoardCard(Info.RowsInfo.GetDownCardList(RegionTypes.Hand), GameEnum.CardBoardMode.ChangeCard); ;
                //    Debug.Log("选择了卡牌" + Info.GlobalBattleInfo.SelectBoardCardIds[0]);
                //    Debug.Log("抽卡次数为" + Info.GlobalBattleInfo.ChangeableCardNum);
                //}
            });
        }
        public static async Task RoundEnd(int num)
        {
            await Task.Run(async () =>
            {
                UiCommand.SetNoticeBoardTitle($"第{num + 1}小局结束\n{PointInfo.TotalDownPoint}:{PointInfo.TotalUpPoint}\n{((PointInfo.TotalDownPoint > PointInfo.TotalUpPoint) ? "Win" : "Lose")}");
                UiCommand.NoticeBoardShow();
                await Task.Delay(2000);
                //UiCommand.NoticeBoardHide();
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
                        break;
                    }
                    if (Info.GlobalBattleInfo.IsCurrectPass)
                    {
                        Info.GlobalBattleInfo.IsCardEffectCompleted = false;
                        break;
                    }
                    if (Info.GlobalBattleInfo.IsCardEffectCompleted)
                    {
                        Info.GlobalBattleInfo.IsCardEffectCompleted = false;
                        break;
                    }
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
            GlobalBattleInfo.IsWaitForSelectRegion = false;
        }
        public static async Task WaitForSelectLocation()
        {
            GlobalBattleInfo.IsWaitForSelectLocation = true;
            RowCommand.SetRegionSelectable(true);
            GlobalBattleInfo.SelectLocation = -1;
            await Task.Run(() =>
            {
                while (Info.GlobalBattleInfo.SelectLocation < 0) { }
            });
            //Debug.Log("选择完毕");
            RowCommand.SetRegionSelectable(false);
            GlobalBattleInfo.IsWaitForSelectLocation = false;
        }
        public static async Task WaitForSelecUnit(Card OriginCard, List<Card> Cards, int num)
        {
            //Debug.Log("选择场上数量" + Cards.Count);
            //Debug.Log("选择场上单位" + Math.Min(Cards.Count, num));
            Info.GlobalBattleInfo.ArrowStartCard = OriginCard;
            GlobalBattleInfo.IsWaitForSelectUnits = true;
            GameCommand.GetCardList(GameEnum.LoadRangeOnBattle.All).ForEach(card => card.IsActive = true);
            Cards.ForEach(card => card.IsActive = false);
            GlobalBattleInfo.SelectUnits.Clear();
            await Task.Run(async () =>
            {
                await Task.Delay(500);
                Command.UiCommand.SetArrowShow();
                Debug.Log("选择单位");
                while (Info.GlobalBattleInfo.SelectUnits.Count < Math.Min(Cards.Count, num)) { }
                await Task.Delay(250);
                Command.UiCommand.SetArrowDestory();
            });
            //Debug.Log("双方数量" + Info.GlobalBattleInfo.SelectUnits.Count + ":" + Math.Min(Cards.Count, num));
            //Debug.Log("选择单位完毕");
            GameCommand.GetCardList(GameEnum.LoadRangeOnBattle.All).ForEach(card => card.IsActive = false);
            GlobalBattleInfo.IsWaitForSelectUnits = false;
        }
        public static async Task WaitForSelectBoardCard<T>(List<T> CardIds, GameEnum.CardBoardMode Mode = GameEnum.CardBoardMode.Select, int num = 1)
        {

            GlobalBattleInfo.SelectBoardCardIds = new List<int>();
            GlobalBattleInfo.IsWaitForSelectBoardCard = true;
            UiCommand.SetCardBoardShow();
            if (typeof(T) == typeof(Card))
            {
                CardBoardCommand.LoadCardList(CardIds.Cast<Card>().ToList());
            }
            else
            {
                CardBoardCommand.LoadCardList(CardIds.Cast<int>().ToList());
            }
            await Task.Run(async () =>
            {
                switch (Mode)
                {
                    case GameEnum.CardBoardMode.Select:
                        while (GlobalBattleInfo.SelectBoardCardIds.Count < Mathf.Min(CardIds.Count, num) && !GlobalBattleInfo.IsFinishSelectBoardCard) { }
                        break;
                    case GameEnum.CardBoardMode.ChangeCard:
                        while (Info.GlobalBattleInfo.ExChangeableCardNum != 0 && !Info.GlobalBattleInfo.IsSelectCardOver)
                        {
                            if (Info.GlobalBattleInfo.SelectBoardCardIds.Count > 0)
                            {
                                await CardCommand.ExchangeCard();
                                Info.GlobalBattleInfo.ExChangeableCardNum--;
                                Info.GlobalBattleInfo.SelectBoardCardIds.Clear();
                                UiCommand.SetCardBoardTitle("剩余抽卡次数为" + Info.GlobalBattleInfo.ExChangeableCardNum);
                            }
                        }
                        break;
                    case GameEnum.CardBoardMode.ShowOnly:
                        break;
                    default:
                        break;
                }
            });
            //Debug.Log("关闭");
            UiCommand.SetCardBoardHide();
            GlobalBattleInfo.IsWaitForSelectBoardCard = false;
        }
        public static void SetPassState(bool IsPlayer1, bool IsActive)
        {
            if (IsPlayer1)
            {
                Debug.Log("我方pass啦" + IsActive);
                Info.UiInfo.Instance.MyPass.SetActive(IsActive);
            }
            else
            {
                Debug.Log("敌方pass啦" + IsActive);
                Info.UiInfo.Instance.OpPass.SetActive(IsActive);
            }
        }
        public static async Task Surrender()
        {
            await Task.Run(async () =>
            {
                NetCommand.Surrender();
                await BattleEnd(true,false);
            });
        }
    }

}

