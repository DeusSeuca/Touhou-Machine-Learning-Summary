using CardSpace;
using Control;
using GameEnum;
using System.Collections.Generic;
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
        public RowControl Control;
        public Material CardMaterial;
        public int RowRank => RowsInfo.GlobalCardList.IndexOf(ThisRowCards);
        public int Location => this.JudgeRank(AgainstInfo.FocusPoint);
        public List<Card> ThisRowCards => belong == Belong.My ? RowsInfo.GetDownCardList(region) : RowsInfo.GetUpCardList(region);
        private void Awake()
        {
            RowsInfo.SingleRowInfos.Add(this);
            Control = GetComponent<RowControl>();
            CardMaterial = transform.GetComponent<Renderer>().material;
        }

        public void SetRegionSelectable(bool CanBeSelected)
        {
            this.CanBeSelected = CanBeSelected;
            MainThread.Run(() =>
            {
                CardMaterial.SetColor("_GlossColor", CanBeSelected ? color : Color.black);
            });
        }
    }
}
