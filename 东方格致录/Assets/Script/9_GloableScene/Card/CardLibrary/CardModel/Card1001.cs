using CardSpace;
using Command;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static GameEnum;

public class Card1001 : Card
{

    [TriggerType.Deploy]
    public Func<Task> Step1 => (async () =>
    {
        await StateCommand.WaitForSelectLocation();
        await Deploy();
        await Task.Delay(100);
    });
    [TriggerType.Deploy]
    public Func<Task> Step2 => (async () =>
    {
        await StateCommand.WaitForSelectBoardCard(GameCommand.GetCardList(OnOther: LoadRangeOnOther.My_Hand), CardBoardMode.Select, 1);
        Info.GlobalBattleInfo.PlayerPlayCard = Info.GlobalBattleInfo.SingleSelectCardOnBoard;
        await CardCommand.PlayCard();
        await StateCommand.WaitForSelecUnit(this,GameCommand.GetCardList( LoadRangeOnBattle.My_All),1);
        await CardCommand.RebackCard();
        await Task.Delay(100);
    });
}

