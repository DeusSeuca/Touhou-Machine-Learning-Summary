using CardSpace;
using Command;
using System;
using System.Threading.Tasks;

public class Card1002 : Card
{
    [TriggerType.PlayCard]
    public Func<Task> Step1 => (async () => { print("test"); IsCanSee = false; await Task.Delay(1000); });
    [TriggerType.PlayCard]
    public Func<Task> Step2 => (async () =>
    {
        RowCommand.GetCardList("my&noglod&nogaipai");
        await StateCommand.WaitForSelectLocation();
        await Deploy();
        await Task.Delay(100);
    });
}

