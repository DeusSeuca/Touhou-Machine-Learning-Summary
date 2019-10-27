using CardModel;
using CardSpace;
using Command.Network;
using GameEnum;
using UnityEngine;
namespace Control
{
    public class CardControl : MonoBehaviour
    {
        Card ThisCard => GetComponent<Card>();
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
        }
        private void OnMouseUp()
        {
            if (Info.AgainstInfo.PlayerPlayCard != null)
            {
                if (Info.AgainstInfo.PlayerFocusRegion != null)
                {
                    if (Info.AgainstInfo.PlayerFocusRegion.name == "下方_墓地")
                    {
                        print(name + "进入墓地");
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
    }
}

