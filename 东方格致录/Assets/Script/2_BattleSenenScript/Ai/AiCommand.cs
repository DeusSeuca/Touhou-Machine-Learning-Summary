using Extension;
using GameEnum;
using Info;
using System;
using System.Threading.Tasks;
namespace Command
{
    public class AiCommand
    {
        static Random rand = new Random("gezi".GetHashCode());
        public static void Init() => rand = new Random("gezi".GetHashCode());
        public static int GetRandom(int Min, int Max) => rand.Next(Min, Max);
        public static async Task TempOperationPlayCard()
        {
            if ((Info.AgainstInfo.IsDownPass) && PointInfo.TotalDownPoint < PointInfo.TotalUpPoint)
            {
                GameUI.UiCommand.SetCurrentPass();
            }
            else
            {
                await CardCommand.PlayCard(AgainstInfo.cardSet[Orientation.My][RegionTypes.Hand].cardList[0]);
            }
        }
        //临时的ai操作
        public static async Task TempOperationDiscard()
        {
            if (Info.AgainstInfo.IsDownPass || Info.AgainstInfo.IsUpPass)
            {
                Command.GameUI.UiCommand.SetCurrentPass();
            }
            else
            {
                await CardCommand.DisCard(AgainstInfo.cardSet[Orientation.My][RegionTypes.Hand].cardList[0]);
            }
        }
    }
}

