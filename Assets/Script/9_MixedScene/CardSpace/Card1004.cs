using CardModel;
using Command;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static Info.AgainstInfo;
using GameEnum;
using Thread;

namespace CardSpace
{
    public class Card1004 : Card
    {
        //初始化一张卡牌1004
        public override void Init()
        {
            base.Init();
            //当被打出时触发的效果
            cardEffect[TriggerTime.When][TriggerType.Play] = new List<Func<TriggerInfo, Task>>()
            {
                async (triggerInfo) =>
                {
                    //选择一个位置
                    await GameSystem.SelectSystem.SelectLocation(this);
                    //部署到这个位置
                    await GameSystem.TransSystem.DeployCard(TriggerInfo.Build(this,this));
                }
            };
            //当被部署时触发的效果
            cardEffect[TriggerTime.When][TriggerType.Deploy] = new List<Func<TriggerInfo, Task>>()
            {
                async (triggerInfo) =>
                {
                    //选择一个单位
                    await GameSystem.SelectSystem.SelectUnite(this,cardSet[Orientation.My][RegionTypes.Battle].cardList,3);
                    if (SelectUnits.Count>0)
                    {
                            //await GameSystem.PointSystem.Gain(TriggerInfo.Build(this,SelectUnits,1));
                        await GameSystem.TransSystem.BanishCard(TriggerInfo.Build(this,SelectUnits));

                        //Debug.Log("未选择的目标触发效果");
                        //for (int i = 1; i < 10; i++)
                        //{
                        //    await GameSystem.PointSystem.Gain(TriggerInfo.Build(this,SelectUnits,1));
                        //   // await Task.Delay(2000);
                        //}
                    }
                },
                async (triggerInfo) =>
                {
                    //await GameSystem.SelectSystem.SelectUnite(this,cardSet[Orientation.My][RegionTypes.Battle].cardList,1);
                }
            };
        }
    }
}