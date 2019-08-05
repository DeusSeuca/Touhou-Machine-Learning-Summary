using UnityEngine;
using UnityEngine.UI;

namespace Command
{
    public class UiCommand : MonoBehaviour
    {
        static GameObject MyPass => Info.UiInfo.Instance.MyPass;
        static GameObject OpPass => Info.UiInfo.Instance.OpPass;
        public static void SetCardBoardShow()
        {
            MainThread.Run(() =>
            {
                Info.UiInfo.CardBoard.SetActive(true);
                Info.UiInfo.CardBoard.transform.GetChild(1).GetComponent<Text>().text = Info.UiInfo.CardBoardTitle;
            });
        }
        public static void SetCardBoardHide()
        {
            MainThread.Run(() =>
            {
                Info.UiInfo.CardBoard.SetActive(false);
            });
        }

        public static void CardBoardReload() => Command.CardBoardCommand.CreatBoardCardActual();
        public static Sprite GetBoardCardImage(int Id)
        {
            if (!Info.UiInfo.CardImage.ContainsKey(Id))
            {
                var CardStandardInfo = CardLibrary.GetCardStandardInfo(Id);
                Texture2D texture = CardStandardInfo.Icon;
                Info.UiInfo.CardImage.Add(Id, Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero));
            }
            return Info.UiInfo.CardImage[Id];
        }
        public static void ChangeIntroduction(CardSpace.Card card)
        {
            string Title = card.CardName;
            string Text = card.CardIntroduction;
            string Effect = "";
            int Heigh = Text.Length / 13 * 15 + 100;
            Info.UiInfo.IntroductionTextBackground.sizeDelta = new Vector2(300, Heigh);
            //修改文本为富文本
            Info.UiInfo.IntroductionTitle.text = Title;
            Info.UiInfo.IntroductionText.text = Text;
            Info.UiInfo.IntroductionEffect.text = Effect;
        }
        public static void SetCardBoardTitle(string Title) => Info.UiInfo.CardBoardTitle = Title;
        public static void SetNoticeBoardTitle(string Title) => Info.UiInfo.NoticeBoardTitle = Title;
        public static void SetArrowShow()
        {
            MainThread.Run(() =>
            {
                bool IsFirst = Info.GlobalBattleInfo.ArrowList.Count == 0;
                GameObject NewArrow = Instantiate(Info.UiInfo.Arrow);
                NewArrow.GetComponent<ArrowManager>().RefreshArrow(
                    Info.GlobalBattleInfo.ArrowStartCard.transform,
                    IsFirst ? Info.UiInfo.ArrowEndPoint.transform :
                    Info.GlobalBattleInfo.PlayerFocusCard.transform
                    );
                Info.GlobalBattleInfo.ArrowList.Add(NewArrow);
            });
        }

        public static void SetArrowDestory()
        {
            MainThread.Run(() =>
            {
                Info.GlobalBattleInfo.ArrowList.ForEach(Destroy);
                Info.GlobalBattleInfo.ArrowList.Clear();
            });
        }
        public static void NoticeBoardShow()
        {
            MainThread.Run(() =>
            {
                Info.UiInfo.NoticeBoard.GetComponent<Text>().text = Info.UiInfo.NoticeBoardTitle;
                Info.UiInfo.Instance.NoticeAnim.SetTrigger("Play");
            });
        }
        public static void SetCardBoardMode(CardBoardMode CardBoardMode) => Info.GlobalBattleInfo.CardBoardMode = CardBoardMode;
        public static void SetCurrentPass()
        {
            if (Info.GlobalBattleInfo.IsPlayer1 ^ Info.GlobalBattleInfo.IsMyTurn)
            {
                Info.GlobalBattleInfo.IsPlayer2Pass = true;
            }
            else
            {
                Info.GlobalBattleInfo.IsPlayer1Pass = true;
            }
        }
        public static void ReSetPassState()
        {
            Info.GlobalBattleInfo.IsPlayer1Pass = false;
            Info.GlobalBattleInfo.IsPlayer2Pass = false;
        }
    }
}

