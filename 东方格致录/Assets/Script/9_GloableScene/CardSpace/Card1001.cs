using Command;
using GameAttribute;
using Sirenix.OdinInspector;
using System;
using System.Threading.Tasks;
using GameEnum;
namespace CardSpace
{
    public class Card1001 : Card
    {
        [CardProperty(CardField.Timer)]
        public int cd = 3;
        [TriggerType.PlayCard]//效果：使用卡牌时，cd+1
        public Func<Task> Step1 => async () => this[CardField.Timer]++;

        [TriggerType.PlayCard]
        public Func<Task> Step2 => async () =>
        {
            Info.AgainstInfo.AllCardList.ForEach(card => card[CardField.Timer]++);
            await StateCommand.WaitForSelectLocation();
            await Deploy();
            await Task.Delay(100);
        };
        //[ShowInInspector]public int show => this[CardField.Timer];
        //[ShowInInspector]public int show2 => this[CardField.Point];
    }
}


