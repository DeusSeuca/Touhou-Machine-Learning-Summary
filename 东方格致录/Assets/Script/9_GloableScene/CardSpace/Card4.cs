using Command;
using System;
using System.Threading.Tasks;
namespace CardSpace
{
    public class Card4 : Card
    {
        [TriggerType.PlayCard]
        public Func<Task> Step1 => async () => { print("test"); await Task.Delay(1000); };

        [TriggerType.PlayCard]
        public Func<Task> Step2 => async () =>
        {
            await StateCommand.WaitForSelectLocation();
            await Deploy();
            await Task.Delay(100);
        };
    }
}


