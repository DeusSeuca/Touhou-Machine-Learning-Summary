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

        public Orientation orientation;
        public RegionTypes region;
        public bool CanBeSelected;
        public int RowRank => RowsInfo.globalCardList.IndexOf(ThisRowCards);
        public int Location => this.JudgeRank(AgainstInfo.FocusPoint);
        public Material CardMaterial => transform.GetComponent<Renderer>().material;

        public List<Card> ThisRowCards => orientation == Orientation.Down ? RowsInfo.GetDownCardList(region) : RowsInfo.GetUpCardList(region);
        private void Awake()
        {
            RowsInfo.singleRowInfos.Add(this);
            //CardMaterial = transform.GetComponent<Renderer>().material;
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
