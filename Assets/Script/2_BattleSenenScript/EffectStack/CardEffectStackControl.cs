using CardModel;
using CardSpace;
using Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using Sirenix.OdinInspector;
namespace Control
{

    public class CardEffectStackControl:MonoBehaviour
    {
        public static bool IsRuning;
        public static int taskCount = 0;
        [ShowInInspector]
        public Dictionary<string, int> dict;
        [ShowInInspector]
        public static Stack<(Func<TriggerInfo, Task>, TriggerInfo)> TaskStack = new Stack<(Func<TriggerInfo, Task>, TriggerInfo)>();
        public static async Task Run()
        {
            if (!IsRuning)
            {
                IsRuning = true;
                while (TaskStack.Count != 0)
                {
                    (Func<TriggerInfo, Task>, TriggerInfo) taskinfo = TaskStack.Pop();
                    await taskinfo.Item1(taskinfo.Item2);
                }
                IsRuning = false;
            }
        }
        public static async Task TriggerLogic(TriggerInfo triggerInfo)
        {
            foreach (var card in triggerInfo.targetCards)
            {
                AddEffectStactTask();
                await TriggerBoradcast(triggerInfo[card][TriggerTime.Before]);
                await Trigger(triggerInfo[card][TriggerTime.When]);
                await TriggerBoradcast(triggerInfo[card][TriggerTime.After]);
                RemoveEffectStactTask();
            }
        }
        public static void AddEffectStactTask() => taskCount++;
        public static void RemoveEffectStactTask()
        {
            taskCount--;
            if (taskCount == 0)
            {
                Info.AgainstInfo.IsCardEffectCompleted = true;
                Debug.LogError("当前效果栈清空");
            }
        }

        public static async Task TriggerBoradcast(TriggerInfo triggerInfo)
        {
            triggerInfo.targetCards = Info.AgainstInfo.cardSet.BroastCardList(triggerInfo.triggerCard);
            foreach (var card in triggerInfo.targetCards)
            {
                triggerInfo.targetCards = new List<Card>() { card };
                await Trigger(triggerInfo);
            }
        }
        public static async Task Trigger(TriggerInfo triggerInfo)
        {
            var tasks = triggerInfo.targetCards[0].cardEffect[triggerInfo.triggerTime][triggerInfo.triggerType];
            tasks.Reverse();
            tasks.ForEach(task => TaskStack.Push((task, triggerInfo)));
            await Run();
        }
        
    }
}