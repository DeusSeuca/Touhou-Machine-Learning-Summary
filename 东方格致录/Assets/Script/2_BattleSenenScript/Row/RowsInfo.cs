using CardSpace;
using Extension;
using GameEnum;
using Network;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;

namespace Info
{
    public class RowsInfo : SerializedMonoBehaviour
    {
        void Awake() => Command.RowCommand.InitRowsInfo();
        [ShowInInspector]
        public static List<List<Card>> globalCardList = new List<List<Card>>();
        public static List<SingleRowInfo> singleRowInfos = new List<SingleRowInfo>();
        public List<SingleRowInfo> this[RegionTypes region] => singleRowInfos.InRogin(region);
        public List<SingleRowInfo> this[Orientation orientation] => singleRowInfos.At(orientation);
        public static List<Card> GetRow(Card TargetCard) => globalCardList.First(list => list.Contains(TargetCard));//List<Card> TargetCardList = null;//foreach (var cardlist in globalCardList)//{//    if (cardlist.Contains(TargetCard))//    {//        TargetCardList = cardlist;//    }//}//return TargetCardList;
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
            return new NetInfoModel.Location(RankX, RankY);
        }
        public static Card GetCard(int x, int y) => x == -1 ? null : globalCardList[x][y];
        public static Card GetCard(NetInfoModel.Location Locat) => Locat.x == -1 ? null : globalCardList[Locat.x][Locat.y];
        public static SingleRowInfo GetSingleRowInfoById(int Id) => singleRowInfos.First(infos => infos.ThisRowCards == globalCardList[Id]);
    }
}