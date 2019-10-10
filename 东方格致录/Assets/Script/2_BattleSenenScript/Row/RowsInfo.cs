using CardSpace;
using GameEnum;
using Network;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;

namespace Info
{
    public class RowsInfo : SerializedMonoBehaviour
    {
        void Awake() => Init();
        public static RowsInfo Instance;
        [ShowInInspector]
        public static List<List<Card>> globalCardList = new List<List<Card>>();
        public static List<SingleRowInfo> singleRowInfos = new List<SingleRowInfo>();
        public void Init()
        {
            Instance = this;
            globalCardList.Clear();
            for (int i = 0; i < 18; i++)
            {
                globalCardList.Add(new List<Card>());
            }
        }
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
        public static NetInfoModel.Location GetLocation(Card TargetCard)
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
        public static Card GetCard(NetInfoModel.Location Locat) => Locat.x == -1 ? null : globalCardList[Locat.x][Locat.y];
        public static SingleRowInfo GetSingleRowInfoById(int Id) => singleRowInfos.First(infos => infos.ThisRowCards == globalCardList[Id]);
    }
}