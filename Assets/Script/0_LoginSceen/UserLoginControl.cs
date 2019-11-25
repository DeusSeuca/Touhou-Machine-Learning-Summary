using Extension;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Network.NetInfoModel;
namespace Control
{
    public class UserLoginControl : MonoBehaviour
    {
        public Text UserName;
        public Text Password;
        void Start()
        {
            Command.Network.NetCommand.Init();
            //Command.Network.NetCommand.Login("", "");

            //UserLogin();//自动登录
        }
        public void UserRegister()
        {
            Command.Network.NetCommand.Register(UserName.text, Password.text);
        }
        public void UserLogin()
        {
            Command.Network.NetCommand.Login(UserName.text, Password.text);

        }
        private void OnApplicationQuit()
        {
            Command.Network.NetCommand.Dispose();
        }
    }
}