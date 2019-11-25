using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    static bool isTimerStart;
    public static int limitTime { get; set; }
    public static float time { get; set; }
    public static bool isTimeout => time > limitTime;
    public static void SetIsTimerStart(int limit_time)
    {
        isTimerStart = true;
        limitTime = limit_time;
    }

    public static void SetIsTimerClose()
    {
        isTimerStart = false;
        time = 0;
    }
    void Update()
    {
        if (isTimerStart)
        {
            time += Time.deltaTime;
        }
    }
}
