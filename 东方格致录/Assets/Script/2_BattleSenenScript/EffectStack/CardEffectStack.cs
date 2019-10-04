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
        public static Stack<Func<Task>> TaskStack = new Stack<Func<Task>>();
        public static List<Func<Task>> AsyneTriggerTask = new List<Func<Task>>();
        public static async Task TriggerCardList<T>(List<Card> Cards)
        {
            Cards.ForEach(card => card.Trigger<T>());
        }
        public void Trigger<T>()
        {
            List<Func<Task>> Steps = new List<Func<Task>>();
            List<PropertyInfo> tasks = GetType().GetProperties().Where(x =>
                x.GetCustomAttributes(true).Count() > 0 && x.GetCustomAttributes(true)[0].GetType() == typeof(T)).ToList();
            tasks.Reverse();
            tasks.Select(x => x.GetValue(this)).Cast<Func<Task>>().ToList().ForEach(CardEffectStackControl.TaskStack.Push);
            _ = Run();
        }
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
                Debug.LogWarning("效果执行完毕");
                AgainstInfo.IsCardEffectCompleted = true;
            }
        }
    }
}