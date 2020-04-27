using CardModel;
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
            public static void LoadCardList(List<int> cardIds)
            {
                Info.AgainstInfo.cardBoardIDList = cardIds;
                CreatBoardCardVitual();
            }
            public static void LoadCardList(List<Card> cards)
            {
                Info.AgainstInfo.cardBoardList = cards;
                CreatBoardCardActual();
            }
            public void Replace(int num, Card card)
            {

            }
            //生成对局存在的卡牌
            public static void CreatBoardCardActual()
            {
                MainThread.Run(() =>
                {
                    Info.GameUI.UiInfo.CardBoard.transform.GetChild(1).GetComponent<Text>().text = Info.GameUI.UiInfo.CardBoardTitle;
                    Info.GameUI.UiInfo.ShowCardLIstOnBoard.ForEach(GameObject.Destroy);
                    List<Card> Cards = Info.AgainstInfo.cardBoardList;
                    for (int i = 0; i < Cards.Count; i++)
                    {
                        var CardStandardInfo = Command.CardInspector.CardLibraryCommand.GetCardStandardInfo(Cards[i].CardId);
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
            //生成对局不存在的卡牌
            private static void CreatBoardCardVitual()
            {
                MainThread.Run(() =>
                {
                    Info.GameUI.UiInfo.CardBoard.transform.GetChild(1).GetComponent<Text>().text = Info.GameUI.UiInfo.CardBoardTitle;
                    Info.GameUI.UiInfo.ShowCardLIstOnBoard.ForEach(GameObject.Destroy);
                    List<int> CardIds = Info.AgainstInfo.cardBoardIDList;
                    for (int i = 0; i < CardIds.Count; i++)
                    {
                        var CardStandardInfo = Command.CardInspector.CardLibraryCommand.GetCardStandardInfo(CardIds[i]);
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