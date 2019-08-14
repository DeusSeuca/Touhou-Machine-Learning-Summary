using UnityEngine;

public class BoardCardInfo : MonoBehaviour
{
    public int Rank;
    public void OnMouseClick()
    {
        if (Info.GlobalBattleInfo.SelectBoardCardIds.Contains(Rank))
        {
            Info.GlobalBattleInfo.SelectBoardCardIds.Remove(Rank);
            print("移除了一张牌");
        }
        else
        {
            Info.GlobalBattleInfo.SelectBoardCardIds.Add(Rank);
            //print("加入了一张牌");
        }
    }
}
