using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Thread;
using UnityEngine;

public class CoinControl : MonoBehaviour
{
    AnimationCurve curve;
    public GameObject center;
    public GameObject roatePoint;
    public GameObject water;
    public GameObject fire;
    public GameObject wind;
    public GameObject soil;
    [ShowInInspector]
    // Start is called before the first frame update
    void Start()
    {
       
        Task.Run(async () =>
        {
            await Task.Delay(1000);
            Fold();
            ShowCurrentPlayerCoin(true);
            while (true)
            {
                MainThread.Run(() =>
                {
                    transform.GetChild(0).transform.eulerAngles = new Vector3(0, 0, -Timer.Process);

                });
                await Task.Delay(1000);
            }
        });
    }
    [Button("切换属性")]
    public void ChangeProperty(GameEnum.Region region)
    {
        Unfold();
        Vector3 start = roatePoint.transform.eulerAngles;
        Vector3 end = new Vector3(0, 0, 450 + (int)region * 90);
        Task.Run(async () =>
        {
            await Task.Delay(1000);
            for (int i = 0; i < 100; i++)
            {
                MainThread.Run(() =>
                {
                    roatePoint.transform.eulerAngles = Vector3.Lerp(start, end, i / 100f);
                });
                await Task.Delay(10);
            }
            await Task.Delay(500);
            MainThread.Run(() =>
            {
                Fold();
            });
        });
    }
    // Update is called once per frame
    [Button("展开")]
    public void Unfold()
    {
        Vector3 start = Vector3.zero;
        Vector3 end_water = new Vector3(25, 0, 0);
        Vector3 end_fire = new Vector3(0, -25, 0);
        Vector3 end_wind = new Vector3(-25, 0, 0);
        Vector3 end_soil = new Vector3(0, 25, 0);
        Task.Run(async () =>
        {
            for (int i = 0; i < 100; i++)
            {
                MainThread.Run(() =>
                {
                    water.transform.localPosition = Vector3.Lerp(start, end_water, i / 100f);
                    fire.transform.localPosition = Vector3.Lerp(start, end_fire, i / 100f);
                    wind.transform.localPosition = Vector3.Lerp(start, end_wind, i / 100f);
                    soil.transform.localPosition = Vector3.Lerp(start, end_soil, i / 100f);
                });
                await Task.Delay(5);
            }
        });
    }
    [Button("关闭")]
    public void Fold()
    {
        Vector3 start_water = new Vector3(25, 0, 0);
        Vector3 start_fire = new Vector3(0, -25, 0);
        Vector3 start_wind = new Vector3(-25, 0, 0);
        Vector3 start_soil = new Vector3(0, 25, 0);
        Vector3 end = Vector3.zero;
        Task.Run(async () =>
        {
            for (int i = 0; i < 100; i++)
            {
                MainThread.Run(() =>
                {
                    water.transform.localPosition = Vector3.Lerp(start_water, end, i / 100f);
                    fire.transform.localPosition = Vector3.Lerp(start_fire, end, i / 100f);
                    wind.transform.localPosition = Vector3.Lerp(start_wind, end, i / 100f);
                    soil.transform.localPosition = Vector3.Lerp(start_soil, end, i / 100f);
                });
                await Task.Delay(5);
            }
        });
    }
    [Button("旋转")]
    public void ShowCurrentPlayerCoin(bool isMyturn)
    {
        Vector3 start = new Vector3(0, isMyturn ? 180 : 0, 0);
        Vector3 end = new Vector3(0, isMyturn ? 0 : 180, 0);
        Task.Run(async () =>
        {
            for (int i = 0; i < 100; i++)
            {
                MainThread.Run(() =>
                {
                    center.transform.eulerAngles = Vector3.Lerp(start, end, i / 100f);
                });
                await Task.Delay(100);
            }
        });
    }
}
