using Command;
using System.Threading.Tasks;
using UnityEngine;
namespace Control
{
    public class StateControl : MonoBehaviour
    {

        bool IsLastPlay1Pass = false;
        bool IsLastPlay2Pass = false;
        void Start()
        {
            _ = BattleProcess();
            //PlayerSurrender().Catch();
        }
        private void Update()
        {
            CheckPassState();
        }
        public async Task BattleProcess()
        {
            await StateCommand.BattleStart();
            for (int i = 0; i < 3; i++)
            {
                print("小局开始");
                await StateCommand.RoundStart(i);
                while (true)
                {
                    print("回合开始");
                    await StateCommand.TurnStart();
                    await StateCommand.WaitForPlayerOperation();
                    await StateCommand.TurnEnd();
                    if (Info.GlobalBattleInfo.IsBoothPass) { break; }
                }
                await StateCommand.RoundEnd(i);
            }
            await StateCommand.BattleEnd();
        }
        private void CheckPassState()
        {
            //当pass状态发生改变时
            if (IsLastPlay1Pass ^ Info.GlobalBattleInfo.IsPlayer1Pass)
            {
                IsLastPlay1Pass = Info.GlobalBattleInfo.IsPlayer1Pass;
                StateCommand.SetPassState(Info.GlobalBattleInfo.IsPlayer1, Info.GlobalBattleInfo.IsPlayer1Pass);
            }
            if (IsLastPlay2Pass ^ Info.GlobalBattleInfo.IsPlayer2Pass)
            {
                IsLastPlay2Pass = Info.GlobalBattleInfo.IsPlayer2Pass;
                StateCommand.SetPassState(!Info.GlobalBattleInfo.IsPlayer1, Info.GlobalBattleInfo.IsPlayer2Pass);
            }
        }
        //public async Task PlayerSurrender()
        //{
        //    await StateCommand.Surrender();
        //}
    }

}
