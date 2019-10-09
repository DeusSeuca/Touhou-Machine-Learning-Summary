
using CardSpace;
using GameEnum;
using Info;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CardListExtension
{
    public static int GetRowRank(this List<Card> cardList) => Info.RowsInfo.globalCardList.IndexOf(cardList);
    public static void Trigger<T>(this List<Card> cardList) where T : Attribute => cardList.ForEach(card => card.Trigger<T>());
    //待修正
    public static List<Card> HasTag(this List<Card> cardList, params string[] tags)
    {
        cardList.Where(card => (card.tag.Contains(',') ? card.tag.Split(',') : new string[] { card.tag }).Intersect(tags).Any()).ToList().ForEach(x => Debug.Log("符合卡片有" + x.name));
        return cardList.Where(card => (card.tag.Contains(',') ? card.tag.Split(',') : new string[] { card.tag }).Intersect(tags).Any()).ToList();
        //.ForEach(x => Debug.Log("符合卡片有" + x.name)
        //return cardList.Where(card => card.tag.Split(',').Intersect(tags).Count() > 0).ToList();
    }
    public static List<Card> At(this List<Card> cardList, Orientation orientation)
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
            default:
                return null;
        }
    }

    //public static List<Card> Belong(this List<Card> cardList, GameEnum.Belong belong) => cardList.Where(card => card.belong == belong).ToList();
    //返回所属区域的卡片集合
    public static List<Card> InRogin(this List<Card> cardList, params RegionTypes[] regions)
    {
        return Info.AgainstInfo.AllRegionList.InRogin(regions).SelectMany(x => x.ThisRowCards).Intersect(cardList).ToList();
    }
    //待修正
    public static List<Card> HasValue(this List<Card> cardList, params string[] tags) => null;
    public static List<Card> LessPoint(this List<Card> cardList, int point) => cardList.Where(card => card.CardPoint < point).ToList();
    public static List<Card> MmorePoint(this List<Card> cardList, int point) => cardList.Where(card => card.CardPoint > point).ToList();
    public static List<Card> EqualPoint(this List<Card> cardList, int point) => cardList.Where(card => card.CardPoint == point).ToList();

}
public static class RegionExtension
{
    public static List<SingleRowInfo> InRogin(this List<SingleRowInfo> rows, params RegionTypes[] regions)
    {
        List<SingleRowInfo> targetRows;
        if (regions.Contains(RegionTypes.Battle))
        {
            targetRows = rows.Where(row =>
             row.region == RegionTypes.Water ||
             row.region == RegionTypes.Fire ||
             row.region == RegionTypes.Wind ||
             row.region == RegionTypes.Soil
            ).ToList();
        }
        else
        {
            targetRows = rows.Where(row => regions.Contains(row.region)).ToList();
        }
        return targetRows;
    }
    public static List<SingleRowInfo> At(this List<SingleRowInfo> rows, GameEnum.Orientation orientation)
    {
        return rows.Where(row => row.orientation == orientation).ToList(); ;
    }
}
