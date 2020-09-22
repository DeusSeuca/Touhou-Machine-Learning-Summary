﻿using Extension;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Network.NetInfoModel;
namespace Control
{
    public class UserLoginControl : MonoBehaviour
    {
        public GameObject loginCanvas;
        public Text UserName;
        public Text Password;
        void Start() => Command.Network.NetCommand.Init();//Command.Network.NetCommand.Login("", "");//UserLogin();//自动登录
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                //BookControl.ClosePageAll();
                CameraViewControl.MoveToInitView();
                loginCanvas.SetActive(true);
            }
        }
        public void UserRegister() => Command.Network.NetCommand.Register(UserName.text, Password.text);
        public void UserLogin()
        {
            //Command.Network.NetCommand.Login(UserName.text, Password.text);
            BookControl.OpenToPage(1);
            CameraViewControl.MoveToBookView();
            loginCanvas.SetActive(false);
        }

        private void OnApplicationQuit() => Command.Network.NetCommand.Dispose();
    }
}