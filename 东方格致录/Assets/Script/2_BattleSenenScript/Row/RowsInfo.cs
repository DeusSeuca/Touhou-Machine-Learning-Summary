using CardSpace;
using GameEnum;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;

namespace Info
{
    public class RowsInfo : SerializedMonoBehaviour
    {
        [ShowInInspector]
        public static List<List<Card>> globalCardList = new List<List<Card>>();
        public static List<SingleRowInfo> singleRowInfos = new List<SingleRowInfo>();
        public static SingleRowInfo GetSingleRowInfoById(int Id) => singleRowInfos.First(infos => infos.ThisRowCards == globalCardList[Id]);

        public static RowsInfo Instance;

        public Dictionary<RegionName_Battle, SingleRowInfo> singleBattleInfos = new Dictionary<RegionName_Battle, SingleRowInfo>();
        public Dictionary<RegionName_Other, SingleRowInfo> singleOtherInfos = new Dictionary<RegionName_Other, SingleRowInfo>();
        void Awake() => Init();
        public void Init()
        {
            Instance = this;
            globalCardList.Clear();
            for (int i = 0; i < 18; i++)
            {
                globalCardList.Add(new List<Card>());
            }
        }
        /// <summary>
        /// 获取上方玩家手牌卡组
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<Card> GetUpCardList(RegionTypes type) => globalCardList[(int)type + (AgainstInfo.IsPlayer1 ? 9 : 0)];
        /// <summary>
        /// 获取下方玩家手牌卡组
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<Card> GetDownCardList(RegionTypes type) => globalCardList[(int)type + (AgainstInfo.IsPlayer1 ? 0 : 9)];
        /// <summary>
        /// 获取相对于当前回合的我方卡组
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<Card> GetMyCardList(RegionTypes type) => AgainstInfo.IsMyTurn ? GetDownCardList(type) : GetUpCardList(type);
        /// <summary>
        /// 获取相对于当前回合的敌方卡组
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<Card> GetOpCardList(RegionTypes type) => AgainstInfo.IsMyTurn ? GetUpCardList(type) : GetDownCardList(type);
        public static SingleRowInfo GetSingleRowInfo(RegionTypes type, bool InMyTerritory) => singleRowInfos.First(SingleRow => SingleRow.ThisRowCards.Equals(InMyTerritory ? GetMyCardList(type) : GetDownCardList(type)));
        public static Network.NetInfoModel.Location GetLocation(Card TargetCard)
        {
            int RankX = -1;
            int RankY = -1;
            for (int i = 0; i < globalCardList.Count; i++)
            {
                if (globalCardList[i].Contains(TargetCard))
                {
                    RankX = i;
                    RankY = globalCardList[i].IndexOf(TargetCard);
                }
            }
            return new Network.NetInfoModel.Location(RankX, RankY);
        }
        public static Card GetCard(int x, int y) => x == -1 ? null : globalCardList[x][y];
        public static Card GetCard(Network.NetInfoModel.Location Locat) => Locat.x == -1 ? null : globalCardList[Locat.x][Locat.y];
        public static List<Card> GetRow(Card TargetCard)
        {
            List<Card> TargetCardList = null;
            foreach (var cardlist in globalCardList)
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

