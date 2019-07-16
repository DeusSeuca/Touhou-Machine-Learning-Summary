using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject a;
    // Start is called before the first frame update
    void Start()
    {
        print("在主线程查看物体"+a);
        Task.Run(async () => {
            MainThread.Run(() => {
                print("在分线程回到主线程查看物体" + a);
            });
            await Task.Delay(10);
            print("在分线程查看物体");
            print(a);
        }).Wait();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
