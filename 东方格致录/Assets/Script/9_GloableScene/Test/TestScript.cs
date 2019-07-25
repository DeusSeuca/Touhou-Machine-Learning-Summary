using System.Threading.Tasks;
using UnityEngine;
namespace Test
{
    public class TestScript : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetMouseButtonDown(4))
            {

            }
            if (Input.GetMouseButtonDown(3))
            {
                Task.Run(() =>
                {
                    Command.NetCommand.AsyncInfoRequir(null, null, "{\"Datas\":[1,9,1,0]}");
                }).Wait();
            }
        }
    }

}
