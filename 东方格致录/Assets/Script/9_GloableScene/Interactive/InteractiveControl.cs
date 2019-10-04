using GameEnum;
using Info;
using UnityEngine;

namespace Control
{
    public class InteractiveControl : MonoBehaviour
    {
        Ray ray;
        public float PassPressTime;
        void Update()
        {
            GetFocusTarget();
            MouseEvent();
            KeyBoardEvent();
        }
        private void GetFocusTarget()
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] Infos = Physics.RaycastAll(ray);
            if (Infos.Length > 0)
            {
                for (int i = 0; i < Infos.Length; i++)
                {
                    if (Infos[i].transform.GetComponent<SingleRowInfo>() != null)
                    {
                        AgainstInfo.PlayerFocusRegion = Infos[i].transform.GetComponent<SingleRowInfo>();
                        AgainstInfo.FocusPoint = Infos[i].point;
                        break;
                    }
                    AgainstInfo.PlayerFocusRegion = null;
                }
            }
        }
        private void KeyBoardEvent()
        {

            if (Input.GetKey(KeyCode.Space) && Info.AgainstInfo.IsMyTurn)
            {
                PassPressTime += Time.deltaTime;
                if (PassPressTime > 2)
                {
                    Command.Network.NetCommand.AsyncInfo(NetAcyncType.Pass);
                    Command.GameUI.UiCommand.SetCurrentPass();
                    PassPressTime = 0;
                }
            }
            if (Input.GetKeyUp(KeyCode.Space) && Info.AgainstInfo.IsMyTurn)
            {
                PassPressTime = 0;
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _ = Command.StateCommand.Surrender();
            }
        }
        private void MouseEvent()
        {
            if (Input.GetMouseButtonDown(0) && Info.AgainstInfo.IsMyTurn)
            {
                //if (GlobalBattleInfo.PlayerFocusCard != null && GlobalBattleInfo.PlayerFocusCard.IsPrePrepareToPlay)
                //{
                //    GlobalBattleInfo.PlayerPlayCard = GlobalBattleInfo.PlayerFocusCard;
                //}
                if (AgainstInfo.IsWaitForSelectRegion)
                {
                    AgainstInfo.SelectRegion = AgainstInfo.PlayerFocusRegion;
                }
                if (AgainstInfo.IsWaitForSelectUnits && AgainstInfo.PlayerFocusCard != null && !AgainstInfo.PlayerFocusCard.IsGray)
                {
                    AgainstInfo.SelectUnits.Add(AgainstInfo.PlayerFocusCard);
                    Command.GameUI.UiCommand.SetArrowShow();
                }
                if (AgainstInfo.IsWaitForSelectLocation)
                {
                    if (AgainstInfo.PlayerFocusRegion != null && AgainstInfo.PlayerFocusRegion.CanBeSelected)
                    {
                        AgainstInfo.SelectRegion = AgainstInfo.PlayerFocusRegion;
                        AgainstInfo.SelectLocation = AgainstInfo.PlayerFocusRegion.Location;
                    }
                }
            }
            if (Input.GetMouseButton(0) && Info.AgainstInfo.IsMyTurn)
            {
                LayerMask mask = 1 << LayerMask.NameToLayer("Default");
                if (Physics.Raycast(ray, out RaycastHit HitInfo, 100, mask))
                {
                    AgainstInfo.DragToPoint = HitInfo.point;
                }
            }
        }
    }
}