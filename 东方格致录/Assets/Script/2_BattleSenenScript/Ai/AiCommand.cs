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
        //临时的ai操作
        public static async Task TempOperationAsync()
        {
            if (Info.GlobalBattleInfo.IsPlayer1Pass || Info.GlobalBattleInfo.IsPlayer2Pass)
            {
                Command.GameUI.UiCommand.SetCurrentPass();
            }
            else
            {
                await CardCommand.DisCard(Info.RowsInfo.GetMyCardList(RegionTypes.Hand)[0]);
            }
        }
    }
}

