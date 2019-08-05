using Command;
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
                        GlobalBattleInfo.PlayerFocusRegion = Infos[i].transform.GetComponent<SingleRowInfo>();
                        GlobalBattleInfo.FocusPoint = Infos[i].point;
                        break;
                    }
                    GlobalBattleInfo.PlayerFocusRegion = null;
                }
            }
        }
        private void KeyBoardEvent()
        {

            if (Input.GetKey(KeyCode.Space) && Info.GlobalBattleInfo.IsMyTurn)
            {
                PassPressTime += Time.deltaTime;
                if (PassPressTime > 2)
                {
                    NetCommand.AsyncInfo(NetAcyncType.Pass);
                    UiCommand.SetCurrentPass();
                    PassPressTime = 0;
                }
            }
            if (Input.GetKeyUp(KeyCode.Space) && Info.GlobalBattleInfo.IsMyTurn)
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
            if (Input.GetMouseButtonDown(0) && Info.GlobalBattleInfo.IsMyTurn)
            {
                if (GlobalBattleInfo.PlayerFocusCard != null && GlobalBattleInfo.PlayerFocusCard.IsPrePrepareToPlay)
                {
                    GlobalBattleInfo.PlayerPlayCard = GlobalBattleInfo.PlayerFocusCard;
                }
                if (GlobalBattleInfo.IsWaitForSelectRegion)
                {
                    GlobalBattleInfo.SelectRegion = GlobalBattleInfo.PlayerFocusRegion;
                }
                if (GlobalBattleInfo.IsWaitForSelectUnits && GlobalBattleInfo.PlayerFocusCard!=null && !GlobalBattleInfo.PlayerFocusCard.IsGray)
                {
                    GlobalBattleInfo.SelectUnits.Add(GlobalBattleInfo.PlayerFocusCard);
                    Command.UiCommand.SetArrowShow();
                }
                if (GlobalBattleInfo.IsWaitForSelectLocation)
                {
                    if (GlobalBattleInfo.PlayerFocusRegion != null && GlobalBattleInfo.PlayerFocusRegion.CanBeSelected)
                    {
                        GlobalBattleInfo.SelectRegion = GlobalBattleInfo.PlayerFocusRegion;
                        GlobalBattleInfo.SelectLocation = GlobalBattleInfo.PlayerFocusRegion.Location;
                    }
                }
            }
            if (Input.GetMouseButton(0) && Info.GlobalBattleInfo.IsMyTurn)
            {
                LayerMask mask = 1 << LayerMask.NameToLayer("Default");
                if (Physics.Raycast(ray, out RaycastHit HitInfo, 100, mask))
                {
                    GlobalBattleInfo.DragToPoint = HitInfo.point;
                }
            }
            if (Input.GetMouseButtonUp(0) && Info.GlobalBattleInfo.IsMyTurn)
            {
                if (GlobalBattleInfo.PlayerPlayCard != null)
                {
                    if (GlobalBattleInfo.PlayerFocusRegion != null)
                    {
                        if (GlobalBattleInfo.PlayerFocusRegion.name == "我方_墓地")
                        {
                            _ = CardCommand.DisCard();
                        }
                        else if (GlobalBattleInfo.PlayerFocusRegion.name == "我方_手牌")
                        {
                            GlobalBattleInfo.PlayerPlayCard = null;
                        }
                        else
                        {
                            _ = CardCommand.PlayCard();
                        }
                    }
                    else
                    {
                        _ = CardCommand.PlayCard();
                    }
                }
            }
        }
    }
}