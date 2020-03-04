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
            if (ThisCard.isPrepareToPlay)
            {
                Info.AgainstInfo.PlayerPlayCard = ThisCard;
            }
            //Command.EffectCommand.TheWorldPlay(GetComponent<Card>());
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
                        Debug.Log("1打出一张牌"+ Info.AgainstInfo.PlayerPlayCard);
                        _ = GameSystem.TransSystem.PlayCard(TriggerInfo.Build(Info.AgainstInfo.PlayerPlayCard, Info.AgainstInfo.PlayerPlayCard));
                    }
                }
                else
                {
                    Debug.Log("2打出一张牌"+ Info.AgainstInfo.PlayerPlayCard);
                    _ = GameSystem.TransSystem.PlayCard(TriggerInfo.Build(Info.AgainstInfo.PlayerPlayCard, Info.AgainstInfo.PlayerPlayCard));
                }
            }
        }
        private void Update()
        {
            if (gap_step == 1)
            {
                gapMaterial.SetFloat("_gapWidth", Mathf.Lerp(gapMaterial.GetFloat("_gapWidth"), 1.5f, Time.deltaTime * 20));
            }
            else if (gap_step == 2)
            {
                gapMaterial.SetFloat("_gapWidth", Mathf.Lerp(gapMaterial.GetFloat("_gapWidth"), 10, Time.deltaTime * 2));
                cardMaterial.SetFloat("_gapWidth", Mathf.Lerp(cardMaterial.GetFloat("_gapWidth"), 10, Time.deltaTime * 2));
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

