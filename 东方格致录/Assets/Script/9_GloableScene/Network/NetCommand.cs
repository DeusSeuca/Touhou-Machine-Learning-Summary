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
            Bind("SurrenderRequir", SurrenderRequir);
        }



        public static void AsyncInfo(NetAcyncType AcyncType)
        {
            if (Info.GlobalBattleInfo.IsPVP && (Info.GlobalBattleInfo.IsMyTurn|| AcyncType== NetAcyncType.FocusCard))
            {
                switch (AcyncType)
                {
                    case NetAcyncType.FocusCard:
                        {
                            //Debug.Log("同步焦点卡片为" + Info.GlobalBattleInfo.PlayerFocusCard.Location);
                            Vector2 TargetCardLocation = Info.GlobalBattleInfo.PlayerFocusCard != null ? Info.GlobalBattleInfo.PlayerFocusCard.Location : new Vector2(-1, -1);
                            Client.SendMessge("AsyncInfo", new GeneralCommand(AcyncType, Info.GlobalBattleInfo.RoomID, (int)TargetCardLocation.x, (int)TargetCardLocation.y));
                            break;
                        }

                    case NetAcyncType.PlayCard:
                        {
                            Vector2 TargetCardLocation = Info.GlobalBattleInfo.PlayerPlayCard.Location;
                            Debug.Log("同步焦点卡片为" + TargetCardLocation);
                            Client.SendMessge("AsyncInfo", new GeneralCommand(AcyncType, Info.GlobalBattleInfo.RoomID, (int)TargetCardLocation.x, (int)TargetCardLocation.y));
                            break;
                        }
                    default:
                        break;
                }
            }
        }
        public static void AsyncInfoRequir(PacketHeader packetHeader, Connection connection, string Data)
        {
            object[] ReceiveInfo = Data.ToObject<GeneralCommand>().Datas;
            Debug.Log("收到信息" + Data);
            Debug.Log("收到信息1：" + ReceiveInfo[0].ToString());
            int Type = int.Parse(ReceiveInfo[0].ToString());
            switch (Type)
            {
                case 0:
                    {
                        int X = int.Parse(ReceiveInfo[2].ToString());
                        int Y = int.Parse(ReceiveInfo[3].ToString());
                        Info.GlobalBattleInfo.OpponentFocusCard = Info.RowsInfo.GetCard((X, Y));
                        break;
                    }
                case 1:
                    {
                        Debug.Log("触发卡牌同步");
                        int X = int.Parse(ReceiveInfo[2].ToString());
                        int Y = int.Parse(ReceiveInfo[3].ToString());
                        Info.GlobalBattleInfo.PlayerPlayCard = Info.RowsInfo.GetCard((X, Y));
                        _ = Command.CardCommand.PlayCard();
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
        public static void Surrender()
        {
            Client.SendMessge("Surrender", null);
        }
        public static void SurrenderRequir(PacketHeader packetHeader, Connection connection, string data)
        {
            _ = Command.StateCommand.BattleEnd(true,true);
        }
        private static void InitBattleInfo(PacketHeader packetHeader, Connection connection, string data)
        {
            Debug.Log("接收到加入结果:" + data);
            object[] ReceiveInfo = data.ToObject<GeneralCommand>().Datas;
            Info.GlobalBattleInfo.RoomID = int.Parse(ReceiveInfo[0].ToString());
            Debug.LogError("房间号为" + Info.GlobalBattleInfo.RoomID);
            Info.GlobalBattleInfo.IsPlayer1 = (bool)ReceiveInfo[1];
            Info.GlobalBattleInfo.IsMyTurn = (bool)ReceiveInfo[1];
            Debug.LogError(Info.GlobalBattleInfo.IsPlayer1);
        }
    }
}

