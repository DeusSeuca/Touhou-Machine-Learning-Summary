using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 内容测试
{
    class Program
    {
        //class Countdown : Attribute { }
        //class Unite { }
        //class UniteA : Unite
        //{
        //    [Countdown]
        //    public int Countdown = 3;
        //    public void Effect() => Console.WriteLine($"当前计数为{Countdown--}");
        //}
        //class UniteB : Unite
        //{
        //    public void Effect(Unite unite)
        //    {
        //        unite.GetType().GetFields().First(x => x.GetCustomAttributes(true)[0].GetType() == typeof(Countdown)).SetValue(unite, 9);
        //        Console.WriteLine($"将计数改为⑨");
        //    }
        //}
        static void Main(string[] args)
        {


            string language = "Ch";
            datas = File.ReadAllLines(@"E:\东方格致录\东方格致录\Assets\Resources\CardData.csv", Encoding.GetEncoding("gb2312"));
            datas.ToList().ForEach(Console.WriteLine);
            for (int i = 1; i < datas.Length; i++)
            {
                GetItem(i, $"Name-{language}").ToShow("名字：");
                GetItem(i, $"Describe-{language}").ToShow("描述：");
                GetItem(i, "Point", true).ToShow("数值：");
                ((Sectarian)GetItem(i, "Camp", true)).ToShow("阵营：");
                Console.WriteLine();
            }

            while (true) { }
        }
        static string[] datas;

        public static int GetIndex(string Item)
        {
            return datas[0].Split(',').ToList().IndexOf(Item);
        }
        public static string GetItem(int id, string Item)
        {
            var s = datas[id].Split(',');
            var e = GetIndex(Item);
            return datas[id].Split(',')[GetIndex(Item)];
        }
        public static int GetItem(int id, string Item, bool TransInt)
        {
            var s = GetIndex(Item);
            return int.Parse(datas[id].Split(',')[GetIndex(Item)]);
        }
        public void CardModelInfo(string icon, string cardId, string cardName, string point, string sectarian)
        {
            //Icon = icon;
            //CardId = cardId;
            //CardName = cardName;
            //Point = point;
            //this.sectarian = sectarian;
        }
        public enum Sectarian
        {
            道教,
            神道教,
            佛教,
            中立
        }
    }
    static class Extern
    {
        public static void ToShow(this object Target, string Title = "")
        {
            Console.Write(Title + Target.ToString() + " ");
        }
    }
}
