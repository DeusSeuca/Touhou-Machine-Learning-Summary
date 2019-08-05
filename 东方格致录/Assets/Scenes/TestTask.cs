using Command;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TestTask : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Task.Run(async () =>
        //{
        //    Info.GlobalBattleInfo.IsPVP=true;
        //    Info.GlobalBattleInfo.SelectUnits.Add(await Command.CardCommand.CreatCard(1000));
        //    Debug.LogError("创建完成");
        //    NetCommand.AsyncInfo(NetAcyncType.SelectUnites);
        //});
    }

    // Update is called once per frame
    void Update()
    {

    }
}
