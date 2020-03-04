using Command;
using System.Threading.Tasks;
using UnityEngine;
namespace Control
{
    public class StateControl : MonoBehaviour
    {
        void Start() => _ = BattleProcess();
        private void OnApplicationQuit() => Info.StateInfo.TaskManager.Cancel();
        public async Task BattleProcess()
        {
            
            await StateCommand.BattleStart();
            for (int i = 0; i < 3; i++)
            {
                await StateCommand.RoundStart(i);
                //await StateCommand.WaitForSelectProperty();
                while (true)
                {
                    await StateCommand.TurnStart();
                    await StateCommand.WaitForPlayerOperation();
                    await StateCommand.TurnEnd();
                    if (Info.AgainstInfo.isBoothPass) { break; }
                }
                await StateCommand.RoundEnd(i);
            }
            await StateCommand.BattleEnd();
            Debug.Log("结束对局");
        }
    }
}
