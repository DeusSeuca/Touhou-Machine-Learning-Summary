using CardModel;
using Command;
using GameEnum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static Info.AgainstInfo;
namespace CardSpace
{
    public class Card10004 : Card
    {
        public override void Init()
        {
            base.Init();

            cardEffect[TriggerTime.When][TriggerType.Play] = new List<Func<TriggerInfo, Task>>()
            {
                async (triggerInfo) =>
                {
                    await GameSystem.SelectSystem.SelectLocation(this);
                    await GameSystem.TransSystem.DeployCard(TriggerInfo.Build(this,this));
                }
            };
            cardEffect[TriggerTime.When][TriggerType.Deploy] = new List<Func<TriggerInfo, Task>>()
            {
                async (triggerInfo) =>
                {
                    //for (int i = 0; i < 3; i++)
                    //{
                    //    Debug.Log("①等待选择一个单位");
                    //    await GameSystem.SelectSystem.SelectUnite(this,cardSet[Orientation.Op][RegionTypes.Battle][CardFeature.Largest].cardList,1,false);
                    //    Debug.Log("②对其造成伤害");
                    //    await GameSystem.PointSystem.Hurt(TriggerInfo.Build(this,SelectUnits,1));
                    //    Debug.Log("③伤害完毕");
                    //}
                    Debug.Log("①等待选择一个单位");
                    await GameSystem.SelectSystem.SelectUnite(this,cardSet[Orientation.Op][RegionTypes.Battle][CardFeature.Largest].cardList,1,false);
                    Debug.Log("②对其造成伤害");
                    await GameSystem.PointSystem.Hurt(TriggerInfo.Build(this,SelectUnits,1));
                    Debug.Log("③伤害完毕");
                    Debug.Log("①等待选择一个单位");
                    await GameSystem.SelectSystem.SelectUnite(this,cardSet[Orientation.Op][RegionTypes.Battle][CardFeature.Largest].cardList,1,false);
                    Debug.Log("②对其造成伤害");
                    await GameSystem.PointSystem.Hurt(TriggerInfo.Build(this,SelectUnits,1));
                    Debug.Log("③伤害完毕");
                    Debug.Log("①等待选择一个单位");
                    await GameSystem.SelectSystem.SelectUnite(this,cardSet[Orientation.Op][RegionTypes.Battle][CardFeature.Largest].cardList,1,false);
                    Debug.Log("②对其造成伤害");
                    await GameSystem.PointSystem.Hurt(TriggerInfo.Build(this,SelectUnits,1));
                    Debug.Log("③伤害完毕");
                }
            };
        }
    }
}
//twoSideVitality+1+2