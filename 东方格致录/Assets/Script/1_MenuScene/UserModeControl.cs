using UnityEngine;
using UnityEngine.SceneManagement;
namespace Control
{
    public class UserModeControl : MonoBehaviour
    {
        public void JoinRoom() => Command.NetCommand.JoinRoom();
        public void CreatSingleRoom() => SceneManager.LoadSceneAsync(2);
    }
}