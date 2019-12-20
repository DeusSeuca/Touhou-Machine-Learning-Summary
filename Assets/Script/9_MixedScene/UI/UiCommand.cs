﻿using CardModel;
using GameEnum;
using GameUI;
using System.Threading.Tasks;
using Thread;
using UnityEngine;
using UnityEngine.UI;

namespace Command
{
    namespace GameUI
    {
        public class UiCommand : MonoBehaviour
        {
            static GameObject MyPass => Info.GameUI.UiInfo.Instance.DownPass;
            static GameObject OpPass => Info.GameUI.UiInfo.Instance.UpPass;
            public static void SetCardBoardShow()
            {
                MainThread.Run(() =>
                {
                    Info.GameUI.UiInfo.CardBoard.SetActive(true);
                    Info.GameUI.UiInfo.CardBoard.transform.GetChild(1).GetComponent<Text>().text = Info.GameUI.UiInfo.CardBoardTitle;
                });
            }
            public static void SetCardBoardHide()
            {
                MainThread.Run(() =>
                {
                    Info.GameUI.UiInfo.CardBoard.SetActive(false);
                });
            }
            public static void CardBoardReload()
            {
                Command.GameUI.CardBoardCommand.CreatBoardCardActual();
            }
            public static Sprite GetBoardCardImage(int Id)
            {
                if (!Info.GameUI.UiInfo.CardImage.ContainsKey(Id))
                {
                    var CardStandardInfo = Command.CardInspector.CardLibraryCommand.GetCardStandardInfo(Id);
                    Texture2D texture = CardStandardInfo.icon;
                    Info.GameUI.UiInfo.CardImage.Add(Id, Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero));
                }
                return Info.GameUI.UiInfo.CardImage[Id];
            }
            public static void ChangeIntroduction(Card card)
            {
                string Title = card.CardName;
                string Text = card.CardIntroduction;
                string Effect = "";
                int Heigh = Text.Length / 13 * 15 + 100;
                Info.GameUI.UiInfo.IntroductionTextBackground.sizeDelta = new Vector2(300, Heigh);
                //修改文本为富文本
                Info.GameUI.UiInfo.IntroductionTitle.text = Title;
                Info.GameUI.UiInfo.IntroductionText.text = Text;
                Info.GameUI.UiInfo.IntroductionEffect.text = Effect;
            }
            public static void SetCardBoardTitle(string Title) => Info.GameUI.UiInfo.CardBoardTitle = Title;
            public static void SetNoticeBoardTitle(string Title) => Info.GameUI.UiInfo.NoticeBoardTitle = Title;
            public static void SetArrowShow()
            {
                MainThread.Run(() =>
                {
                    bool IsFirst = Info.AgainstInfo.ArrowList.Count == 0;
                    GameObject NewArrow = Instantiate(Info.GameUI.UiInfo.Arrow);
                    NewArrow.GetComponent<ArrowManager>().RefreshArrow(
                        Info.AgainstInfo.ArrowStartCard.transform,
                        IsFirst ? Info.GameUI.UiInfo.ArrowEndPoint.transform :
                        Info.AgainstInfo.PlayerFocusCard.transform
                        );
                    Info.AgainstInfo.ArrowList.Add(NewArrow);
                });
            }
            public static void SetArrowDestory()
            {
                MainThread.Run(() =>
                {
                    Info.AgainstInfo.ArrowList.ForEach(Destroy);
                    Info.AgainstInfo.ArrowList.Clear();
                });
            }
            public static async Task NoticeBoardShow()
            {
                MainThread.Run(() =>
                {
                    Info.GameUI.UiInfo.NoticeBoard.transform.GetChild(0).GetComponent<Text>().text = Info.GameUI.UiInfo.NoticeBoardTitle;
                });
                Info.GameUI.UiInfo.isNoticeBoardShow = true;
                await Task.Delay(1000);
                Info.GameUI.UiInfo.isNoticeBoardShow = false;
            }
            public void CardBoardClose() => Info.AgainstInfo.IsSelectCardOver = true;
            public static void SetCardBoardMode(CardBoardMode CardBoardMode) => Info.AgainstInfo.CardBoardMode = CardBoardMode;
            public static void SetCurrentPass()
            {
                MainThread.Run(() =>
                {
                    Info.GameUI.UiInfo.MyPass.SetActive(true);
                    switch (Info.AgainstInfo.IsMyTurn)
                    {
                        case true: Info.AgainstInfo.isDownPass = true; break;
                        case false: Info.AgainstInfo.isUpPass = true; break;
                    }
                });
            }
            public static void ReSetPassState()
            {
                MainThread.Run(() =>
                {
                    Info.GameUI.UiInfo.MyPass.SetActive(false);
                    Info.GameUI.UiInfo.OpPass.SetActive(false);
                    Info.AgainstInfo.isUpPass = false;
                    Info.AgainstInfo.isDownPass = false;
                });
            }
        }
    }

}