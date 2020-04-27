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

            cardAbility[TriggerTime.When][TriggerType.Play] = new List<Func<TriggerInfo, Task>>()
            {
                async (triggerInfo) =>
                {
                    await GameSystem.SelectSystem.SelectLocation(this);
                    await GameSystem.TransSystem.DeployCard(TriggerInfo.Build(this,this));
                }
            };
            cardAbility[TriggerTime.When][TriggerType.Deploy] = new List<Func<TriggerInfo, Task>>()
            {
                async (triggerInfo) =>
                {
                    
                    await GameSystem.SelectSystem.SelectUnite(this,cardSet[RegionTypes.Battle].CardList,1);
                    await GameSystem.PointSystem.Gain(TriggerInfo.Build(this,SelectUnits,1));
                },
            };
        }
    }
}