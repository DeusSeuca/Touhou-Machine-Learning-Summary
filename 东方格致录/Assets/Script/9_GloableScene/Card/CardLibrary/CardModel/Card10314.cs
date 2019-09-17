using CardSpace;
using Command;
using System;
using System.Threading.Tasks;

public class Card10314 : Card
{
    [TriggerType.PlayCard]
    public Func<Task> Step1 => (async () => { print("打出卡牌"); await Task.Delay(1000); });
    [TriggerType.PlayCard]
    public Func<Task> Step2 => (async () =>
    {
        await StateCommand.WaitForSelectLocation();
        await Deploy();
        await Task.Delay(100);
    });
    [TriggerType.Deploy]//当部署时触发
    public Func<Task> Step3 => (async () =>
    {
        await StateCommand.WaitForSelecUnit(this, RowCommand.GetCardList("my&noglod&battle"), 1);
        await Info.GlobalBattleInfo.SelectUnits[0].Hurt(1);
    });
    [TriggerType.WhenOtherHurt]//当有单位被伤害时触发
    public Func<Task> Step4 => (async () =>
    {
        await Boost(1);
    });
}

