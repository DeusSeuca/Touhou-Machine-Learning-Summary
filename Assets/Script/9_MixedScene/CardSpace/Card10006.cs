using CardModel;
using Command;
using GameEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Info.AgainstInfo;
namespace CardSpace
{
    public class Card10006 : Card
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
                    if (!this[CardState.Seal])
                    {
                        List<Card> targetCardList= cardSet[Orientation.My][RegionTypes.Deck].CardList.Where(card=>card.CardId==10007||card.CardId==10008).ToList();
                        await GameSystem.TransSystem.SummonCard(new TriggerInfo(this,targetCardList));
                    }
                }
            };
        }
    }
}