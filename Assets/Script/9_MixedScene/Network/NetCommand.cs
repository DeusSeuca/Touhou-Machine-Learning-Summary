using Extension;
using GameEnum;
using Network;
using System.Collections.Generic;
using System.Linq;
using Thread;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Network.NetInfoModel;
using WebSocketSharp;
using System.Threading.Tasks;

namespace Command
{

    namespace Network
    {
        public static class NetCommand
        {
            static string ip = "127.0.0.1:514";
            //static string ip = "106.15.38.165:514";
            static WebSocket AsyncConnect = new WebSocket($"ws://{ip}/AsyncInfo");
            public static void Init()
            {


                //var clients = new WebSocket($"ws://{ip}/Join");
                //clients.OnMessage += (sender, e) =>
                //{
                //    Debug.Log("收到回应: " + e.Data);
                //};
                //clients.Connect();
                //Debug.Log("加入房间");
            }
            public static void Dispose()
            {
                Debug.Log("释放网络资源");
                //clients.Close();
            }
            public static void Register(string name, string password)
            {
                Debug.Log("注册请求");
                var client = new WebSocket($"ws://{ip}/Register");
                client.OnMessage += (sender, e) =>
                {
                    Debug.Log("有回应了");
                    Debug.Log(e.Data);
                    string result = e.Data;
                    MainThread.Run(() =>
                    {
                        //打开弹窗
                        if (result == "1")
                        {
                            Debug.Log("注册成功");
                        }
                        if (result == "-1")
                        {
                            Debug.Log("账号已存在");
                        }
                    });
                    Debug.Log("收到回应: " + e.Data);
                    client.Close();
                };
                client.Connect();
                client.Send(new GeneralCommand<string>(name, password).ToJson());

            }
            public static void Login(string name, string password)
            {
                Debug.Log("登录请求");
                var client = new WebSocket($"ws://{ip}/Login");
                client.OnMessage += (sender, e) =>
                {
                    MainThread.Run(() =>
                    {
                        string result = e.Data.ToObject<GeneralCommand<string>>().Datas[0];
                        string playerInfo = e.Data.ToObject<GeneralCommand<string>>().Datas[1];
                        Debug.Log("登录结果" + result);
                        Info.AllPlayerInfo.UserInfo = playerInfo.ToObject<PlayerInfo>();
                        SceneManager.LoadSceneAsync(1);
                    });
                    Debug.Log("收到回应: " + e.Data);
                    client.Close();
                };
                client.Connect();
                Debug.Log("连接完成");
                client.Send(new GeneralCommand<string>(name, password).ToJson());
            }
            public static void JoinRoom()
            {
                Debug.Log("登录请求");
                var client = new WebSocket($"ws://{ip}/Join");
                client.OnMessage += (sender, e) =>
                {
                    Debug.LogError("收到了来自服务器的初始信息" + e.Data);
                    object[] ReceiveInfo = e.Data.ToObject<GeneralCommand>().Datas;
                    Info.AgainstInfo.RoomID = int.Parse(ReceiveInfo[0].ToString());
                    Debug.Log("房间号为" + Info.AgainstInfo.RoomID);
                    Info.AgainstInfo.isPlayer1 = (bool)ReceiveInfo[1];
                    Debug.Log("是否玩家1？：" + Info.AgainstInfo.isPlayer1);
                    Info.AgainstInfo.IsPVP = true;
                    Info.AgainstInfo.IsMyTurn = Info.AgainstInfo.isPlayer1;
                    Info.AllPlayerInfo.UserInfo = ReceiveInfo[2].ToString().ToObject<PlayerInfo>();
                    Info.AllPlayerInfo.OpponentInfo = ReceiveInfo[3].ToString().ToObject<PlayerInfo>();
                    Debug.Log("收到回应: " + e.Data);
                    //client.Close();
                    MainThread.Run(() =>
                    {
                        SceneManager.LoadSceneAsync(2);
                        InitAsyncConnection();
                    });
                };
                client.Connect();
                Debug.Log("连接完成");
                client.Send(Info.AllPlayerInfo.UserInfo.ToJson());
                Debug.Log(Info.AllPlayerInfo.UserInfo.ToJson());
                Debug.Log("发送完毕");
            }

            private static void InitAsyncConnection()
            {
                AsyncConnect = new WebSocket($"ws://{ip}/AsyncInfo");
                AsyncConnect.Connect();
                AsyncConnect.OnMessage += (sender, e) =>
                {
                    Debug.Log("收到信息" + e.Data);
                    object[] ReceiveInfo = e.Data.ToObject<GeneralCommand>().Datas;
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
                        case NetAcyncType.SelectRegion:
                            {
                                //Debug.Log("触发区域同步");
                                int X = int.Parse(ReceiveInfo[2].ToString());
                                Info.AgainstInfo.SelectRegion = Info.RowsInfo.GetSingleRowInfoById(X);
                                break;
                            }
                        case NetAcyncType.SelectLocation:
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
                                //Debug.Log("收到同步单位信息为" + rawData);
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
                            {
                                Task.Run(async () =>
                                {
                                    await StateCommand.BattleEnd(true, true);
                                });
                                break;
                            }
                        case NetAcyncType.ExchangeCard:
                            {
                                Debug.Log("交换卡牌信息");
                                // Debug.Log("收到信息" + rawData);
                                Location location = ReceiveInfo[2].ToString().ToObject<Location>();
                                int RandomRank = int.Parse(ReceiveInfo[3].ToString());
                                _ = CardCommand.ExchangeCard(Info.RowsInfo.GetCard(location), false, RandomRank);
                                break;
                            }
                        case NetAcyncType.SelectProperty:
                            {
                                Info.AgainstInfo.SelectProperty = (Region)int.Parse(ReceiveInfo[1].ToString());
                                Debug.Log("通过网络同步当前属性为" + Info.AgainstInfo.SelectProperty);
                                break;
                            }
                        default:
                            break;
                    }
                };
                AsyncConnect.OnError += (sender, e) =>
                {
                    Debug.Log("连接失败" + e.Message);
                    Debug.Log("连接失败" + e.Exception);
                };
                Debug.LogError("初始化数据" + new GeneralCommand(NetAcyncType.Init, Info.AgainstInfo.RoomID, Info.AgainstInfo.isPlayer1).ToJson());
                AsyncConnect.Send(new GeneralCommand(NetAcyncType.Init, Info.AgainstInfo.RoomID, Info.AgainstInfo.isPlayer1).ToJson());

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
                                AsyncConnect.Send(new GeneralCommand(AcyncType, Info.AgainstInfo.RoomID, (int)TargetCardLocation.x, (int)TargetCardLocation.y).ToJson());
                                break;
                            }
                        case NetAcyncType.PlayCard:
                            {
                                Location TargetCardLocation = Info.AgainstInfo.PlayerPlayCard.Location;
                                //Debug.Log("同步焦点卡片为" + TargetCardLocation);
                                AsyncConnect.Send(new GeneralCommand(AcyncType, Info.AgainstInfo.RoomID, (int)TargetCardLocation.x, (int)TargetCardLocation.y).ToJson());

                                break;
                            }
                        case NetAcyncType.SelectRegion:
                            {
                                int RowRank = Info.AgainstInfo.SelectRegion.RowRank;
                                Debug.Log("同步焦点区域为" + RowRank);
                                AsyncConnect.Send(new GeneralCommand(AcyncType, Info.AgainstInfo.RoomID, (int)RowRank).ToJson());

                                break;
                            }
                        case NetAcyncType.SelectLocation:
                            {
                                int RowRank = Info.AgainstInfo.SelectRegion.RowRank;
                                int LocationRank = Info.AgainstInfo.SelectLocation;
                                Debug.Log("同步焦点区域为" + RowRank);
                                AsyncConnect.Send(new GeneralCommand(AcyncType, Info.AgainstInfo.RoomID, RowRank, LocationRank).ToJson());
                                break;
                            }
                        case NetAcyncType.SelectUnites:
                            {
                                List<Location> Locations = Info.AgainstInfo.SelectUnits.Select(unite => unite.Location).ToList();
                                Debug.LogError("发出的指令为：" + new GeneralCommand(AcyncType, Info.AgainstInfo.RoomID, Locations).ToJson());
                                AsyncConnect.Send(new GeneralCommand(AcyncType, Info.AgainstInfo.RoomID, Locations.ToJson()).ToJson());
                                Debug.LogError("选择单位完成");
                                break;
                            }
                        case NetAcyncType.ExchangeCard:
                            {
                                Debug.Log("触发交换卡牌信息");
                                Location Locat = Info.AgainstInfo.TargetCard.Location;
                                int RandomRank = Info.AgainstInfo.RandomRank;
                                AsyncConnect.Send(new GeneralCommand(AcyncType, Info.AgainstInfo.RoomID, Locat.ToJson(), RandomRank).ToJson());
                                break;
                            }
                        case NetAcyncType.Pass:
                            {
                                Debug.Log("pass");
                                AsyncConnect.Send(new GeneralCommand(AcyncType, Info.AgainstInfo.RoomID).ToJson());
                                break;
                            }
                        case NetAcyncType.Surrender:
                            {
                                Debug.Log("投降");
                                AsyncConnect.Send(new GeneralCommand(AcyncType, Info.AgainstInfo.RoomID).ToJson());
                                break;
                            }
                        case NetAcyncType.SelectProperty:
                            {
                                Debug.Log("选择场地属性");
                                AsyncConnect.Send(new GeneralCommand(AcyncType, Info.AgainstInfo.SelectProperty).ToJson());
                                break;
                            }
                        default:
                            {
                                Debug.Log("异常同步指令");
                            }
                            break;
                    }
                }
            }
        }
    }
}