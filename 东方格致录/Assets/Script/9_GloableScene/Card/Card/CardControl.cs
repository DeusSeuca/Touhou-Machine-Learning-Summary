using CardSpace;
using UnityEngine;
namespace Control
{
    public class CardControl : MonoBehaviour
    {
        private void OnMouseEnter()
        {
            Info.GlobalBattleInfo.PlayerFocusCard = GetComponent<Card>();
            Command.NetCommand.AsyncInfo(NetAcyncType.FocusCard);
        }
        private void OnMouseExit()
        {
            if (Info.GlobalBattleInfo.PlayerFocusCard == GetComponent<Card>())
            {
                Info.GlobalBattleInfo.PlayerFocusCard = null;
                Command.NetCommand.AsyncInfo(NetAcyncType.FocusCard);
            }
        }
    }
}

