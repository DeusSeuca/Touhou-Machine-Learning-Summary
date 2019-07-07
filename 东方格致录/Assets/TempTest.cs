using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix;
using Newtonsoft.Json;
using static NetInfoModel;

public class TempTest : MonoBehaviour
{
    string Data;
    object[] datas;
    // Start is called before the first frame update
    void Start()
    {
        Data = "{\"Datas\": [0, 0, 10, 0]}";
        object[] ReceiveInfo = Data.ToObject<GeneralCommand>().Datas;
        Debug.Log("收到信息" + Data);
        int Type = int.Parse(ReceiveInfo[0].ToString());
        switch (Type)
        {
            case 0:
                {
                    Debug.Log("同步焦点");

                    int X = int.Parse(ReceiveInfo[2].ToString());
                    int Y = int.Parse(ReceiveInfo[3].ToString());
                    Debug.Log("同步焦点为" + X + " " + Y);

                    Info.GlobalBattleInfo.OpponentFocusCard = Info.RowsInfo.GetCard((X, Y));
                    break;
                }
            default:
                break;
        }
        Debug.LogError(ReceiveInfo[0]);
        Debug.LogError(ReceiveInfo[1]);

        Info.GlobalBattleInfo.RoomID = int.Parse(ReceiveInfo[0].ToString());
        //Debug.LogError(test1);



    }
    class MyClass
    {
        public int a = 5;

        public MyClass(int a)
        {
            this.a = a;
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
