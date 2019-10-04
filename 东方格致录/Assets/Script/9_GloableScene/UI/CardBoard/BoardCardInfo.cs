using UnityEngine;

namespace GameUI
{
    public class BoardCardInfo: MonoBehaviour
    {
        public int Rank;
        public void OnMouseClick()
        {
            if (Info.AgainstInfo.SelectBoardCardIds.Contains(Rank))
            {
                Info.AgainstInfo.SelectBoardCardIds.Remove(Rank);
            }
            else
            {
                Info.AgainstInfo.SelectBoardCardIds.Add(Rank);
            }
        }
    }
}