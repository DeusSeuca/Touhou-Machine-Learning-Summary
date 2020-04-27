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

            cardAbility[TriggerTime.When][TriggerType.Play] = new List<Func<TriggerInfo, Task>>()
            {
                async (triggerInfo) =>
                {
                    await GameSystem.SelectSystem.SelectLocation(this);
                    await GameSystem.TransSystem.DeployCard(TriggerInfo.Build(this,this));
                    Debug.LogError("10002���Ч��������ϣ�");
                }
            };
            cardAbility[TriggerTime.When][TriggerType.Deploy] = new List<Func<TriggerInfo, Task>>()
            {
                async (triggerInfo) =>
                {
                    await GameSystem.SelectSystem.SelectUnite(this,cardSet[Orientation.My][RegionTypes.Battle][CardRank.Glod,CardRank.Leader,CardRank.Copper,CardRank.Silver].CardList,1);
                    await GameSystem.PointSystem.Cure(TriggerInfo.Build(this,SelectUnits));
                    Debug.Log("10002����Ч�������²���λ"+SelectUnits.Count);
                    if (SelectUnits.Any())
                    {
                        SelectRegion=Info.RowsInfo.GetSingleRowInfoById(SelectUnits[0].location.x);
                        SelectLocation=SelectUnits[0].location.y;
                    }
                    Debug.LogError("10002ѡ�����");
                    await GameSystem.TransSystem.DeployCard(TriggerInfo.Build(this,SelectUnits));
                    Debug.LogError("10002����Ч��������ϣ�");
                },
                //async (triggerInfo) =>
                //{
                //    Debug.Log("10002����Ч�������²���λ"+SelectUnits.Count);
                //    if (SelectUnits.Any())
                //    {
                //        SelectRegion=Info.RowsInfo.GetSingleRowInfoById(SelectUnits[0].location.x);
                //        SelectLocation=SelectUnits[0].location.y;
                //    }
                //    Debug.LogError("10002ѡ�����");
                //    await GameSystem.TransSystem.DeployCard(TriggerInfo.Build(this,SelectUnits));
                //    Debug.LogError("10002����Ч��������ϣ�");
                //}
            };
        }
    }
}
//await Task.Delay(300);
////EffectCommand.TheWorldPlay(this);
//await Task.Delay(2000);