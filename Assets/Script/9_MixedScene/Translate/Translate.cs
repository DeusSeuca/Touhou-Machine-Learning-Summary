using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


static class Translate
{
    public static string currentLanguage = "Ch";
    static string[] tagCsvData;
    /// <summary>
    /// 根据中文调取对应语言的tag
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string TransTag(this string text)
    {
        tagCsvData = File.ReadAllLines("Assets\\Resources\\CardData\\Tag.csv", Encoding.GetEncoding("gb2312"));
        return GetCsvData(tagCsvData, text,"Ch");
    }
    /// <summary>
    /// 根据枚举体调取对应语言的tag
    /// </summary>
    /// <param name="cardTag"></param>
    /// <returns></returns>
    public static string TransTag(this GameEnum.CardTag cardTag)
    {
        tagCsvData = File.ReadAllLines("Assets\\Resources\\CardData\\Tag.csv", Encoding.GetEncoding("gb2312"));
        return GetCsvData(tagCsvData, cardTag.ToString(), "En");
    }
    public static string TransUiText(this string text)
    {
        tagCsvData = File.ReadAllLines("Assets\\Resources\\CardData\\UiText.csv", Encoding.GetEncoding("gb2312"));
        return GetCsvData(tagCsvData, text);
    }
    private static string GetCsvData(string[] CsvData, string text,string defaultLanguage="Ch")
    {
        //默认中文列位置
        int defaultRank = CsvData[0].Split(',').ToList().IndexOf(defaultLanguage);
        //目标语言列位置
        int columnRank = CsvData[0].Split(',').ToList().IndexOf(currentLanguage);
        //目标语言行位置
        int rowRank = CsvData.ToList().IndexOf(CsvData.First(data => data.Split(',')[defaultRank] == text));
        string translateText = CsvData[rowRank].Split(',')[columnRank];
        return translateText == "" ? text : translateText;
    }
}

