using Extension;
using Network.Converter;
using Network.Extensions;
using Network.Packets;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;
using NetworkCommsDotNet.Connections.TCP;
using System.Threading.Tasks;
using UnityEngine;

namespace Network
{
    public static class NetExtern_n
    {
        public static T ParseToObject<T>(this RawData rawdata)
        {
            return rawdata.ToUTF8String().ToObject<T>();
        }
        public static void SendMessge(this ClientConnectionContainer client, string Tag, object data)
        {
            if (Info.AgainstInfo.IsPVP)
            {
                client.Send(RawDataConverter.FromUTF8String(Tag, data.ToJson()));
            }
        }
    }
}