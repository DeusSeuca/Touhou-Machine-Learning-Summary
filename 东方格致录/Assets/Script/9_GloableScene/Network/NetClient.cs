using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;
using NetworkCommsDotNet.Connections.TCP;
using System.Net;
using UnityEngine;
namespace Network
{
    public class NetClient : MonoBehaviour
    {
        public static IPEndPoint ip = new IPEndPoint(IPAddress.Parse("47.100.119.84"), 39526);
        //static IPEndPoint ip = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 514);
        public static ConnectionInfo connInfo = new ConnectionInfo(ip);
        public static Connection Client = TCPConnection.GetConnection(connInfo);
    }
}