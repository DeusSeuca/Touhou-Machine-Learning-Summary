using CardSpace;
using Command;
using System;
using System.Threading.Tasks;

public class Card1000 : Card
{
    [TriggerType.PlayCard]
    public Func<Task> Step1 => (async () => { print("test"); await Task.Delay(1000); });
    [TriggerType.PlayCard]
    public Func<Task> Step2 => (async () =>
    {
        print("等待选择位置");
        await StateCommand.WaitForSelectLocation();
        print("开始部署");
        await Deploy();
        print("部署完成");
        await Task.Delay(100);
    });
}

