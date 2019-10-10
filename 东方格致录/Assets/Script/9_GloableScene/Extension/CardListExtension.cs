
using CardSpace;
using GameEnum;
using Info;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Extension
{
    public static class CardListExtension
    {
        public static int GetRowRank(this List<Card> cardList) => Info.RowsInfo.globalCardList.IndexOf(cardList);
        public static void Trigger<T>(this List<Card> cardList) where T : Attribute => cardList.ForEach(card => card.Trigger<T>());
        public static List<Card> HasTag(this List<Card> cardList, params string[] tags)
        {
            cardList.Where(card => (card.tag.Contains(',') ? card.tag.Split(',') : new string[] { card.tag }).Intersect(tags).Any()).ToList().ForEach(x => Debug.Log("符合卡片有" + x.name));
            return cardList.Where(card => (card.tag.Contains(',') ? card.tag.Split(',') : new string[] { card.tag }).Intersect(tags).Any()).ToList();
        }
        private static List<Card> At(this List<Card> cardList, Orientation orientation)
        {
            switch (orientation)
            {
                case GameEnum.Orientation.Up:
                    return cardList.Where(card => RowsInfo.globalCardList.Skip(AgainstInfo.IsPlayer1 ? 9 : 0).Take(8).SelectMany(x => x).Contains(card)).ToList();
                case GameEnum.Orientation.Down:
                    return cardList.Where(card => RowsInfo.globalCardList.Skip(AgainstInfo.IsPlayer1 ? 0 : 9).Take(8).SelectMany(x => x).Contains(card)).ToList();
                case GameEnum.Orientation.My:
                    return AgainstInfo.IsMyTurn ? cardList.At(Orientation.Down) : cardList.At(Orientation.Up);
                case GameEnum.Orientation.Op:
                    return AgainstInfo.IsMyTurn ? cardList.At(Orientation.Up) : cardList.At(Orientation.Down);
                case Orientation.All:
                    return cardList;
                default:
                    return null;
            }
        }
        //返回所属区域的卡片集合
        public static List<Card> InRogin(this List<Card> cardList, Orientation orientation = Orientation.All, params RegionTypes[] regions)
        {
            return Info.AgainstInfo.AllRegionList.InRogin(regions).SelectMany(x => x.ThisRowCards).Intersect(cardList).ToList().At(orientation);
        }
        //待修正
        public static List<Card> HasValue(this List<Card> cardList, params string[] tags) => null;
        public static List<Card> LessPoint(this List<Card> cardList, int point) => cardList.Where(card => card.point < point).ToList();
        public static List<Card> MmorePoint(this List<Card> cardList, int point) => cardList.Where(card => card.point > point).ToList();
        public static List<Card> EqualPoint(this List<Card> cardList, int point) => cardList.Where(card => card.point == point).ToList();
    }
}
