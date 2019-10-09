using CardSpace;
using GameEnum;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Info
{
    /// <summary>
    /// 全局对战信息
    /// </summary>
    public static class AgainstInfo
    {
        //网络同步信息
        public static Card TargetCard;
        public static int RandomRank;
        public static List<int> TargetCardIDList;
        public static List<Card> TargetCardList;
        public static bool IsSelectCardOver;
        public static int RoomID;
        //操作标志位
        public static int LanguageRank;
        public static bool IsCardEffectCompleted;
        public static List<GameObject> ArrowList = new List<GameObject>();

        public static CardBoardMode CardBoardMode;
        public static List<int> Player1BlackCardList;
        public static List<int> Player2BlackCardList;

        public static Vector3 DragToPoint;
        public static Card PlayerFocusCard;
        public static Card OpponentFocusCard;
        public static Card PlayerPlayCard;

        public static SingleRowInfo PlayerFocusRegion;
        public static bool IsWaitForSelectRegion;
        public static SingleRowInfo SelectRegion;
        //选择的单位
        public static Card ArrowStartCard;
        public static Card ArrowEndCard;
        public static bool IsWaitForSelectUnits;
        public static List<Card> SelectUnits = new List<Card>();//玩家选择的单位
        //选择的坐标
        public static Vector3 FocusPoint;
        public static bool IsWaitForSelectLocation;
        public static int SelectLocation = -1;
        //选择的卡牌面板卡片
        public static List<int> SelectBoardCardIds;
        public static bool IsFinishSelectBoardCard;
        public static int ExChangeableCardNum = 0;
        public static bool IsMyTurn = true;
        public static bool IsPVP = false;

        public static List<Card> AllCardList => RowsInfo.globalCardList.SelectMany(x => x).ToList();
        public static List<SingleRowInfo> AllRegionList => RowsInfo.singleRowInfos;

        public static bool IsPlayer1 = true;
        public static (int P1Score, int P2Score) PlayerScore;
        public static (int MyScore, int OpScore) ShowScore => IsPlayer1 ? (PlayerScore.P1Score, PlayerScore.P2Score) : (PlayerScore.P2Score, PlayerScore.P1Score);
        public static bool IsPlayer1Pass;
        public static bool IsPlayer2Pass;


        internal static bool IsBattleEnd;

        public static bool IsCurrectPass => IsPlayer1 ^ IsMyTurn ? IsPlayer2Pass : IsPlayer1Pass;
        public static bool IsBoothPass => IsPlayer1Pass && IsPlayer2Pass;
    };
}