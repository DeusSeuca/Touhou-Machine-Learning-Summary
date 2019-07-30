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
        print("�ȴ�ѡ��λ��");
        await StateCommand.WaitForSelectLocation();
        print("��ʼ����");
        await Deploy();
        print("�������");
        await Task.Delay(100);
    });
}

