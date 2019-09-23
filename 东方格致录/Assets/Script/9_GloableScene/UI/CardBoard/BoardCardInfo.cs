using UnityEngine;

namespace GameUI
{
    public class BoardCardInfo: MonoBehaviour
    {
        public int Rank;
        public void OnMouseClick()
        {
            if (Info.GlobalBattleInfo.SelectBoardCardIds.Contains(Rank))
            {
                Info.GlobalBattleInfo.SelectBoardCardIds.Remove(Rank);
            }
            else
            {
                Info.GlobalBattleInfo.SelectBoardCardIds.Add(Rank);
            }
        }
    }
}