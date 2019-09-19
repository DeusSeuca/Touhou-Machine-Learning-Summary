using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
namespace Thread
{
    public class MainThread : MonoBehaviour
    {
        static Queue<Action> TargetAction = new Queue<Action>();
        public static void Run(Action RunAction) => TargetAction.Enqueue(RunAction);
        void Update()
        {
            if (TargetAction.Count > 0)
            {
                for (int i = 0; i < TargetAction.Count; i++)
                {
                    TargetAction.Dequeue()();
                }
            }
        }
    }

}
