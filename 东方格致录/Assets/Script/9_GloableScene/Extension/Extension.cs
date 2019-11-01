using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Extension
{
    public static class OtherExtension
    {
        public static string ToJson(this object target) => JsonConvert.SerializeObject(target);
        public static T ToObject<T>(this string Data) => JsonConvert.DeserializeObject<T>(Data);
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action) => enumerable.ToList().ForEach(action);
    }
}


