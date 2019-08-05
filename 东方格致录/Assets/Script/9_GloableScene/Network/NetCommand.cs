﻿using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static NetInfoModel;
using static NetworkCommsDotNet.NetworkComms;

namespace Command
{
    public class NetCommand
    {
        static Connection Client;
        public static void Bind(string Tag, PacketHandlerCallBackDelegate<string> Func) => AppendGlobalIncomingPacketHandler(Tag, Func);
        public static void Init(Connection NetClient)
        {
            Client = NetClient;
            Bind("InitBattleInfo", InitBattleInfo);
            Bind("JoinResult", JoinResult);
            Bind("AsyncInfoRequir", AsyncInfoRequir);
            Bind("SurrenderRequir", SurrenderRequir);
        }

        public static string Register(string name, string password)
        {
            //Debug.Log("发送指令");
            //Debug.Log("发送注册指令" + new NetInfoModel.GeneralCommand<string>(name, password).ToJson());
            return Client.SendReceiveMessge("Regist", "RegistResult", new NetInfoModel.GeneralCommand<string>(name, password));
        }
        public static string Login(string name, string password)
        {
            return Client.SendReceiveMessge("Login", "LoginResult", new NetInfoModel.GeneralCommand<string>(name, password));
        }
        public static void JoinRoom()
        {
            //Debug.Log(Info.AllPlayerInfo.UserInfo.ToJson());
            Info.GlobalBattleInfo.IsPVP = true;
            Client.SendMessge("Join", Info.AllPlayerInfo.UserInfo);
            //Debug.Log("发送完毕");
        }
        private static void JoinResult(PacketHeader packetHeader, Connection connection, string data)
        {
            Info.AllPlayerInfo.OpponentInfo = data.ToObject<PlayerInfo>();
            Info.GlobalBattleInfo.IsPVP = true;
            MainThread.Run(() => { SceneManager.LoadSceneAsync(2); });
        }
        public static void Surrender()
        {
            Client.SendMessge("Surrender", Info.GlobalBattleInfo.RoomID);
        }
        public static void SurrenderRequir(PacketHeader packetHeader, Connection connection, string data)
        {
            _ = Command.StateCommand.BattleEnd(true, true);
        }
        private static void InitBattleInfo(PacketHeader packetHeader, Connection connection, string data)
        {
            //Debug.Log("接收到加入结果:" + data);
            object[] ReceiveInfo = data.ToObject<GeneralCommand>().Datas;
            Info.GlobalBattleInfo.RoomID = int.Parse(ReceiveInfo[0].ToString());
            //Debug.LogError("房间号为" + Info.GlobalBattleInfo.RoomID);
            Info.GlobalBattleInfo.IsPlayer1 = (bool)ReceiveInfo[1];
            Info.GlobalBattleInfo.IsMyTurn = (bool)ReceiveInfo[1];
            //Debug.LogError(Info.GlobalBattleInfo.IsPlayer1);
        }
        public static void AsyncInfo(NetAcyncType AcyncType)
        {
            if (Info.GlobalBattleInfo.IsPVP && (Info.GlobalBattleInfo.IsMyTurn || AcyncType == NetAcyncType.FocusCard))
            {
                switch (AcyncType)
                {
                    case NetAcyncType.FocusCard:
                        {
                            Location TargetCardLocation = Info.GlobalBattleInfo.PlayerFocusCard != null ? Info.GlobalBattleInfo.PlayerFocusCard.Location : new Location(-1, -1);
                            Client.SendMessge("AsyncInfo", new GeneralCommand(AcyncType, Info.GlobalBattleInfo.RoomID, (int)TargetCardLocation.x, (int)TargetCardLocation.y));
                            break;
                        }
                    case NetAcyncType.PlayCard:
                        {
                            Location TargetCardLocation = Info.GlobalBattleInfo.PlayerPlayCard.Location;
                            //Debug.Log("同步焦点卡片为" + TargetCardLocation);
                            Client.SendMessge("AsyncInfo", new GeneralCommand(AcyncType, Info.GlobalBattleInfo.RoomID, (int)TargetCardLocation.x, (int)TargetCardLocation.y));
                            break;
                        }
                    case NetAcyncType.FocusRegion:
                        {
                            int RowRank = Info.GlobalBattleInfo.SelectRegion.RowRank;
                            Debug.Log("同步焦点区域为" + RowRank);
                            Client.SendMessge("AsyncInfo", new GeneralCommand(AcyncType, Info.GlobalBattleInfo.RoomID, (int)RowRank));
                            break;
                        }
                    case NetAcyncType.FocusLocation:
                        {
                            int RowRank = Info.GlobalBattleInfo.SelectRegion.RowRank;
                            int LocationRank = Info.GlobalBattleInfo.SelectLocation;
                            Debug.Log("同步焦点区域为" + RowRank);
                            Client.SendMessge("AsyncInfo", new GeneralCommand(AcyncType, Info.GlobalBattleInfo.RoomID, RowRank, LocationRank));
                            break;
                        }

                    case NetAcyncType.SelectUnites:
                        {
                            List<Location> Locations = Info.GlobalBattleInfo.SelectUnits.Select(unite => unite.Location).ToList();
                            Debug.LogError("发出的指令为：" + new GeneralCommand(AcyncType, Info.GlobalBattleInfo.RoomID, Locations).ToJson());
                            Client.SendMessge("AsyncInfo", new GeneralCommand(AcyncType, Info.GlobalBattleInfo.RoomID, Locations.ToJson()));
                            Debug.LogError("选择单位完成");
                            break;
                        }

                    case NetAcyncType.Pass:
                        {
                            Client.SendMessge("AsyncInfo", new GeneralCommand(AcyncType, Info.GlobalBattleInfo.RoomID));
                            break;
                        }
                    case NetAcyncType.Surrender:
                        break;
                    default:
                        break;
                }
            }
        }


        public static void AsyncInfoRequir(PacketHeader packetHeader, Connection connection, string Data)
        {
            //Debug.Log("收到信息");
            //Debug.Log("收到信息" + Data);
            object[] ReceiveInfo = Data.ToObject<GeneralCommand>().Datas;
            //Debug.Log("收到信息" + Data);
            //Debug.Log("收到信息1：" + ReceiveInfo[0].ToString());
            NetAcyncType Type = (NetAcyncType)int.Parse(ReceiveInfo[0].ToString());
            switch (Type)
            {
                case NetAcyncType.FocusCard:
                    {
                        int X = int.Parse(ReceiveInfo[2].ToString());
                        int Y = int.Parse(ReceiveInfo[3].ToString());
                        Info.GlobalBattleInfo.OpponentFocusCard = Info.RowsInfo.GetCard(X, Y);
                        break;
                    }
                case NetAcyncType.PlayCard:
                    {
                        //Debug.Log("触发卡牌同步");
                        int X = int.Parse(ReceiveInfo[2].ToString());
                        int Y = int.Parse(ReceiveInfo[3].ToString());
                        Info.GlobalBattleInfo.PlayerPlayCard = Info.RowsInfo.GetCard(X, Y);
                        //_ = Command.CardCommand.PlayCard();
                        CardCommand.PlayCard().Wait();
                        break;
                    }
                case NetAcyncType.FocusRegion:
                    {
                        //Debug.Log("触发区域同步");
                        int X = int.Parse(ReceiveInfo[2].ToString());
                        Info.GlobalBattleInfo.SelectRegion = Info.RowsInfo.GetSingleRowInfoById(X);
                        break;
                    }

                case NetAcyncType.FocusLocation:
                    {
                        //Debug.Log("触发坐标同步");
                        int X = int.Parse(ReceiveInfo[2].ToString());
                        int Y = int.Parse(ReceiveInfo[3].ToString());
                        //Info.RowsInfo.SingleRowInfos.First(infos => infos.ThisRowCard == Info.RowsInfo.GlobalCardList[X]);
                        Info.GlobalBattleInfo.SelectRegion = Info.RowsInfo.GetSingleRowInfoById(X);
                        Info.GlobalBattleInfo.SelectLocation = Y;
                        //Debug.Log($"坐标为：{X}:{Y}");
                        //Debug.Log($"信息为：{Info.GlobalBattleInfo.SelectRegion}:{Info.GlobalBattleInfo.SelectLocation}");

                        break;
                    }

                case NetAcyncType.SelectUnites:
                    {
                        Debug.Log("收到同步单位信息为" + Data);
                        List<Location> Locations = ReceiveInfo[2].ToString().ToObject<List<Location>>();
                        Info.GlobalBattleInfo.SelectUnits.AddRange(Locations.Select(location => Info.RowsInfo.GetCard(location.x, location.y)));
                        break;
                    }

                case NetAcyncType.Pass:
                    {
                        UiCommand.SetCurrentPass();
                        break;
                    }
                case NetAcyncType.Surrender:
                    break;
                default:
                    break;
            }
        }
    }
}

