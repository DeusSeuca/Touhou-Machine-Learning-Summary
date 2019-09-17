using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
namespace Command
{
    public class CardLibraryCommand
    {
        static string[] CsvData => File.ReadAllLines("Assets\\Resources\\CardData.csv", Encoding.GetEncoding("gb2312"));
        public static void CreatScript(int id)
        {
            string OriginPath = Application.dataPath + @"\Script\9_GloableScene\Card\CardLibrary\CardModel\Card0.cs";
            string NewPath = Application.dataPath + $@"\Script\9_GloableScene\Card\CardLibrary\CardModel\Card{id}.cs";
            string ScriptText = File.ReadAllText(OriginPath).Replace("Card0", "Card" + id);
            File.Create(NewPath).Close();
            File.WriteAllText(NewPath, ScriptText);
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }
        public static void GetCardInfo(int id)
        {

        }
        public static void ClearCardLibrary(int id)
        {

        }
        public static void LoadFromCsv()
        {
            CardLibrarySaveData.cards = new System.Collections.Generic.List<CardModelInfo>();
            Debug.Log(CsvData[1]);
            //TextAsset binAsset = Resources.Load("csv", typeof(TextAsset)) as TextAsset;
            //var s = Resources.Load("CardData.csv").ToJson();
            string Language = "Ch";

            for (int i = 1; i < CsvData.Length; i++)
            {
                //Texture2D tex = new Texture2D().LoadImage(File.ReadAllBytes(GetCsvData<string>(i, "ImageUrl") + ".png"));
                Texture2D tex = Resources.Load<Texture2D>("CardTex\\" + GetCsvData<string>(i, "ImageUrl"));
                Debug.Log("CardTex\\" + GetCsvData<string>(i, "ImageUrl"));
                CardLibrarySaveData.cards.Add(
                    //new CardModelInfo(
                    //    new Texture2D(4, 10),
                    //    GetCsvData<int>(i, "Id"),
                    //    GetCsvData<string>(i, "Name-" + Language),
                    //    GetCsvData<int>(i, "Point"),
                    //    GetCsvData<Sectarian>(i, "Camp")
                    //    ));
                    new CardModelInfo(
                        GetCsvData<int>(i, "Id"),
                        GetCsvData<string>(i, "Name-" + Language),
                        GetCsvData<string>(i, "Describe-" + Language),
                        GetCsvData<string>(i, "Tag-" + Language),
                        GetCsvData<Sectarian>(i, "Camp"),
                        GetCsvData<CardLevel>(i, "Level"),
                        GetCsvData<int>(i, "Point"),
                        GetCsvData<int>(i, "RamificationRank"),
                        tex
                    ));
            }
            //Debug.Log(text);
        }

        private static T GetCsvData<T>(int i, string item)
        {
            int rank = CsvData[0].Split(',').ToList().IndexOf(item);
            try
            {
                return (T)Convert.ChangeType(CsvData[i].Split(',')[rank], typeof(T).IsEnum ? typeof(int) : typeof(T));

            }
            catch (Exception)
            {
                Debug.Log(item);
                Debug.Log(CsvData[i].Split(',')[rank]);
                Debug.Log(typeof(T));
                return (T)Convert.ChangeType(CsvData[i].Split(',')[rank], typeof(T).IsEnum ? typeof(int) : typeof(T));
            }
        }

        public static void SaveToCsv()
        {
            CardLibrarySaveData.cards.Clear();
            //string[] datas = File.ReadAllLines(@"E:\东方格致录\东方格致录\Assets\Resources\CardData.csv", Encoding.GetEncoding("gb2312"));
            //datas.ToList().ForEach(Debug.Log);
        }
    }
}