using Command;
using Sirenix.OdinInspector;
using System.Threading.Tasks;
using UnityEngine;
namespace Control
{
    public class StateControl : MonoBehaviour
    {
        Task currentTask;
        void Start() => currentTask = BattleProcess();
        private void OnApplicationQuit() => Info.StateInfo.TaskManager.Cancel();
        [Button("打印线程异常")]//没卵用
        public void ShowMessage()
        {
            try
            {
                currentTask.Wait();
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }
        }
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
                    if (Info.AgainstInfo.isBoothPass) { break; }
                    await StateCommand.TurnEnd();
                }
                await StateCommand.RoundEnd(i);
            }
            await StateCommand.BattleEnd();
            Debug.Log("结束对局");
        }
    }
}
