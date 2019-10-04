
using CardSpace;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CardListExtension
{
    public static int GetRowRank(this List<Card> cardList) => Info.RowsInfo.GlobalCardList.IndexOf(cardList);
    public static void Trigger<T>(this List<Card> cardList) where T : Attribute => cardList.ForEach(card => card.Trigger<T>());
    //待修正
    public static List<Card> hasTag(this List<Card> cardList, params string[] tags)
    {
        cardList.Where(card => (card.tag.Contains(',') ? card.tag.Split(',') : new string[] { card.tag }).Intersect(tags).Count() > 0).ToList().ForEach(x => Debug.Log("符合卡片有" + x.name));
        return cardList.Where(card => (card.tag.Contains(',') ? card.tag.Split(',') : new string[] { card.tag }).Intersect(tags).Count() > 0).ToList();
        //.ForEach(x => Debug.Log("符合卡片有" + x.name)
        //return cardList.Where(card => card.tag.Split(',').Intersect(tags).Count() > 0).ToList();
    }
    public static List<Card> Belong(this List<Card> cardList,GameEnum.Belong belong) => cardList.Where(card => card.belong == belong).ToList();

    //待修正
    public static List<Card> hasValue(this List<Card> cardList, params string[] tags) => null;
    public static List<Card> lessPoint(this List<Card> cardList, int point) => null;
    public static List<Card> morePoint(this List<Card> cardList, int point) => null;
}

