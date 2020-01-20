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
                }
            };
        }
    }
}