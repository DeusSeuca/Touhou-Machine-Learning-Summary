using Command;
using System;
using System.Threading.Tasks;
namespace CardSpace
{
    public class Card2 : Card
    {
        [TriggerType.PlayCard]
        public Func<Task> Step1 => async () => { print("test"); await Task.Delay(1000); };

        [TriggerType.PlayCard]
        public Func<Task> Step2 => async () =>
        {
            print("µÈ´ý²¿Êð");
            await StateCommand.WaitForSelectLocation();
            await Deploy();
            await Task.Delay(100);
        };
    }
}


