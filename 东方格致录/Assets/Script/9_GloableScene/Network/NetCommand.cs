using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;
using System;
using System.Threading.Tasks;
using UnityEngine;
using static GameEnum;
using static NetInfoModel;
using static NetworkCommsDotNet.NetworkComms;

namespace Command
{
    public class NetCommand
    {
        static Connection Client;
        public static void Bind(string Tag, PacketHandlerCallBackDelegate<string> Func)
        {
            AppendGlobalIncomingPacketHandler(Tag, Func);
        }
        public static void Init(Connection NetClient)
        {
            Client = NetClient;
            Debug.Log("登录服务器");
            Bind("InitBattleInfo", InitBattleInfo);

            Bind("JoinResult", JoinResult);
            //Bind("PlayerJudge", PlayerJudge);
            Bind("AsyncInfoRequir", AsyncInfoRequir);
        }

        public static void AsyncInfo(NetAcyncType AcyncType)
        {
            if (Info.GlobalBattleInfo.IsPVP)
            {
                switch (AcyncType)
                {
                    case NetAcyncType.FocusCard:
                        {
                            Debug.Log("同步焦点卡片为" + Info.GlobalBattleInfo.PlayerFocusCard.Location);
                            Vector2 TargetCardLocation = Info.GlobalBattleInfo.PlayerFocusCard != null ? Info.GlobalBattleInfo.PlayerFocusCard.Location : new Vector2(-1, -1);
                            Client.SendMessge("AsyncInfo",new GeneralCommand(AcyncType, Info.GlobalBattleInfo.RoomID, (int)TargetCardLocation.x, (int)TargetCardLocation.y));
                            break;
                        }
                    default:
                        break;
                }


            }
        }
        private static void AsyncInfoRequir(PacketHeader packetHeader, Connection connection, string Data)
        {
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
                        Debug.Log("同步焦点为" + X+" "+Y);

                        Info.GlobalBattleInfo.OpponentFocusCard = Info.RowsInfo.GetCard((X,Y));
                        break;
                    }
                default:
                    break;
            }

        }
        public static string Register(string name, string password)
        {
            Debug.Log("发送指令");
            Debug.Log("发送注册指令" + new NetInfoModel.GeneralCommand<string>(name, password).ToJson());
            return Client.SendReceiveMessge("Regist", "RegistResult", new NetInfoModel.GeneralCommand<string>(name, password));
        }
        public static string Login(string name, string password)
        {
            return Client.SendReceiveMessge("Login", "LoginResult", new NetInfoModel.GeneralCommand<string>(name, password));
        }
        public static void JoinRoom()
        {
            Debug.Log(Info.AllPlayerInfo.UserInfo.ToJson());
            Client.SendMessge("Join", Info.AllPlayerInfo.UserInfo);
        }
        private static void JoinResult(PacketHeader packetHeader, Connection connection, string data)
        {
            Debug.Log("接收到加入结果:" + data);
            Info.AllPlayerInfo.OpponentInfo = data.ToObject<PlayerInfo>();
            // var Result= data.ToObject<(int, NetInfoModel.PlayerInfo)>();
            // Info.AllPlayerInfo.OpponentInfo = Result.Item2;
            Control.UserModeControl.IsJoinRoom = true;
        }
        private static void InitBattleInfo(PacketHeader packetHeader, Connection connection, string data)
        {
            Debug.Log("接收到加入结果:" + data);
            object[] ReceiveInfo = data.ToObject<GeneralCommand>().Datas;
            Info.GlobalBattleInfo.RoomID = int.Parse(ReceiveInfo[0].ToString());
            Debug.LogError("房间号为" + Info.GlobalBattleInfo.RoomID);
            Info.GlobalBattleInfo.IsPlayer1 = (bool)ReceiveInfo[1];
            Debug.LogError(Info.GlobalBattleInfo.IsPlayer1);


            //Debug.Log(Info.AllPlayerInfo.Player1Info.ToJson());
            //Debug.Log(Info.AllPlayerInfo.Player2Info.ToJson());
            //Control.UserModeControl.IsJoinRoom = true;
        }
        //private static void PlayerJudge(PacketHeader packetHeader, Connection connection, string data)
        //{
        //    Info.GlobalBattleInfo.IsPlayer1 = (data == "1");
        //    Debug.LogError(Info.GlobalBattleInfo.IsPlayer1);
        //}
        //[Obsolete]
        //public static async Task<string> JoinRoomAsync()
        //{
        //    NetInfoModel.PlayerInfo playerInfo = Info.AllPlayerInfo.Player1Info;
        //    string Msg = "";
        //    await Task.Run(() =>
        //     {
        //         try
        //         {
        //             Msg = Client.SendReceiveMessge("Join", "JoinResult", new NetInfoModel.GeneralCommand(playerInfo), 5);
        //         }
        //         catch (Exception)
        //         {
        //             Msg = "连接超时";
        //         }
        //     });
        //    return Msg;
        //}

    }
}

