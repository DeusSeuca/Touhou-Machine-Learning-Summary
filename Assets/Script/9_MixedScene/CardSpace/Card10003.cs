using CardModel;
using Command;
using GameEnum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Info.AgainstInfo;
namespace CardSpace
{
    public class Card10003 : Card
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
                    await GameSystem.SelectSystem.SelectUnite(this,cardSet[RegionTypes.Battle].cardList,1);
                    await GameSystem.PointSystem.Gain(TriggerInfo.Build(this,SelectUnits,1));
                },
                async (triggerInfo) =>
                {
                    await GameSystem.SelectSystem.SelectUnite(this,cardSet[RegionTypes.Battle].cardList,1);
                    await GameSystem.PointSystem.Gain(TriggerInfo.Build(this,SelectUnits,1));
                },
                async (triggerInfo) =>
                {
                    await GameSystem.SelectSystem.SelectUnite(this,cardSet[RegionTypes.Battle].cardList,1);
                    await GameSystem.PointSystem.Gain(TriggerInfo.Build(this,SelectUnits,1));
                },
                async (triggerInfo) =>
                {
                    await GameSystem.SelectSystem.SelectUnite(this,cardSet[RegionTypes.Battle].cardList,1);
                    await GameSystem.PointSystem.Gain(TriggerInfo.Build(this,SelectUnits,1));
                },
                async (triggerInfo) =>
                {
                    await GameSystem.SelectSystem.SelectUnite(this,cardSet[RegionTypes.Battle].cardList,1);
                    await GameSystem.PointSystem.Gain(TriggerInfo.Build(this,SelectUnits,1));
                },
            };
        }
    }
}