using CardModel;
using Command;
using GameEnum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Info.AgainstInfo;
namespace CardSpace
{
    public class Card0 : Card
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
        }
    }
}