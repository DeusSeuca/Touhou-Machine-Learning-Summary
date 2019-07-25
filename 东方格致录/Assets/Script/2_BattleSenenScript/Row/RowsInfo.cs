﻿using CardSpace;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Info
{
    public class RowsInfo : SerializedMonoBehaviour
    {
        [ShowInInspector]
        public static List<List<Card>> GlobalCardList = new List<List<Card>>();
        public static List<SingleRowInfo> SingleRowInfos = new List<SingleRowInfo>();
        public static SingleRowInfo SelectSingleRowInfos(int RowRank) => SingleRowInfos.First(infos => infos.ThisRowCard == GlobalCardList[RowRank]);

        public static RowsInfo Instance;

        public Dictionary<RegionName_Battle, SingleRowInfo> SingleBattleInfos = new Dictionary<RegionName_Battle, SingleRowInfo>();
        public Dictionary<RegionName_Other, SingleRowInfo> SingleOtherInfos = new Dictionary<RegionName_Other, SingleRowInfo>();
        void Awake() => Init();
        public void Init()
        {
            Instance = this;
            GlobalCardList.Clear();
            for (int i = 0; i < 18; i++)
            {
                GlobalCardList.Add(new List<Card>());
            }
        }
        /// <summary>
        /// 获取上方玩家手牌卡组
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<Card> GetUpCardList(RegionTypes type) => GlobalCardList[(int)type + (GlobalBattleInfo.IsPlayer1 ? 9 : 0)];
        /// <summary>
        /// 获取下方玩家手牌卡组
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<Card> GetDownCardList(RegionTypes type) => GlobalCardList[(int)type + (GlobalBattleInfo.IsPlayer1 ? 0 : 9)];
        /// <summary>
        /// 获取相对于当前回合的我方卡组
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<Card> GetMyCardList(RegionTypes type) => GlobalBattleInfo.IsMyTurn ? GetDownCardList(type) : GetUpCardList(type);
        /// <summary>
        /// 获取相对于当前回合的敌方卡组
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<Card> GetOpCardList(RegionTypes type) => GlobalBattleInfo.IsMyTurn ? GetUpCardList(type) : GetDownCardList(type);

        public static SingleRowInfo GetRegionCardList(RegionName_Battle region) => Instance.SingleBattleInfos[region];

        public static SingleRowInfo GetRegionCardList(RegionName_Other region) => Instance.SingleOtherInfos[region];

        public static Vector2 GetLocation(Card TargetCard)
        {
            int RankX = -1;
            int RankY = -1;
            for (int i = 0; i < GlobalCardList.Count; i++)
            {
                if (GlobalCardList[i].Contains(TargetCard))
                {
                    RankX = i;
                    RankY = GlobalCardList[i].IndexOf(TargetCard);
                }
            }
            return new Vector2(RankX, RankY);
        }
        public static Card GetCard((int, int) TargetLocation)
        {
            return TargetLocation.Item1 == -1 ? null : GlobalCardList[(int)TargetLocation.Item1][(int)TargetLocation.Item2];
        }
        public static List<Card> GetRow(Card TargetCard)
        {
            List<Card> TargetCardList = null;
            foreach (var cardlist in GlobalCardList)
            {
                if (cardlist.Contains(TargetCard))
                {
                    TargetCardList = cardlist;
                }
            }
            return TargetCardList;
        }
    }
}

