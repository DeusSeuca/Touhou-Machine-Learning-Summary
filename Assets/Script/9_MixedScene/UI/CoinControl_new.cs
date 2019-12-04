using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Thread;
using UnityEngine;

public class CoinControl_new : MonoBehaviour
{
    public AnimationCurve regionCcurve;
    public AnimationCurve campCcurve;
    public AnimationCurve centerCcurve;
    public GameObject camp;
    public GameObject center;
    public List<GameObject> Regions;

    float regionTime;
    Vector3 regions_start;
    Vector3 regions_end;

    float campTime;
    Vector3 camp_start;
    Vector3 camp_end;

    float centerTime;
    Vector3 center_start;
    Vector3 center_end;
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
    private void Update()
    {
        Regions.ForEach(region => region.transform.localPosition = Vector3.Lerp(regions_start, regions_end, regionCcurve.Evaluate(Time.time - regionTime)));
        camp.transform.eulerAngles = Vector3.Lerp(camp_start, camp_end, campCcurve.Evaluate(Time.time - campTime));
        center.transform.eulerAngles = Vector3.Lerp(center_start, center_end, centerCcurve.Evaluate(Time.time - centerTime));
    }
    [Button("切换属性")]
    public void ChangeProperty(GameEnum.Region region)
    {

        Task.Run(async () =>
        {
            MainThread.Run(() =>
            {
                center_start = center.transform.eulerAngles;
                center_end = center.transform.eulerAngles;
                Unfold();
            });
            Debug.Log("2");
            await Task.Delay(2000);
            MainThread.Run(() =>
            {
                center_end = new Vector3(0, 0, 360 + (int)region * 90);
                centerTime = Time.time;
            });
            Debug.Log("3");
            await Task.Delay(2000);
            MainThread.Run(() =>
            {
                Fold();
            });
            Debug.Log("4");
        });
    }
    // Update is called once per frame
    [Button("展开")]
    public void Unfold()
    {
        regions_start = new Vector3(0, 0, 0);
        regions_end = new Vector3(0, 25, 0);
        regionTime = Time.time;
    }
    [Button("关闭")]
    public void Fold()
    {
        regions_start = new Vector3(0, 25, 0);
        regions_end = new Vector3(0, 0, 0);
        regionTime = Time.time;
    }
    [Button("旋转")]
    public void ShowCurrentPlayerCoin(bool isMyturn)
    {
        camp_start = new Vector3(0, isMyturn ? 180 : 0, 0);
        camp_end = new Vector3(0, isMyturn ? 0 : 180, 0);
        campTime = Time.time;
    }
    private void OnMouseDown()
    {
        Debug.Log("yaya");
    }
    private void OnMouseEnter()
    {
        Debug.Log("yaya2");

    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("yaya2");
    }
}
