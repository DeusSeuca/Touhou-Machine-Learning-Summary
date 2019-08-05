using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;
using static NetInfoModel;

namespace Test
{
    public class TestScript : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetMouseButtonDown(4))
            {
                Task.Run(() =>
                {
                    print("等待选择位置");
                    Command.StateCommand.WaitForSelectLocation().Wait();
                    print("开始部署");
                    //Command.NetCommand.AsyncInfoRequir(null, null, "{\"Datas\":[3,9,1,0]}");
                }).Wait();
            }
            if (Input.GetMouseButtonDown(3))
            {
                Task.Run(async () =>
                {
                    for (int i = 0; i < Info.GlobalBattleInfo.SelectUnits.Count; i++)
                    {
                        Debug.Log("卡在这步？");
                        await Info.GlobalBattleInfo.SelectUnits[i].Hurt(1);
                        Debug.Log("卡在这步？");

                    }
                }).Wait();
                //string Data = "{\"Datas\":[4,0,[{\"x\":5,\"y\":0}]]}";

                //Command.NetCommand.AsyncInfoRequir(null, null, "{ \"Datas\":[4,9,[{\"x\":5,\"y\":0}]]}");
                //Command.CardCommand.DisCard(Info.RowsInfo.GetMyCardList(RegionTypes.Hand)[0]);
                //IEnumerator DelayCoroutine()
                //{
                //    TcpClient Client = new TcpClient();
                //    Client.Client.Accept();
                //    yield return new WaitForSeconds(0);
                //}
                //Task.Run(() =>
                //{
                //    Command.NetCommand.AsyncInfoRequir(null, null, "{\"Datas\":[1,9,1,0]}");
                //}).Wait();
            }
        }
    }

}


