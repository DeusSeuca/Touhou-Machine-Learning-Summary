using Extension;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;
using NetworkCommsDotNet.Connections.TCP;
using System.Threading.Tasks;
using UnityEngine;

namespace Network
{
    public static class NetExtern
    {
        public static void SendMessge(this Connection con, string Tag, object data)
        {
            if (Info.AgainstInfo.IsPVP)
            {
                Task.Run(async () =>
                {
                    while (true)
                    {
                        if (con.ConnectionInfo.ConnectionState == ConnectionState.Established)
                        {
                            Debug.Log("成功发送");
                            con.SendObject(Tag, data.ToJson());
                            break;
                        }
                        else
                        {
                            await Task.Delay(100);
                            Debug.Log("当前掉线状态");
                        }
                    }
                });


                //Debug.Log(con.ConnectionInfo.ConnectionState);
                //try
                //{
                //}
                //catch (System.Exception)
                //{
                //    Debug.Log("需要重连");
                //    //NetClient.connInfo = new ConnectionInfo(NetClient.ip);
                //    //NetClient.Client= TCPConnection.GetConnection(NetClient.connInfo);
                //    //Command.Network.NetCommand.Init();
                //    Task.Run(() =>
                //    {
                //        Task.Delay(50);
                //        con.SendMessge(Tag, data);
                //    });

                //}

            }
        }
        public static string SendReceiveMessge(this Connection con, string SengTag, string ReceiveTag, object Info, int LimitTime = 5)
        {
            return con.SendReceiveObject<string, string>(SengTag, ReceiveTag, LimitTime * 1000, Info.ToJson());
        }
    }
}