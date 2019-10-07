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
        //使用 Orientation 代替 Belong
        public Belong belong;
        public Orientation orientation;
        public RegionTypes region;
        public bool CanBeSelected;
        public RowControl Control;
        public Material CardMaterial;
        public int RowRank => RowsInfo.globalCardList.IndexOf(ThisRowCards);
        public int Location => this.JudgeRank(AgainstInfo.FocusPoint);
        public List<Card> ThisRowCards => belong == Belong.My ? RowsInfo.GetDownCardList(region) : RowsInfo.GetUpCardList(region);
        private void Awake()
        {
            RowsInfo.singleRowInfos.Add(this);
            Control = GetComponent<RowControl>();
            CardMaterial = transform.GetComponent<Renderer>().material;
        }

        public void SetRegionSelectable(bool CanBeSelected)
        {
            this.CanBeSelected = CanBeSelected;
            MainThread.Run(() =>
            {
                print(name + "设置为" + (CanBeSelected ? color : Color.black));
                CardMaterial.SetColor("_GlossColor", CanBeSelected ? color : Color.black);
            });
        }
    }
}
