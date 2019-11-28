using UnityEngine;
using UnityEngine.SceneManagement;
namespace Control
{
    public class UserModeControl : MonoBehaviour
    {
        private void Start() => JoinRoom();
        public  void JoinRoom() => Command.Network.NetCommand.JoinRoom();
        public static void CreatSingleRoom() => SceneManager.LoadSceneAsync(2);
        private void OnApplicationQuit() => Command.Network.NetCommand.Dispose();
    }
}