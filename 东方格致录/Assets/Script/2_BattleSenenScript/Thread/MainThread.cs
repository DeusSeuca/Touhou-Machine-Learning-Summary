using System;
using System.Threading.Tasks;
using UnityEngine;

public class MainThread : MonoBehaviour
{
    static Action TargetAction;
    void Update()
    {
        if (TargetAction != null)
        {
            TargetAction();
            TargetAction = null;
        }
    }
    public static void Run(Action RunAction)
    {
        Task.Run(() =>
        {
            while (TargetAction != null) { }
            //print("装载函数");
            TargetAction = RunAction;
        });
    }
}
