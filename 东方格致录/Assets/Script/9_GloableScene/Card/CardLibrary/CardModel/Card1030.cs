using CardSpace;
using Command;
using System;
using System.Threading.Tasks;

public class Card1030 : Card
{
    [TriggerType.Deploy]
    public Func<Task> Step1 => (async () => { print("test"); await Task.Delay(1000); });
    [TriggerType.Deploy]
    public Func<Task> Step2 => (async () =>
    {
        await StateCommand.WaitForSelectLocation();
        await Deploy();
        await Task.Delay(100);
    });
}

