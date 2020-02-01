using CardModel;
using Extension;
using GameEnum;

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
            if ((Info.AgainstInfo.isDownPass && Info.PointInfo.TotalDownPoint < Info.PointInfo.TotalUpPoint) ||
                Info.AgainstInfo.cardSet[Orientation.My][RegionTypes.Hand].cardList.Count == 0)
            {
                GameUI.UiCommand.SetCurrentPass();
            }
            else
            {

                Card targetCard = Info.AgainstInfo.cardSet[Orientation.My][RegionTypes.Hand].cardList[0];
                await GameSystem.TransSystem.PlayCard(TriggerInfo.Build(null, targetCard));
                //await CardCommand.PlayCard(targetCard);
            }
        }
        //临时的ai操作
        public static async Task TempOperationDiscard()
        {
            if (Info.AgainstInfo.isDownPass || Info.AgainstInfo.isUpPass)
            {
                Command.GameUI.UiCommand.SetCurrentPass();
            }
            else
            {
                await CardCommand.DisCard(Info.AgainstInfo.cardSet[Orientation.My][RegionTypes.Hand].cardList[0]);
            }
        }
    }
}

