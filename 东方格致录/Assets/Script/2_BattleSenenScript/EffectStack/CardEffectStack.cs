using CardSpace;
using Info;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Control
{
    public class CardEffectStackControl
    {
        public static Stack<Func<Task>> TaskStack = new Stack<Func<Task>>();
        public static bool IsRuning;
        public static async Task Run()
        {
            if (!IsRuning)
            {
                IsRuning = true;
                while (TaskStack.Count != 0)
                {
                    await TaskStack.Pop()();
                }
                IsRuning = false;
                GlobalBattleInfo.IsCardEffectCompleted = true;
            }
        }
        public static async Task TriggerCardList<T>(List<Card> Cards) => Cards.ForEach(card => card.Trigger<T>());
    }
}