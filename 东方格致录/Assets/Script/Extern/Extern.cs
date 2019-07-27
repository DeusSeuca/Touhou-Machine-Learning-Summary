﻿
using CardSpace;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
public static class Extern
{
    public static string ToJson(this object target) => JsonConvert.SerializeObject(target);
    public static T ToObject<T>(this string Data) => JsonConvert.DeserializeObject<T>(Data);
    public static int GetRowRank(this List<Card> CardList)
    {
        return Info.RowsInfo.GlobalCardList.IndexOf(CardList);
    }
    public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
        enumerable.ToList().ForEach(action);
    }
    public static bool Contain(this LoadRangeOnBattle a, LoadRangeOnBattle b)
    {
        return (a & b) > 0;
    }

}

