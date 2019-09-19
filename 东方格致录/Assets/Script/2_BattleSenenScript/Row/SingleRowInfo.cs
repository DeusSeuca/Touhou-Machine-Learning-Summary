using CardSpace;
using Control;
using GameEnum;
using System.Collections.Generic;
using System.Linq;
using Thread;
using UnityEngine;
namespace Info
{
    public class SingleRowInfo : MonoBehaviour
    {
        public Color color;
        public Card TempCard;
        public Belong belong;
        public RegionTypes region;
        public bool CanBeSelected;
        public RowControl Control ;
        public Material CardMaterial;
        public int RowRank => RowsInfo.GlobalCardList.IndexOf(ThisRowCards);
        public int Location => this.JudgeRank(GlobalBattleInfo.FocusPoint);
        public List<Card> ThisRowCards => belong == Belong.My ? RowsInfo.GetDownCardList(region) : RowsInfo.GetUpCardList(region);
        private void Awake()
        {
            RowsInfo.SingleRowInfos.Add(this);
            Control = GetComponent<RowControl>();
            CardMaterial= transform.GetComponent<Renderer>().material;
        }

        public  void SetRegionSelectable(bool CanBeSelected)
        {
            this.CanBeSelected = CanBeSelected;
            MainThread.Run(() =>
            {
                CardMaterial.SetColor("_GlossColor", CanBeSelected ? color : Color.black);
            });

        }
    }
   
}
static partial class RowInfoExtend
{
    public static int JudgeRank(this Info.SingleRowInfo SingleInfo, Vector3 point)
    {
        int Rank = 0;
        float posx = -(point.x - SingleInfo.transform.position.x);
        int UniteNum = SingleInfo.ThisRowCards.Where(card => !card.IsGray).Count();
        for (int i = 0; i < UniteNum; i++)
        {
            if (posx > i * 1.6 - (UniteNum - 1) * 0.8)
            {
                Rank = i + 1;
            }
        }
        return Rank;
    }
}