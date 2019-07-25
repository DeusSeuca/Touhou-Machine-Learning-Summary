using UnityEngine;
using UnityEngine.SceneManagement;

namespace Control
{


    public class UserModeControl : MonoBehaviour
    {
        public static bool IsJoinRoom = false;
        public void JoinRoom() => Command.NetCommand.JoinRoom();
        public void CreatSingleRoom() => SceneManager.LoadSceneAsync(2);

        void Update()
        {
            if (IsJoinRoom)
            {
               
                IsJoinRoom = false;
            }
        }
    }
}