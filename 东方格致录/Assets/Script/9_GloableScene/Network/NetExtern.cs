using NetworkCommsDotNet.Connections;
using Newtonsoft.Json;
//using Newtonsoft.Json;
using UnityEngine;

public static class NetExtern
{
    public static string ToJson(this object target) => JsonConvert.SerializeObject(target);
    public static T ToObject<T>(this string Data) =>JsonConvert.DeserializeObject<T>(Data);
    public static void SendMessge(this Connection con, string Tag, object Info) => con.SendObject(Tag, Info.ToJson());
    public static string SendReceiveMessge(this Connection con, string SengTag, string ReceiveTag, object Info, int LimitTime = 5) => con.SendReceiveObject<string, string>(SengTag, ReceiveTag, LimitTime * 1000, Info.ToJson());
}
