using CardSpace;
using Extension;
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
        private void Awake() => RowsInfo.singleRowInfos.Add(this);
        public int Location => this.JudgeRank(AgainstInfo.FocusPoint);
        public int RowRank => RowsInfo.globalCardList.IndexOf(ThisRowCards);
        public Material CardMaterial => transform.GetComponent<Renderer>().material;
        public List<Card> ThisRowCards => AgainstInfo.AllCardList.InRogin(orientation,region);
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
