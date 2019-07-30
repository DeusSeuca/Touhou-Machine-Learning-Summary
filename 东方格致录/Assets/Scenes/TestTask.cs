using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TestTask : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Task.Run(() =>
        {
            Command.NetCommand.AsyncInfoRequir(null, null, "{\"Datas\":[3,1,5,0]}");
        }).Wait();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
