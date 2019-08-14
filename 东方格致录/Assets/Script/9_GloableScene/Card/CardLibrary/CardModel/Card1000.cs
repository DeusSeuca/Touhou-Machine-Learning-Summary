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
        await StateCommand.WaitForSelectLocation();
        await Deploy();
        Trigger<TriggerType.Deploy>();
        await Task.Delay(100);
    });
    [TriggerType.PlayCard]
    public Func<Task> Step3 => (async () =>
    {
        await StateCommand.WaitForSelecUnit(this,GameCommand.GetCardList( LoadRangeOnBattle.My_All),2);
        for (int i = 0; i < Info.GlobalBattleInfo.SelectUnits.Count; i++)
        {
            await Info.GlobalBattleInfo.SelectUnits[i].Hurt(1);
        }
    });
}

