using CardSpace;
using Command;
using Info;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static GameEnum;

namespace Test
{
    public class TestScript : MonoBehaviour
    {

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(4))
            {
                
                Debug.Log("目标区域单位为"+GameCommand.GetCardList(LoadRangeOnBattle.My_Water).Count);

                //List<Card> Cards = GameCommand.GetCardList(LoadRangeOnBattle.My_Water);
                //Debug.Log("选择场上数量" + Cards.Count);
                //Debug.Log("选择场上单位" + Math.Min(Cards.Count, 3));
                //GlobalBattleInfo.IsWaitForSelectUnits = true;
                //GameCommand.GetCardList(LoadRangeOnBattle.My_Water).ForEach(card => card.IsTemp = true);
                //Cards.ForEach(card => card.IsTemp = false);
                //GlobalBattleInfo.SelectUnits.Clear();
                //await Task.Run(() =>
                //{害怕
                //    while (Info.GlobalBattleInfo.SelectUnits.Count < Math.Min(Cards.Count, num)) { }
                //});
                Debug.Log("选择单位完毕");
                //GameCommand.GetCardList(GameEnum.LoadRangeOnBattle.My_Water).ForEach(card => card.IsTemp = false);
                //GlobalBattleInfo.IsWaitForSelectUnits = false;

                //print(NewCard.name);
            }
            if (Input.GetMouseButtonDown(3))
            {
                //CardCommand.DrawCard();

                //Card b = a.ThisRowCard[0];
                //a.ThisRowCard.Remove(b);
                //Destroy(b.gameObject);
            }
        }
    }

}
