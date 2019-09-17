using CardSpace;
using Command;
using System;
using System.Threading.Tasks;

public class Card10312 : Card
{
    [TriggerType.PlayCard]
    public Func<Task> Step1 => (async () => { print("สนำร"); await Task.Delay(1000); });
    [TriggerType.Deploy]
    public Func<Task> Step2 => (async () =>
    {
        await StateCommand.WaitForSelectLocation();
        await Deploy();
        //await StateCommand.WaitForSelecUnit(this, Command.GameCommand.GetCardList("all&battle&noglod"), 1);
        //bool IsMyCard  = Info.GlobalBattleInfo.SelectUnits[0].IsMyCard;
        //await Info.GlobalBattleInfo.SelectUnits[0].Destory();
        //await Command.CardCommand.SummonCard(index = 0,IsPlayerSummon=IsMyCard);
        await Task.Delay(100);
    });
}

