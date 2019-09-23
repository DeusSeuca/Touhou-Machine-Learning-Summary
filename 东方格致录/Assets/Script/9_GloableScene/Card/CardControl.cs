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
            Info.GlobalBattleInfo.PlayerFocusCard = ThisCard;
            Command.Network.NetCommand.AsyncInfo(NetAcyncType.FocusCard);
        }
        private void OnMouseExit()
        {
            if (Info.GlobalBattleInfo.PlayerFocusCard == ThisCard)
            {
                Info.GlobalBattleInfo.PlayerFocusCard = null;
                NetCommand.AsyncInfo(NetAcyncType.FocusCard);
            }
        }
        private void OnMouseDown()
        {
            if (ThisCard.IsPrePrepareToPlay)
            {
                Info.GlobalBattleInfo.PlayerPlayCard = ThisCard;
            }

        }
        private void OnMouseUp()
        {
            if (Info.GlobalBattleInfo.PlayerFocusRegion != null)
            {
                if (Info.GlobalBattleInfo.PlayerFocusRegion.name == "我方_墓地")
                {
                    _ = Command.CardCommand.DisCard();
                }
                else if (Info.GlobalBattleInfo.PlayerFocusRegion.name == "我方_手牌")
                {
                    Info.GlobalBattleInfo.PlayerPlayCard = null;
                }
                else
                {
                    _ = Command.CardCommand.PlayCard(true);
                }
            }
            else
            {
                _ = Command.CardCommand.PlayCard(true);
            }
        }

    }
}

