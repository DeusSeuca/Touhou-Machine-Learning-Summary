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
    public static string TransTag(this string text)
    {
        tagCsvData = File.ReadAllLines("Assets\\Resources\\CardData\\Tag.csv", Encoding.GetEncoding("gb2312"));
        return GetCsvData(tagCsvData, text);
    }
    private static string GetCsvData(string[] CsvData, string text)
    {
        //默认中文列位置
        int defaultRank = CsvData[0].Split(',').ToList().IndexOf("Ch");
        //目标语言列位置
        int columnRank = CsvData[0].Split(',').ToList().IndexOf(currentLanguage);
        //目标语言行位置
        int rowRank = CsvData.ToList().IndexOf(CsvData.First(data => data.Split(',')[defaultRank] == text));
        string translateText = CsvData[rowRank].Split(',')[columnRank];
        return translateText == "" ? text : translateText;
    }
}

