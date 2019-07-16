using CardSpace;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Info
{

    public class ThreadAgent : MonoBehaviour
    {
        static int num;

        private void Update()
        {
            
            InvokeInMainThread(ref Info.GlobalBattleInfo.IsCreatCard, CreatCard);
            InvokeInMainThread(ref Info.GlobalBattleInfo.IsPlaySound, PlaySound);
            InvokeInMainThread(ref Info.GlobalBattleInfo.IsArrowShow, ArrowShow);
            InvokeInMainThread(ref Info.GlobalBattleInfo.IsArrowHide, ArrowHide);
            //InvokeInMainThread(ref Info.GlobalBattleInfo.IsNotifyShow, NotifyShow);
            //InvokeInMainThread(ref Info.GlobalBattleInfo.IsBattleEnd, IsBattleEnd);

            //InvokeInMainThread(ref Info.GlobalBattleInfo.IsPlayer1Pass, IsPlayer1Pass);
            //InvokeInMainThread(ref Info.GlobalBattleInfo.IsPlayer2Pass, IsPlayer2Pass);
            InvokeInMainThread(ref Info.GlobalBattleInfo.IsPlayParticle, PlayParticle);
            InvokeInMainThread(ref Info.GlobalBattleInfo.IsCardBoardShow, CardBoardShow);
            InvokeInMainThread(ref Info.GlobalBattleInfo.IsCardBoardHide, CardBoardHide);
            InvokeInMainThread(ref Info.GlobalBattleInfo.CardBoardReload, CardBoardReload);
            InvokeInMainThread(ref Info.GlobalBattleInfo.IsCreatBoardCardActual, CreatBoardCardActual);
            InvokeInMainThread(ref Info.GlobalBattleInfo.IsCreatBoardCardVitual, CreatBoardCardVitual);

        }

        private void IsBattleEnd()
        {
            SceneManager.LoadSceneAsync(1);
        }

        //private void IsPlayer1Pass()
        //{
        //    Info.UiInfo.Instance.MyPass.SetActive(true);
        //}
        //private void IsPlayer2Pass()
        //{
        //    Info.UiInfo.Instance.MyPass.SetActive(true);
        //}

        private void ArrowShow()
        {
            print("触发");
            bool IsFirst = Info.GlobalBattleInfo.ArrowList.Count == 0;
            GameObject NewArrow = Instantiate(Info.UiInfo.Arrow);
            NewArrow.GetComponent<ArrowManager>().RefreshArrow(
                GlobalBattleInfo.ArrowStartCard.transform,
                IsFirst ? Info.UiInfo.ArrowEndPoint.transform :
                GlobalBattleInfo.PlayerFocusCard.transform
                );
            Info.GlobalBattleInfo.ArrowList.Add(NewArrow);
        }
        private void ArrowHide()
        {
            Info.GlobalBattleInfo.ArrowList.ForEach(Destroy);
            Info.GlobalBattleInfo.ArrowList.Clear();

        }
        private void PlaySound()
        {
            AudioSource Source = gameObject.AddComponent<AudioSource>();
            Source.clip = SoundInfo.Instance.Clips[Info.GlobalBattleInfo.PlaySoundRank];
            Source.Play();
            Destroy(Source, Source.clip.length);
        }

        private void PlayParticle()
        {
            ParticleSystem TargetParticle =Instantiate(Info.ParticleInfo.Instance.ParticleEffect[Info.GlobalBattleInfo.PlayParticleRank]);
            TargetParticle.transform.position = Info.GlobalBattleInfo.PlayParticlePos;
            TargetParticle.Play();
        }

        //private void NotifyShow()
        //{
        //    Info.UiInfo.NoticeBoard.GetComponent<Text>().text = Info.UiInfo.NoticeBoardTitle;
        //    Info.UiInfo.Instance.NoticeAnim.SetTrigger("Play");
        //    //Info.UiInfo.NoticeBoard.SetActive(true);
        //}

        //private void NotifyHide()
        //{
        //    Info.UiInfo.NoticeBoard.SetActive(false);
        //}
        private void CardBoardShow()
        {
            UiInfo.CardBoard.SetActive(true);
            Info.UiInfo.CardBoard.transform.GetChild(1).GetComponent<Text>().text = Info.UiInfo.CardBoardTitle;
        }
        private void CardBoardHide()
        {
            UiInfo.CardBoard.SetActive(false);
        }
        private void CardBoardReload()
        {
            Info.GlobalBattleInfo.IsCreatBoardCardActual = true;
        }
        private void InvokeInMainThread(ref bool TriggerSign, Action RunFunction)
        {
            if (TriggerSign)
            {
                RunFunction();
                TriggerSign = false;
            }
        }
        private static void CreatCard()
        {
            GameObject NewCard = Instantiate(CardLibrary.Instance.Card_Model);
            NewCard.name = num + "";
            num++;
            int id = Info.GlobalBattleInfo.TargetCardID;
            var CardStandardInfo = CardLibrary.GetCardStandardInfo(id);
            NewCard.AddComponent(Type.GetType("Card" + id));
            Card card = NewCard.GetComponent<Card>();
            card.CardId = CardStandardInfo.CardId;
            card.CardPoint = CardStandardInfo.Point;
            card.icon = CardStandardInfo.Icon;
            card.CardProperty = CardStandardInfo.CardProperty;
            card.CardTerritory = CardStandardInfo.CardTerritory;
            card.GetComponent<Renderer>().material.SetTexture("_Front", card.icon);
            card.Init();
            Info.GlobalBattleInfo.CreatedCard = card;
        }
        private static void DestoryCard()
        {
        }
        private static void CreatBoardCardActual()
        {
            Info.UiInfo.CardBoard.transform.GetChild(1).GetComponent<Text>().text = Info.UiInfo.CardBoardTitle;
            Info.UiInfo.ShowCardLIstOnBoard.ForEach(Destroy);
            List<Card> Cards = Info.GlobalBattleInfo.TargetCardList;
            for (int i = 0; i < Cards.Count; i++)
            {
                var CardStandardInfo = CardLibrary.GetCardStandardInfo(Cards[i].CardId);
                GameObject NewCard = Instantiate(Info.UiInfo.CardModel);
                NewCard.GetComponent<BoardCardInfo>().Rank = i;
                NewCard.transform.SetParent(Info.UiInfo.Constant);
                Texture2D texture = CardStandardInfo.Icon;
                NewCard.GetComponent<Image>().sprite = Command.UiCommand.GetBoardCardImage(Cards[i].CardId);
                Info.UiInfo.ShowCardLIstOnBoard.Add(NewCard);
            }
            Info.UiInfo.Constant.GetComponent<RectTransform>().sizeDelta = new Vector2(Cards.Count * 325 + 200, 800);
        }
        private static void CreatBoardCardVitual()
        {
            Info.UiInfo.CardBoard.transform.GetChild(1).GetComponent<Text>().text = Info.UiInfo.CardBoardTitle;
            UiInfo.ShowCardLIstOnBoard.ForEach(Destroy);
            List<int> CardIds = Info.GlobalBattleInfo.TargetCardIDList;
            for (int i = 0; i < CardIds.Count; i++)
            {
                var CardStandardInfo = CardLibrary.GetCardStandardInfo(CardIds[i]);
                GameObject NewCard = Instantiate(Info.UiInfo.CardModel);
                NewCard.GetComponent<BoardCardInfo>().Rank = i;
                NewCard.transform.SetParent(Info.UiInfo.Constant);
                Texture2D texture = CardStandardInfo.Icon;
                NewCard.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                Info.UiInfo.ShowCardLIstOnBoard.Add(NewCard);
            }
            Info.UiInfo.Constant.GetComponent<RectTransform>().sizeDelta = new Vector2(CardIds.Count * 325 + 200, 800);
        }
    }
}
