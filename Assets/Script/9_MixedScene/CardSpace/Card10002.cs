using CardModel;
using Command;
using GameEnum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
                }
            };
            cardEffect[TriggerTime.When][TriggerType.Deploy] = new List<Func<TriggerInfo, Task>>()
            {
                async (triggerInfo) =>
                {
                    //await GameSystem.SelectSystem.SelectUnite(this,cardSet[Orientation.My][RegionTypes.Battle][CardRank.Copper,CardRank.Silver].cardList,1);
                    //await GameSystem.PointSystem.Cure(TriggerInfo.Build(this,SelectUnits));
                },
                async (triggerInfo) =>
                {
                     //await GameSystem.TransSystem.DeployCard(TriggerInfo.Build(this,SelectUnits));
                }
            };
        }
    }
}
//await Task.Delay(300);
////EffectCommand.TheWorldPlay(this);
//await Task.Delay(2000);