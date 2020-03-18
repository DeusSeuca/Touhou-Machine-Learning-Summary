using CardModel;
using Command;
using GameEnum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Info.AgainstInfo;
namespace CardSpace
{
    public class Card10009 : Card
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
                    for (int i = 0; i < 1+twoSideVitality; i++)
                    {
                        await GameSystem.SelectSystem.SelectUnite(this,cardSet[RegionTypes.Battle].CardList,1,false);
                        await GameSystem.StateSystem.SealCard(new TriggerInfo(this,SelectUnits,1));
                        await GameSystem.PointSystem.Hurt(new TriggerInfo(this,SelectUnits,1));
                    }
                }
            };
        }
    }
}