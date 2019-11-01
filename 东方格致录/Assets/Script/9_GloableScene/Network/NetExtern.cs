using Extension;
using NetworkCommsDotNet.Connections;
namespace Network
{
    public static class NetExtern
    {
        public static void SendMessge(this Connection con, string Tag, object data)
        {
            if (Info.AgainstInfo.IsPVP)
            {
                con.SendObject(Tag, data.ToJson());
            }
        }
        public static string SendReceiveMessge(this Connection con, string SengTag, string ReceiveTag, object Info, int LimitTime = 5)
        {
            return con.SendReceiveObject<string, string>(SengTag, ReceiveTag, LimitTime * 1000, Info.ToJson());
        }
    }
}