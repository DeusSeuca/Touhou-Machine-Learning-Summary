using CardSpace;
using Command;
using System;
using System.Threading.Tasks;
using UnityEngine;

public class Card1000 : Card
{
    [TriggerType.PlayCard]
    public Func<Task> Step1 => (async () => { print("test"); await Task.Delay(1000); });
    [TriggerType.PlayCard]
    public Func<Task> Step2 => (async () =>
    {
        //print("等待选择位置");
        await StateCommand.WaitForSelectLocation();
        //print("开始部署");
        await Deploy();
        //print("部署完成");
        Trigger<TriggerType.Deploy>();
        await Task.Delay(100);
    });
    [TriggerType.PlayCard]
    public Func<Task> Step3 => (async () =>
    {
        await StateCommand.WaitForSelecUnit(this,GameCommand.GetCardList( LoadRangeOnBattle.My_All),2);
        Debug.Log("卡在这步？");
       
        for (int i = 0; i < Info.GlobalBattleInfo.SelectUnits.Count; i++)
        {
            Debug.Log("卡在这步？");
            await Info.GlobalBattleInfo.SelectUnits[i].Hurt(1);
        }
        
        Debug.LogWarning("打出卡牌效果");
    });
}

