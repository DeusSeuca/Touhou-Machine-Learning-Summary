using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MainThread : MonoBehaviour
{
    static Action TargetAction;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
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
            TargetAction = RunAction;
        });
    }
}
