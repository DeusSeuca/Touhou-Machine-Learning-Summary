using Command;
using System.Threading.Tasks;
using UnityEngine;
namespace Control
{
    public class StateControl : MonoBehaviour
    {
        //bool IsLastPlay1Pass = false;
        //bool IsLastPlay2Pass = false;
        void Start() =>  _=BattleProcess();//PlayerSurrender().Catch();
        //private void Update()
        //{
        //    //CheckPassState();
        //}
        public async Task BattleProcess()
        {
            await StateCommand.BattleStart();
            for (int i = 0; i < 3; i++)
            {
                await StateCommand.RoundStart(i);
                while (true)
                {
                    await StateCommand.TurnStart();
                    await StateCommand.WaitForPlayerOperation();
                    await StateCommand.TurnEnd();
                    if (Info.AgainstInfo.IsBoothPass) { break; }
                }
                await StateCommand.RoundEnd(i);
            }
            await StateCommand.BattleEnd();
        }
        //private void CheckPassState()
        //{
        //    //当pass状态发生改变时
        //    if (IsLastPlay1Pass ^ Info.AgainstInfo.IsPlayer1Pass)
        //    {
        //        IsLastPlay1Pass = Info.AgainstInfo.IsPlayer1Pass;
        //        StateCommand.SetPassState(Info.AgainstInfo.IsPlayer1, Info.AgainstInfo.IsPlayer1Pass);
        //    }
        //    if (IsLastPlay2Pass ^ Info.AgainstInfo.IsPlayer2Pass)
        //    {
        //        IsLastPlay2Pass = Info.AgainstInfo.IsPlayer2Pass;
        //        StateCommand.SetPassState(!Info.AgainstInfo.IsPlayer1, Info.AgainstInfo.IsPlayer2Pass);
        //    }
        //}
    }
}
