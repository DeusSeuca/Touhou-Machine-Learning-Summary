using CardSpace;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Command
{
    public class CardBoardCommand
    {
        public static void LoadCardList(List<int> CardsIds)
        {
            // Debug.Log("执行虚牌组载入");
            Info.GlobalBattleInfo.TargetCardIDList = CardsIds;
            CreatBoardCardVitual();
        }
        public static void LoadCardList(List<Card> CardsIds)
        {
            //Debug.Log("执行实牌组载入");
            Info.GlobalBattleInfo.TargetCardList = CardsIds;
            CreatBoardCardActual();


        }
        public void Replace(int num, Card card)
        {

        }
        public static void CreatBoardCardActual()
        {
            MainThread.Run(() =>{
                Info.UiInfo.CardBoard.transform.GetChild(1).GetComponent<Text>().text = Info.UiInfo.CardBoardTitle;
                Info.UiInfo.ShowCardLIstOnBoard.ForEach(GameObject.Destroy);
                List<Card> Cards = Info.GlobalBattleInfo.TargetCardList;
                for (int i = 0; i < Cards.Count; i++)
                {
                    var CardStandardInfo = CardLibrary.GetCardStandardInfo(Cards[i].CardId);
                    GameObject NewCard = GameObject.Instantiate(Info.UiInfo.CardModel);
                    NewCard.GetComponent<BoardCardInfo>().Rank = i;
                    NewCard.transform.SetParent(Info.UiInfo.Constant);
                    Texture2D texture = CardStandardInfo.Icon;
                    NewCard.GetComponent<Image>().sprite = Command.UiCommand.GetBoardCardImage(Cards[i].CardId);
                    Info.UiInfo.ShowCardLIstOnBoard.Add(NewCard);
                }
                Info.UiInfo.Constant.GetComponent<RectTransform>().sizeDelta = new Vector2(Cards.Count * 325 + 200, 800);
            });
        }
        private static void CreatBoardCardVitual()
        {
            MainThread.Run(() => {
                Info.UiInfo.CardBoard.transform.GetChild(1).GetComponent<Text>().text = Info.UiInfo.CardBoardTitle;
                Info.UiInfo.ShowCardLIstOnBoard.ForEach(GameObject.Destroy);
                List<int> CardIds = Info.GlobalBattleInfo.TargetCardIDList;
                for (int i = 0; i < CardIds.Count; i++)
                {
                    var CardStandardInfo = CardLibrary.GetCardStandardInfo(CardIds[i]);
                    GameObject NewCard = GameObject.Instantiate(Info.UiInfo.CardModel);
                    NewCard.GetComponent<BoardCardInfo>().Rank = i;
                    NewCard.transform.SetParent(Info.UiInfo.Constant);
                    Texture2D texture = CardStandardInfo.Icon;
                    NewCard.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                    Info.UiInfo.ShowCardLIstOnBoard.Add(NewCard);
                }
                Info.UiInfo.Constant.GetComponent<RectTransform>().sizeDelta = new Vector2(CardIds.Count * 325 + 200, 800);
            });
            
        }
    }
}