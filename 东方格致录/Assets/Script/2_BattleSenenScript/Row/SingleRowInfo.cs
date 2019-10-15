using CardSpace;
using Extension;
using GameEnum;
using Sirenix.OdinInspector;
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
        [ShowInInspector]
        public int rank => (int)region + (AgainstInfo.IsPlayer1 ^ (orientation == Orientation.Down) ? 9 : 0);
        private void Awake() => RowsInfo.singleRowInfos.Add(this);
        public int Location => this.JudgeRank(AgainstInfo.FocusPoint);
        public int RowRank => RowsInfo.globalCardList.IndexOf(ThisRowCards);
        public Material CardMaterial => transform.GetComponent<Renderer>().material;
        public List<Card> ThisRowCards => RowsInfo.globalCardList[rank];
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
