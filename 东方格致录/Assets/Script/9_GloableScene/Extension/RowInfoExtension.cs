﻿using System.Linq;
using UnityEngine;
namespace Extension
{
    static partial class RowInfoExtension
    {
        //public static void OrderCard(this Info.SingleRowInfo singleInfo)
        //{
        //    singleInfo.ThisRowCards = singleInfo.ThisRowCards.OrderBy(card => card.point);
        //}
        public static int JudgeRank(this Info.SingleRowInfo singleRowInfo, Vector3 point)
        {
            int Rank = 0;
            float posx = -(point.x - singleRowInfo.transform.position.x);
            int UniteNum = singleRowInfo.ThisRowCards.Where(card => !card.IsGray).Count();
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
}