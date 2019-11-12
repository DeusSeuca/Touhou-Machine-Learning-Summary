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
            UserLogin();//自动登录
        }
        public void UserRegister()
        {
            GeneralCommand<int> msg = Command.Network.NetCommand.Register(UserName.text, Password.text).ToObject<GeneralCommand<int>>();
            if (msg.Datas[0] == 1)
            {
                print("注册成功");
            }
            if (msg.Datas[0] == -1)
            {
                print("账号已存在");
            }
        }
        public void UserLogin()
        {
            GeneralCommand<string> msg = Command.Network.NetCommand.Login(UserName.text, Password.text).ToObject<GeneralCommand<string>>();
            Info.AllPlayerInfo.UserInfo = msg.Datas[1].ToObject<PlayerInfo>();
            SceneManager.LoadSceneAsync(1);
        }
    }
}