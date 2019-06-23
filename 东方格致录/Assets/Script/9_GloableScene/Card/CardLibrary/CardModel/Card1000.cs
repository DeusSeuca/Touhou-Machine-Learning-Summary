using CardSpace;
using Command;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using static GameEnum;

public class Card1000 : Card
{
    [TriggerType.Deploy]
    public Func<Task> Step1 => (async () => { print("test"); await Task.Delay(1000); });
    [TriggerType.Deploy]
    public Func<Task> Step2 => (async () =>
    {
        await StateCommand.WaitForSelectLocation();
        await CardCommand.Deploy();
        await Task.Delay(100);
    });
    [TriggerType.Deploy]
    public Func<Task> Step3 => (async () =>
    {
        await StateCommand.WaitForSelecUnit(this, GameCommand.GetCardList(LoadRangeOnBattle.My_Water).Where(card => card.CardPoint < 5).ToList(), 2);
        Info.GlobalBattleInfo.SelectUnits.ForEach(card => card.Gain(this, 2));
        Info.GlobalBattleInfo.SelectUnits.Clear();
        await StateCommand.WaitForSelecUnit(this, GameCommand.GetCardList(LoadRangeOnBattle.My_Water).Where(card => card.CardPoint < 5).ToList(), 1);
        Info.GlobalBattleInfo.SelectUnits.ForEach(card => card.Hurt(this, 2));
        Info.GlobalBattleInfo.SelectUnits.Clear();


        //print(Info.GlobalBattleInfo.SelectUnits.Count);
        //Info.GlobalBattleInfo.SelectUnits.ForEach(x => Debug.Log("·ûºÏµÄ¿¨Æ¬idÎª" + x.CardId));
        await Task.Delay(100);
    });
}

