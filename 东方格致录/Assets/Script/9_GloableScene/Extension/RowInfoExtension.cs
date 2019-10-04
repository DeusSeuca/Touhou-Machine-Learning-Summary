using System.Linq;
using UnityEngine;

static partial class RowInfoExtension
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