using CardSpace;
using GameUI;
using System.Collections.Generic;
using Thread;
using UnityEngine;
using UnityEngine.UI;
namespace Command
{
    namespace GameUI
    {
        public class CardBoardCommand
        {
            public static void LoadCardList(List<int> CardsIds)
            {
                Info.GlobalBattleInfo.TargetCardIDList = CardsIds;
                CreatBoardCardVitual();
            }
            public static void LoadCardList(List<Card> CardsIds)
            {
                Info.GlobalBattleInfo.TargetCardList = CardsIds;
                CreatBoardCardActual();
            }
            public void Replace(int num, Card card)
            {

            }
            public static void CreatBoardCardActual()
            {
                MainThread.Run(() =>
                {
                    Info.GameUI.UiInfo.CardBoard.transform.GetChild(1).GetComponent<Text>().text = Info.GameUI.UiInfo.CardBoardTitle;
                    Info.GameUI.UiInfo.ShowCardLIstOnBoard.ForEach(GameObject.Destroy);
                    List<Card> Cards = Info.GlobalBattleInfo.TargetCardList;
                    for (int i = 0; i < Cards.Count; i++)
                    {
                        var CardStandardInfo = Command.CardLibraryCommand.GetCardStandardInfo(Cards[i].CardId);
                        GameObject NewCard = GameObject.Instantiate(Info.GameUI.UiInfo.CardModel);
                        NewCard.GetComponent<BoardCardInfo>().Rank = i;
                        NewCard.transform.SetParent(Info.GameUI.UiInfo.Constant);
                        Texture2D texture = CardStandardInfo.icon;
                        NewCard.GetComponent<Image>().sprite = Command.GameUI.UiCommand.GetBoardCardImage(Cards[i].CardId);
                        Info.GameUI.UiInfo.ShowCardLIstOnBoard.Add(NewCard);
                    }
                    Info.GameUI.UiInfo.Constant.GetComponent<RectTransform>().sizeDelta = new Vector2(Cards.Count * 325 + 200, 800);
                });
            }
            private static void CreatBoardCardVitual()
            {
                MainThread.Run(() =>
                {
                    Info.GameUI.UiInfo.CardBoard.transform.GetChild(1).GetComponent<Text>().text = Info.GameUI.UiInfo.CardBoardTitle;
                    Info.GameUI.UiInfo.ShowCardLIstOnBoard.ForEach(GameObject.Destroy);
                    List<int> CardIds = Info.GlobalBattleInfo.TargetCardIDList;
                    for (int i = 0; i < CardIds.Count; i++)
                    {
                        var CardStandardInfo = Command.CardLibraryCommand.GetCardStandardInfo(CardIds[i]);
                        GameObject NewCard = GameObject.Instantiate(Info.GameUI.UiInfo.CardModel);
                        NewCard.GetComponent<BoardCardInfo>().Rank = i;
                        NewCard.transform.SetParent(Info.GameUI.UiInfo.Constant);
                        Texture2D texture = CardStandardInfo.icon;
                        NewCard.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                        Info.GameUI.UiInfo.ShowCardLIstOnBoard.Add(NewCard);
                    }
                    Info.GameUI.UiInfo.Constant.GetComponent<RectTransform>().sizeDelta = new Vector2(CardIds.Count * 325 + 200, 800);
                });

            }
        }
    }
}