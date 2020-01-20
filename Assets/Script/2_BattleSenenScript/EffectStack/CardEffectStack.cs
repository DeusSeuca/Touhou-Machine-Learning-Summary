using CardModel;
using CardSpace;
using Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

namespace Control
{
    public class CardEffectStackControl
    {
        public static bool IsRuning;
        public static Stack<Func<Card, Task>> TaskStack = new Stack<Func<Card, Task>>();
        public static List<Func<Card, Task>> AsyneTriggerTask = new List<Func<Card, Task>>();
        //需要改善
        public static async Task TriggerCardList<T>(List<Card> Cards)
        {
            Cards.ForEach(async card =>await card.TriggerAsync<T>());
        }
        //public static void Trigger<T>(Card card)
        //{
        //    List<Func<Card, Task>> Steps = new List<Func<Card, Task>>();
        //    List<PropertyInfo> tasks = card.GetType().GetProperties().Where(x =>
        //        x.GetCustomAttributes(true).Count() > 0 && x.GetCustomAttributes(true)[0].GetType() == typeof(T)).ToList();
        //    tasks.Reverse();
        //    tasks.Select(x => x.GetValue(card)).Cast<Func<Task>>().ToList().ForEach(TaskStack.Push);
        //    _ = Run();
        //}
        public static async Task Trigger_NewAsync<T>(Card card)
        {
            FieldInfo[] fieldInfo = card
                  .GetType()
                  .GetFields();
            FieldInfo fieldInfo1 = fieldInfo
                  .First(field => field.GetCustomAttributes(true).Any() &&
                  field.GetCustomAttributes(true)[0].GetType() == typeof(T));
            var tasks = (List<Func<Card,Task>>)fieldInfo1
                  .GetValue(card);
            tasks.Reverse();
            tasks.ForEach(TaskStack.Push);
            await Run(card);
        }
        public static async Task Trigger_NewAsync<T>(List<Card> cards)
        {
            foreach (var card in cards)
            {
                await Trigger_NewAsync<T>(card);
            }
        }
        public static async Task Run(Card card)
        {
            if (!IsRuning)
            {
                IsRuning = true;
                while (TaskStack.Count != 0)
                {
                    //可能出奇怪的bug
                    await TaskStack.Pop()(card);
                }
                IsRuning = false;
                Debug.LogWarning("效果执行完毕");
                AgainstInfo.IsCardEffectCompleted = true;
            }
        }
    }
}