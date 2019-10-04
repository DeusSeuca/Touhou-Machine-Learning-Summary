using GameEnum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
public static class Extension
{
    public static string ToJson(this object target) => JsonConvert.SerializeObject(target);
    public static T ToObject<T>(this string Data) => JsonConvert.DeserializeObject<T>(Data);
    public static bool Contain(this LoadRangeOnBattle a, LoadRangeOnBattle b) => (a & b) > 0;
    public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action) => enumerable.ToList().ForEach(action);

}

