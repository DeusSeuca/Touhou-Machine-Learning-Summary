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
            if ((Info.AgainstInfo.IsPlayer1Pass || Info.AgainstInfo.IsPlayer2Pass)&&PointInfo.TotalDownPoint< PointInfo.TotalUpPoint)
            {
                Command.GameUI.UiCommand.SetCurrentPass();
            }
            else
            {
                //await CardCommand.DisCard(AgainstInfo.cardSet[Orientation.My][RegionTypes.Hand].cardList[0]);
                await CardCommand.PlayCard(AgainstInfo.cardSet[Orientation.My][RegionTypes.Hand].cardList[0]);
            }
        }
        //临时的ai操作
        public static async Task TempOperationDiscard()
        {
            if (Info.AgainstInfo.IsPlayer1Pass || Info.AgainstInfo.IsPlayer2Pass)
            {
                Command.GameUI.UiCommand.SetCurrentPass();
            }
            else
            {
                await CardCommand.DisCard(AgainstInfo.cardSet[Orientation.My][ RegionTypes.Hand].cardList[0]);
            }
        }
    }
}

