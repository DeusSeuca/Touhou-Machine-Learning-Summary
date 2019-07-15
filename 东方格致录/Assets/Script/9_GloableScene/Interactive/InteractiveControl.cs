﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Info;
using Command;
using System.Threading.Tasks;
using System;
using CardSpace;

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
           
                if (Input.GetKey(KeyCode.Space)&& Info.GlobalBattleInfo.IsMyTurn)
                {
                    PassPressTime += Time.deltaTime;
                    if (PassPressTime > 2)
                    {
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
                if (GlobalBattleInfo.IsWaitForSelectUnits && !GlobalBattleInfo.PlayerFocusCard.IsActive)
                {
                    print("选择了卡片");
                    GlobalBattleInfo.SelectUnits.Add(GlobalBattleInfo.PlayerFocusCard);
                    Command.UiCommand.SetArrowShow();
                    //Instantiate(Info.UiInfo.Arrow).GetComponent<ArrowManager>().RefreshArrow
                    //    (
                    //    GlobalBattleInfo.ArrowStartCard.transform,
                    //    GlobalBattleInfo.PlayerFocusCard.transform
                    //    );
                }
                if (GlobalBattleInfo.IsWaitForSelectLocation)
                {
                    if (GlobalBattleInfo.PlayerFocusRegion != null && GlobalBattleInfo.PlayerFocusRegion.CanBeSelected)
                    {
                        //print("选择位置" + GlobalBattleInfo.PlayerFocusRegion.Rank);
                        GlobalBattleInfo.SelectRegion = GlobalBattleInfo.PlayerFocusRegion;
                        GlobalBattleInfo.SelectLocation = GlobalBattleInfo.PlayerFocusRegion.Rank;
                    }
                }
                // print($"所在行为{GlobalBattleInfo.PlayerFocusCard.Row}，所在坐标为{GlobalBattleInfo.PlayerFocusCard.Location}");
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
                            print("出发丢弃卡牌");
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