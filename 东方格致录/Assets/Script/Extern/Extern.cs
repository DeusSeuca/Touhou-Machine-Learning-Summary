
using CardSpace;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static GameEnum;

public static class Extern
{
    public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
        enumerable.ToList().ForEach(action);
    }

    public static bool Contain(this LoadRangeOnBattle a, LoadRangeOnBattle b)
    {
        return (a & b) > 0;
    }

}

