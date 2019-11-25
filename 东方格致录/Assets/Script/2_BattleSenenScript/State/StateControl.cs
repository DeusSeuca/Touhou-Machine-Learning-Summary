using Command;
using System.Threading.Tasks;
using UnityEngine;
namespace Control
{
    public class StateControl : MonoBehaviour
    {

        void Start() => _ = BattleProcess();

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
            Debug.Log("完全中了");
        }
    }
}
