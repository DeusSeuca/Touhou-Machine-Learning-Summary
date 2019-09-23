using GameEnum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
namespace Command
{
    public class CardLibraryCommand
    {
        public static CardLibrarySaveData GetLibrarySaveData() => Resources.Load<CardLibrarySaveData>("SaveData");


        static string[] CsvData => File.ReadAllLines("Assets\\Resources\\CardData.csv", Encoding.GetEncoding("gb2312"));
        public static void CreatScript(int cardId)
        {
            string targetPath = Application.dataPath + $@"\Script\9_GloableScene\CardSpace\Card{cardId}.cs";

            if (!File.Exists(targetPath))
            {
                string OriginPath = Application.dataPath + @"\Script\9_GloableScene\CardSpace\Card0.cs";
                string ScriptText = File.ReadAllText(OriginPath).Replace("Card0", "Card" + cardId);
                File.Create(targetPath).Close();
                File.WriteAllText(targetPath, ScriptText);
#if UNITY_EDITOR
                AssetDatabase.Refresh();
#endif
            }
        }
        public static CardModelInfo GetCardStandardInfo(int id) => Command.CardLibraryCommand.GetLibrarySaveData().cards.First(info => info.cardId == id);
        public static void LoadFromCsv()
        {
            //TextAsset binAsset = Resources.Load("csv", typeof(TextAsset)) as TextAsset;
            //var s = Resources.Load("CardData.csv").ToJson();
            Command.CardLibraryCommand.GetLibrarySaveData().cards = new List<CardModelInfo>();
            string Language = "Ch";
            for (int i = 1; i < CsvData.Length; i++)
            {
                Texture2D tex = Resources.Load<Texture2D>("CardTex\\" + GetCsvData<string>(i, "ImageUrl"));
                Command.CardLibraryCommand.GetLibrarySaveData().cards.Add(
                    new CardModelInfo(
                        GetCsvData<int>(i, "Id"),
                        GetCsvData<string>(i, "Name-" + Language),
                        GetCsvData<string>(i, "Describe-" + Language),
                        GetCsvData<string>(i, "Tag-" + Language),
                        GetCsvData<Sectarian>(i, "Camp"),
                        GetCsvData<CardLevel>(i, "Level"),
                        GetCsvData<Property>(i, "Property"),
                        GetCsvData<Territory>(i, "Territory"),
                        GetCsvData<int>(i, "Point"),
                        GetCsvData<int>(i, "RamificationRank"),
                        tex
                    ));
            }
        }
        private static T GetCsvData<T>(int i, string item)
        {
            try
            {
                //Debug.Log(i + ":" + item);
                int rank = CsvData[0].Split(',').ToList().IndexOf(item);
                return (T)Convert.ChangeType(CsvData[i].Split(',')[rank], typeof(T).IsEnum ? typeof(int) : typeof(T));
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
                int rank = CsvData[0].Split(',').ToList().IndexOf(item);
                return (T)Convert.ChangeType(CsvData[i].Split(',')[rank], typeof(T).IsEnum ? typeof(int) : typeof(T));
            }

        }
        public static void SaveToCsv()
        {
            Command.CardLibraryCommand.GetLibrarySaveData().cards.Clear();
            //string[] datas = File.ReadAllLines(@"E:\东方格致录\东方格致录\Assets\Resources\CardData.csv", Encoding.GetEncoding("gb2312"));
            //datas.ToList().ForEach(Debug.Log);
        }
    }
}