using UnityEngine;

namespace GameUI
{
    public class BoardCardInfo : MonoBehaviour
    {
        public int Rank;
        public void OnMouseClick()
        {
            if (Info.AgainstInfo.selectBoardCardRanks.Contains(Rank))
            {
                Info.AgainstInfo.selectBoardCardRanks.Remove(Rank);
            }
            else
            {
                Info.AgainstInfo.selectBoardCardRanks.Add(Rank);
            }
        }
    }
}