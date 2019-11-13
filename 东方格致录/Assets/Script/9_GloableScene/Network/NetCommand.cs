using Extension;
using GameEnum;
using Network;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;
using System.Collections.Generic;
using System.Linq;
using Thread;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Network.NetInfoModel;
using static NetworkCommsDotNet.NetworkComms;
namespace Command
{

    namespace Network
    {
        public static class NetCommand
        {
            static ClientConnectionContainer client = ConnectionFactory.CreateClientConnectionContainer("106.15.38.165", 495);
            public static void Init()
            {
                client = ConnectionFactory.CreateClientConnectionContainer("106.15.38.165", 495);
                client.ConnectionEstablished += (tcp, type) =>
                {
                    tcp.RegisterRawDataHandler("InitBattleInfo", (rawData, player) =>
                    {
                        //Debug.Log("接收到加入结果:" + data);
                        object[] ReceiveInfo = rawData.ParseToObject<GeneralCommand>().Datas;
                        Info.AgainstInfo.RoomID = int.Parse(ReceiveInfo[0].ToString());
                        //Debug.LogError("房间号为" + Info.GlobalBattleInfo.RoomID);
                        Info.AgainstInfo.IsPlayer1 = (bool)ReceiveInfo[1];
                        Info.AgainstInfo.IsMyTurn = Info.AgainstInfo.IsPlayer1;
                        //Debug.LogError(Info.GlobalBattleInfo.IsPlayer1);
                    });
                    tcp.RegisterRawDataHandler("AsyncInfoRequir", (rawData, player) =>
                    {
                        //Debug.Log("收到信息");
                        Debug.Log("收到信息" + rawData.ParseToObject<string>());
                        object[] ReceiveInfo = rawData.ParseToObject<GeneralCommand>().Datas;
                        //Debug.Log("收到信息" + Data);
                        //Debug.Log("收到信息1：" + ReceiveInfo[0].ToString());
                        NetAcyncType Type = (NetAcyncType)int.Parse(ReceiveInfo[0].ToString());
                        switch (Type)
                        {
                            case NetAcyncType.FocusCard:
                                {
                                    int X = int.Parse(ReceiveInfo[2].ToString());
                                    int Y = int.Parse(ReceiveInfo[3].ToString());
                                    Info.AgainstInfo.OpponentFocusCard = Info.RowsInfo.GetCard(X, Y);
                                    break;
                                }
                            case NetAcyncType.PlayCard:
                                {
                                    //Debug.Log("触发卡牌同步");
                                    int X = int.Parse(ReceiveInfo[2].ToString());
                                    int Y = int.Parse(ReceiveInfo[3].ToString());
                                    Command.CardCommand.PlayCard(Info.RowsInfo.GetCard(X, Y), false).Wait();
                                    break;
                                }
                            case NetAcyncType.FocusRegion:
                                {
                                    //Debug.Log("触发区域同步");
                                    int X = int.Parse(ReceiveInfo[2].ToString());
                                    Info.AgainstInfo.SelectRegion = Info.RowsInfo.GetSingleRowInfoById(X);
                                    break;
                                }

                            case NetAcyncType.FocusLocation:
                                {
                                    //Debug.Log("触发坐标同步");
                                    int X = int.Parse(ReceiveInfo[2].ToString());
                                    int Y = int.Parse(ReceiveInfo[3].ToString());
                                    //Info.RowsInfo.SingleRowInfos.First(infos => infos.ThisRowCard == Info.RowsInfo.GlobalCardList[X]);
                                    Info.AgainstInfo.SelectRegion = Info.RowsInfo.GetSingleRowInfoById(X);
                                    Info.AgainstInfo.SelectLocation = Y;
                                    //Debug.Log($"坐标为：{X}:{Y}");
                                    //Debug.Log($"信息为：{Info.GlobalBattleInfo.SelectRegion}:{Info.GlobalBattleInfo.SelectLocation}");

                                    break;
                                }
                            case NetAcyncType.SelectUnites:
                                {
                                    Debug.Log("收到同步单位信息为" + rawData);
                                    List<Location> Locations = ReceiveInfo[2].ToString().ToObject<List<Location>>();
                                    Info.AgainstInfo.SelectUnits.AddRange(Locations.Select(location => Info.RowsInfo.GetCard(location.x, location.y)));
                                    break;
                                }
                            case NetAcyncType.Pass:
                                {
                                    GameUI.UiCommand.SetCurrentPass();
                                    break;
                                }
                            case NetAcyncType.Surrender:
                                break;
                            case NetAcyncType.ExchangeCard:
                                {
                                    Debug.Log("交换卡牌信息");
                                    Debug.Log("收到信息" + rawData);

                                    Location location = ReceiveInfo[2].ToString().ToObject<Location>();
                                    int RandomRank = int.Parse(ReceiveInfo[3].ToString());
                                    _ = CardCommand.ExchangeCard(Info.RowsInfo.GetCard(location), false, RandomRank);
                                    break;
                                }
                            default:
                                break;
                        }
                    });
                    tcp.RegisterRawDataHandler("SurrenderRequir", (rawData, player) =>
                    {
                        _ = Command.StateCommand.BattleEnd(true, true);
                    });
                    tcp.RegisterRawDataHandler("RegistResult", (rawData, player) =>
                    {
                        int result = rawData.ParseToObject<GeneralCommand<int>>().Datas[0];
                        MainThread.Run(() =>
                        {
                            //打开弹窗
                            if (result == 1)
                            {
                                Debug.Log("注册成功");
                            }
                            if (result == -1)
                            {
                                Debug.Log("账号已存在");
                            }
                        });
                    });
                    tcp.RegisterRawDataHandler("LoginResult", (rawData, player) =>
                    {
                        string result = rawData.ParseToObject<GeneralCommand<string>>().Datas[0];
                        MainThread.Run(() =>
                        {
                            Info.AllPlayerInfo.UserInfo = result.ToObject<PlayerInfo>();
                            SceneManager.LoadSceneAsync(1);
                        });
                    });
                    tcp.RegisterRawDataHandler("JoinResult", (rawData, player) =>
                    {
                        Info.AllPlayerInfo.OpponentInfo = rawData.ParseToObject<PlayerInfo>();
                        Info.AgainstInfo.IsPVP = true;
                        MainThread.Run(() => { SceneManager.LoadSceneAsync(2); });
                    });
                };
            }
            public static void Register(string name, string password)
            {
                client.SendMessge("Register", new GeneralCommand<string>(name, password));
            }
            public static void Login(string name, string password)
            {
                client.SendMessge("Login", new GeneralCommand<string>(name, password));
            }
            public static void JoinRoom()
            {
                //Debug.Log(Info.AllPlayerInfo.UserInfo.ToJson());
                Info.AgainstInfo.IsPVP = true;
                client.SendMessge("Join", Info.AllPlayerInfo.UserInfo);
                //Debug.Log("发送完毕");
            }
            public static void Surrender()
            {
                client.SendMessge("Surrender", Info.AgainstInfo.RoomID);
            }
            public static void AsyncInfo(NetAcyncType AcyncType)
            {
                if (Info.AgainstInfo.IsPVP && (Info.AgainstInfo.IsMyTurn || AcyncType == NetAcyncType.FocusCard || AcyncType == NetAcyncType.ExchangeCard))
                {
                    switch (AcyncType)
                    {
                        case NetAcyncType.FocusCard:
                            {
                                Location TargetCardLocation = Info.AgainstInfo.PlayerFocusCard != null ? Info.AgainstInfo.PlayerFocusCard.Location : new Location(-1, -1);
                                client.SendMessge("AsyncInfo", new GeneralCommand(AcyncType, Info.AgainstInfo.RoomID, (int)TargetCardLocation.x, (int)TargetCardLocation.y));
                                break;
                            }
                        case NetAcyncType.PlayCard:
                            {
                                Location TargetCardLocation = Info.AgainstInfo.PlayerPlayCard.Location;
                                //Debug.Log("同步焦点卡片为" + TargetCardLocation);
                                client.SendMessge("AsyncInfo", new GeneralCommand(AcyncType, Info.AgainstInfo.RoomID, (int)TargetCardLocation.x, (int)TargetCardLocation.y));
                                break;
                            }
                        case NetAcyncType.FocusRegion:
                            {
                                int RowRank = Info.AgainstInfo.SelectRegion.RowRank;
                                Debug.Log("同步焦点区域为" + RowRank);
                                client.SendMessge("AsyncInfo", new GeneralCommand(AcyncType, Info.AgainstInfo.RoomID, (int)RowRank));
                                break;
                            }
                        case NetAcyncType.FocusLocation:
                            {
                                int RowRank = Info.AgainstInfo.SelectRegion.RowRank;
                                int LocationRank = Info.AgainstInfo.SelectLocation;
                                Debug.Log("同步焦点区域为" + RowRank);
                                client.SendMessge("AsyncInfo", new GeneralCommand(AcyncType, Info.AgainstInfo.RoomID, RowRank, LocationRank));
                                break;
                            }
                        case NetAcyncType.SelectUnites:
                            {
                                List<Location> Locations = Info.AgainstInfo.SelectUnits.Select(unite => unite.Location).ToList();
                                Debug.LogError("发出的指令为：" + new GeneralCommand(AcyncType, Info.AgainstInfo.RoomID, Locations).ToJson());
                                client.SendMessge("AsyncInfo", new GeneralCommand(AcyncType, Info.AgainstInfo.RoomID, Locations.ToJson()));
                                Debug.LogError("选择单位完成");
                                break;
                            }
                        case NetAcyncType.ExchangeCard:
                            {
                                Debug.Log("触发交换卡牌信息");
                                Location Locat = Info.AgainstInfo.TargetCard.Location;
                                int RandomRank = Info.AgainstInfo.RandomRank;
                                client.SendMessge("AsyncInfo", new GeneralCommand(AcyncType, Info.AgainstInfo.RoomID, Locat.ToJson(), RandomRank));
                                break;
                            }
                        case NetAcyncType.Pass:
                            {
                                client.SendMessge("AsyncInfo", new GeneralCommand(AcyncType, Info.AgainstInfo.RoomID));
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
    }
}