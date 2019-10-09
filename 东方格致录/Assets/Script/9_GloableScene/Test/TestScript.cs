using GameEnum;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Test
{
    public class TestScript : MonoBehaviour
    {
        public int num;
        void Update()
        {
            if (Input.GetMouseButtonDown(4))
            {
                Task.Run(() =>
                {
                    print("等待选择位置");
                    Command.StateCommand.WaitForSelectLocation(new CardSpace.Card()).Wait();
                    print("开始部署");
                    //Command.NetCommand.AsyncInfoRequir(null, null, "{\"Datas\":[3,9,1,0]}");
                }).Wait();
            }
            if (Input.GetMouseButtonDown(3))
            {

                //string Data = "{\"Datas\":[4,0,[{\"x\":5,\"y\":0}]]}";

                //Command.NetCommand.AsyncInfoRequir(null, null, "{ \"Datas\":[4,9,[{\"x\":5,\"y\":0}]]}");
                //Command.CardCommand.DisCard(Info.RowsInfo.GetMyCardList(RegionTypes.Hand)[0]);
                //IEnumerator DelayCoroutine()
                //{
                //    TcpClient Client = new TcpClient();
                //    Client.Client.Accept();
                //    yield return new WaitForSeconds(0);
                //}
                //Task.Run(() =>
                //{
                //    Command.NetCommand.AsyncInfoRequir(null, null, "{\"Datas\":[1,9,1,0]}");
                //}).Wait();
            }

        }
        public List<CardSpace.Card> Cards1;
        public List<CardSpace.Card> Cards2;
        public List<CardSpace.Card> Cards3;
        [Button]
        private void Filter(Orientation orientation,int lessPoint)
        {
            Cards1 = Info.AgainstInfo.AllCardList.At(orientation);
            Cards2 = Info.AgainstInfo.AllCardList.LessPoint(lessPoint);
            Cards3 = Info.AgainstInfo.AllCardList.At(orientation).LessPoint(lessPoint);
            //num = Info.AgainstInfo.AllCardList.Count;
            //num = Info.AgainstInfo.AllCardList.hasTag(Text).Count;
            //Command.RowCommand.GetCardList(Text);
        }
    }

}


