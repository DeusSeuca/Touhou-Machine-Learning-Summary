using CardSpace;
using Command;
using Info;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
namespace Control
{
    public class RowControl : MonoBehaviour
    {
        SingleRowInfo SingleInfo;
        public float Range;
        public bool IsMyHandRegion;
        public bool IsSingle;
        public bool HasTempCard;
        void Awake()
        {
            SingleInfo = GetComponent<SingleRowInfo>();
        }
        void Update()
        {
            ControlCardPosition(SingleInfo.ThisRowCards);
            RefreshHandCard(SingleInfo.ThisRowCards);
            TempCardControk();
        }
        public void TempCardControk()
        {
            if (SingleInfo.TempCard == null && SingleInfo.CanBeSelected && GlobalBattleInfo.PlayerFocusRegion == SingleInfo && !HasTempCard)
            {
                HasTempCard = true;
                //print(SingleInfo.TempCard);
                _ = CreatTempCard();
            }
            if (SingleInfo.TempCard != null && SingleInfo.Location != SingleInfo.ThisRowCards.IndexOf(SingleInfo.TempCard))
            {
                ChangeTempCard();
            }
            if (SingleInfo.TempCard != null && (!SingleInfo.CanBeSelected || GlobalBattleInfo.PlayerFocusRegion != SingleInfo))
            {
                DestoryTempCard();
                HasTempCard = false;
            }
        }
        public async Task CreatTempCard()
        {
            SingleInfo.TempCard = await CardCommand.CreatCard(RowsInfo.GetRegionCardList(RegionName_Other.My_Uesd).ThisRowCards[0].CardId);
            SingleInfo.TempCard.IsGray = true;
            SingleInfo.TempCard.IsCanSee = true;
            SingleInfo.ThisRowCards.Insert(SingleInfo.Location, SingleInfo.TempCard);
            SingleInfo.TempCard.Init();
        }
        public void DestoryTempCard()
        {
            SingleInfo.ThisRowCards.Remove(SingleInfo.TempCard);
            Destroy(SingleInfo.TempCard.gameObject);
            SingleInfo.TempCard = null;
        }
        public void ChangeTempCard()
        {
            SingleInfo.ThisRowCards.Remove(SingleInfo.TempCard);
            SingleInfo.ThisRowCards.Insert(SingleInfo.Location, SingleInfo.TempCard);
        }
        //public void SetSelectable(bool Seleceable)
        //{
        //    SingleInfo.CanBeSelected = Seleceable;
        //    transform.GetComponent<Renderer>().material.SetColor("_GlossColor", SingleInfo.CanBeSelected ? SingleInfo.color : Color.black);
        //}
        void RefreshHandCard(List<Card> ThisCardList)
        {
            if (IsMyHandRegion)
            {
                foreach (var item in ThisCardList)
                {
                    if (GlobalBattleInfo.PlayerFocusCard != null && item == GlobalBattleInfo.PlayerFocusCard && item.IsLimit == false)
                    {
                        item.IsPrePrepareToPlay = true;
                    }
                    else
                    {
                        item.IsPrePrepareToPlay = false;
                    }
                }
            }
        }
        void ControlCardPosition(List<Card> ThisCardList)
        {
            int Num = ThisCardList.Count;
            for (int i = 0; i < ThisCardList.Count; i++)
            {

                float Actual_Interval = Mathf.Min(Range / Num, 1.6f);
                float Actual_Bias = IsSingle ? 0 : (Mathf.Min(ThisCardList.Count, 6) - 1) * 0.8f;
                //Bias = Actual_Bias;
                Vector3 Actual_Offset_Up = transform.up * (0.2f + i * 0.01f) * (ThisCardList[i].IsPrePrepareToPlay ? 1.1f : 1); //transform.up * (1 + i * 0.1f);//Vector3.up * (1 + i * 0.1f);
                Vector3 MoveStepOver_Offset = ThisCardList[i].IsMoveStepOver ? Vector3.zero : Vector3.up;                                                                                                               // Vector3 Actual_Offset_Up = transform.up * i; //transform.up * (1 + i * 0.1f);//Vector3.up * (1 + i * 0.1f);
                Vector3 Actual_Offset_Forward = ThisCardList[i].IsPrePrepareToPlay ? -transform.forward * 0.5f : Vector3.zero;
                if (ThisCardList[i].IsAutoMove)
                {
                    ThisCardList[i].SetMoveTarget(transform.position + Vector3.left * (Actual_Interval * i - Actual_Bias) + Actual_Offset_Up + Actual_Offset_Forward + MoveStepOver_Offset, transform.eulerAngles);
                }
                else
                {
                    ThisCardList[i].SetMoveTarget(GlobalBattleInfo.DragToPoint, Vector3.zero);
                }
                ThisCardList[i].RefreshState();
            }
        }
    }
}

