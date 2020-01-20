using CardModel;
using Command;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static Info.AgainstInfo;
using GameEnum;
namespace CardSpace
{
    public class Card1004 : Card
    {
        public override void Init()
        {
            base.Init();
            
            cardEffect_Play = new List<Func<Card, Task>>()
            {
                async (triggerCard) =>
                {
                    await GameSystem.SelectSystem.SelectLocation(this);
                    await GameSystem.TransSystem.DeployCard(this);
                }
            };
            cardEffect_WhenDeploy= new List<Func<Card, Task>>()
            {
                async (triggerCard) =>
                {
                    await GameSystem.SelectSystem.SelectUnite(this,cardSet[Orientation.My][RegionTypes.Battle].cardList,1);
                    await GameSystem.PointSystem.Gain(SelectUnits,5);
                },
                async (triggerCard) =>
                {
                    await GameSystem.SelectSystem.SelectUnite(this,cardSet[Orientation.My][RegionTypes.Battle].cardList,1);
                    //await GameSystem.TransSystem.RecycleCard(SelectUnits);
                }
            };
        }
    }
}
//cardEffect_Dead = new List<Func<Card, Task>>()
//{
//    async () =>
//    {
//        await GameSystem.SelectSystem.SelectUnite(this,cardSet[Orientation.My].cardList,2);
//        await GameSystem.PointSystem.Hurt(SelectUnits,5);
//    }
//};
//[TriggerType.PlayCard]
//public Func<Task> Step1 => async () => { print("test"); await Task.Delay(1000); };

//[TriggerType.PlayCard]
//public Func<Task> Step2 => async () =>
//{
//    await StateCommand.WaitForSelectLocation(this);
//    await Deploy();
//    await Task.Delay(100);
//};
//[TriggerType.Deploy]
//public Func<Task> Step3 => async () =>
//{
//    await StateCommand.WaitForSelectLocation(this);
//    GameSystem.PointSystem.Hurt(this, 5);
//    await Task.Delay(100);
//};