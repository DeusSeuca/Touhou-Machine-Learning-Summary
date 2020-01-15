using CardModel;
using CardSpace;
using Command.Network;
using GameEnum;
using UnityEngine;
namespace Control
{
    public class CardControl : MonoBehaviour
    {
        int gap_step = 0;
        Card ThisCard => GetComponent<Card>();
        GameObject gap => transform.GetChild(1).gameObject;
        Material gapMaterial => gap.GetComponent<Renderer>().material;
        Material cardMaterial => GetComponent<Renderer>().material;
        private void OnMouseEnter()
        {
            Info.AgainstInfo.PlayerFocusCard = ThisCard;
            Command.Network.NetCommand.AsyncInfo(NetAcyncType.FocusCard);
        }
        private void OnMouseExit()
        {
            if (Info.AgainstInfo.PlayerFocusCard == ThisCard)
            {
                Info.AgainstInfo.PlayerFocusCard = null;
                NetCommand.AsyncInfo(NetAcyncType.FocusCard);
            }
        }
        private void OnMouseDown()
        {
            if (ThisCard.IsPrePrepareToPlay)
            {
                Info.AgainstInfo.PlayerPlayCard = ThisCard;
            }
            //临时间隙
            GameSystem.TransSystem.BanishCard(GetComponent<Card>());
        }
        private void OnMouseUp()
        {
            if (Info.AgainstInfo.PlayerPlayCard != null)
            {
                if (Info.AgainstInfo.PlayerFocusRegion != null)
                {
                    if (Info.AgainstInfo.PlayerFocusRegion.name == "下方_墓地")
                    {
                        //print(name + "进入墓地");
                        _ = Command.CardCommand.DisCard(ThisCard);
                    }
                    else if (Info.AgainstInfo.PlayerFocusRegion.name == "下方_领袖" || Info.AgainstInfo.PlayerFocusRegion.name == "下方_手牌")
                    {
                        Info.AgainstInfo.PlayerPlayCard = null;
                    }
                    else
                    {
                        _ = Command.CardCommand.PlayCard(Info.AgainstInfo.PlayerPlayCard);
                    }
                }
                else
                {
                    _ = Command.CardCommand.PlayCard(Info.AgainstInfo.PlayerPlayCard);
                }
            }
        }
        private void Update()
        {
            if (gap_step == 1)
            {
                gapMaterial.SetFloat("_width", Mathf.Lerp(gapMaterial.GetFloat("_width"), 1.5f, Time.deltaTime * 5));
            }
            else if (gap_step == 2)
            {
                gapMaterial.SetFloat("_width", Mathf.Lerp(gapMaterial.GetFloat("_width"), 10, Time.deltaTime * 2));
                cardMaterial.SetFloat("_gap", Mathf.Lerp(cardMaterial.GetFloat("_gap"), 10, Time.deltaTime * 2));
            }
        }
        public void CreatGap()
        {
            gap.SetActive(true);
            gap_step = 1;
        }
        public void FoldGap()
        {
            transform.GetChild(0).gameObject.SetActive(false);
            gap_step = 2;
        }
        public void DestoryGap()
        {
            gap.SetActive(false);
            gap_step = 0;
            Destroy(gameObject);
        }
    }
}

