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
        //print("�ȴ�ѡ��λ��");
        await StateCommand.WaitForSelectLocation();
        //print("��ʼ����");
        await Deploy();
        //print("�������");
        Trigger<TriggerType.Deploy>();
        await Task.Delay(100);
    });
    [TriggerType.PlayCard]
    public Func<Task> Step3 => (async () =>
    {
        await StateCommand.WaitForSelecUnit(this,GameCommand.GetCardList( LoadRangeOnBattle.My_All),2);
        Debug.Log("�����ⲽ��");
       
        for (int i = 0; i < Info.GlobalBattleInfo.SelectUnits.Count; i++)
        {
            Debug.Log("�����ⲽ��");
            await Info.GlobalBattleInfo.SelectUnits[i].Hurt(1);
        }
        
        Debug.LogWarning("�������Ч��");
    });
}

