using CardModel;
using Command;
using System;
using System.Threading.Tasks;
namespace CardSpace
{
    public class Card1014 : Card
    {
        [TriggerType.PlayCard]
        public Func<Task> Step1 => async () => { print("test"); await Task.Delay(1000); };

        [TriggerType.PlayCard]
        public Func<Task> Step2 => async () =>
        {
            await StateCommand.WaitForSelectLocation(this);
            await Deploy();
            await Task.Delay(100);
        };
    }
}