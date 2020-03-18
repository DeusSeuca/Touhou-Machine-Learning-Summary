using CardModel;
using Command;
using GameEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using static Info.AgainstInfo;
namespace CardSpace
{
    public class Card10002 : Card
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
                    Debug.LogError("10002打出效果触发完毕！");
                }
            };
            cardEffect[TriggerTime.When][TriggerType.Deploy] = new List<Func<TriggerInfo, Task>>()
            {
                async (triggerInfo) =>
                {
                    await GameSystem.SelectSystem.SelectUnite(this,cardSet[Orientation.My][RegionTypes.Battle][CardRank.Glod,CardRank.Leader,CardRank.Copper,CardRank.Silver].CardList,1);
                    await GameSystem.PointSystem.Cure(TriggerInfo.Build(this,SelectUnits));
                    Debug.Log("10002卡牌效果：重新部署单位"+SelectUnits.Count);
                    if (SelectUnits.Any())
                    {
                        SelectRegion=Info.RowsInfo.GetSingleRowInfoById(SelectUnits[0].location.x);
                        SelectLocation=SelectUnits[0].location.y;
                    }
                    Debug.LogError("10002选择完毕");
                    await GameSystem.TransSystem.DeployCard(TriggerInfo.Build(this,SelectUnits));
                    Debug.LogError("10002部署效果触发完毕！");
                },
                //async (triggerInfo) =>
                //{
                //    Debug.Log("10002卡牌效果：重新部署单位"+SelectUnits.Count);
                //    if (SelectUnits.Any())
                //    {
                //        SelectRegion=Info.RowsInfo.GetSingleRowInfoById(SelectUnits[0].location.x);
                //        SelectLocation=SelectUnits[0].location.y;
                //    }
                //    Debug.LogError("10002选择完毕");
                //    await GameSystem.TransSystem.DeployCard(TriggerInfo.Build(this,SelectUnits));
                //    Debug.LogError("10002部署效果触发完毕！");
                //}
            };
        }
        private void OnMouseDown()
        {
            //GameSystem.TransSystem.DeployCard(TriggerInfo.Build(this, this)).GetAwaiter().GetResult();

        }
    }

}
//await Task.Delay(300);
////EffectCommand.TheWorldPlay(this);
//await Task.Delay(2000);