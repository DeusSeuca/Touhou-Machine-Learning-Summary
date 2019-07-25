using CardSpace;
using Command;
using System;
using System.Threading.Tasks;

public class Card1001 : Card
{

    [TriggerType.PlayCard]
    public Func<Task> Step1 => (async () =>
    {
        await StateCommand.WaitForSelectLocation();
        await Deploy();
        await Task.Delay(100);
    });
    [TriggerType.PlayCard]
    public Func<Task> Step2 => (async () =>
    {
        //await StateCommand.WaitForSelectBoardCard(GameCommand.GetCardList(OnOther: LoadRangeOnOther.My_Hand), CardBoardMode.Select, 1);
        //Info.GlobalBattleInfo.PlayerPlayCard = Info.GlobalBattleInfo.SingleSelectCardOnBoard;
        //await CardCommand.PlayCard();
        //await StateCommand.WaitForSelecUnit(this,GameCommand.GetCardList( LoadRangeOnBattle.My_All),1);
        //await CardCommand.RebackCard();
        await Task.Delay(100);
    });
}

