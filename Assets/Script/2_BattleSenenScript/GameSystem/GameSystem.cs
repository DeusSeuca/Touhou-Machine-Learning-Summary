using CardModel;
using CardSpace;
using Control;
using GameEnum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using static Info.AgainstInfo;
using static Control.CardEffectStackControl;
namespace GameSystem
{
    /// <summary>
    /// 改变卡牌点数的相关机制
    /// </summary>
    public class PointSystem
    {
        public static async Task Gain(TriggerInfo triggerInfo) => await TriggerLogic(triggerInfo[TriggerType.Gain]);
        public static async Task Hurt(TriggerInfo triggerInfo) => await TriggerLogic(triggerInfo[TriggerType.Hurt]);
        public static async Task Cure(TriggerInfo triggerInfo) => await TriggerLogic(triggerInfo[TriggerType.Cure]);
        public static async Task Reset(TriggerInfo triggerInfo) => await TriggerLogic(triggerInfo[TriggerType.Reset]);
        public static async Task Destory(TriggerInfo triggerInfo) => await TriggerLogic(triggerInfo[TriggerType.Destory]);
        public static async Task Strengthen(TriggerInfo triggerInfo) => await TriggerLogic(triggerInfo[TriggerType.Strengthen]);
        public static async Task Impair(TriggerInfo triggerInfo) => await TriggerLogic(triggerInfo[TriggerType.Impair]);
        
    }
    /// <summary>
    /// 转移卡牌位置、所属区域的相关机制
    /// </summary>
    public class TransSystem
    {
        public static async Task DrawCard(TriggerInfo triggerInfo) => await TriggerLogic(triggerInfo[TriggerType.Draw]);
        public static async Task PlayCard(TriggerInfo triggerInfo)
        {
            await Command.CardCommand.PlayCard(triggerInfo.targetCard);
            await TriggerLogic(triggerInfo[TriggerType.Play]);
        }
        public static async Task RecycleCard(List<Card> card)
        {

        }
        public static async Task DeployCard(TriggerInfo triggerInfo)
        {
            await Command.CardCommand.DeployCard(triggerInfo.targetCard, SelectRegion, SelectLocation);
            await TriggerLogic(triggerInfo[TriggerType.Deploy]);
            //Info.AgainstInfo.IsCardEffectCompleted = true;
        }
        public static async Task BanishCard(TriggerInfo triggerInfo) => await TriggerLogic(triggerInfo[TriggerType.Banish]);
        public static async Task Discard(TriggerInfo triggerInfo) => await TriggerLogic(triggerInfo[TriggerType.Discard]);
    }
    /// <summary>
    /// 选择单位、区域、场景属性的相关机制
    /// </summary>
    public class SelectSystem
    {
        public static async Task SelectUnite(Card card, List<Card> targetCards, int num,bool isAuto=false) => await Command.StateCommand.WaitForSelecUnit(card, targetCards, num, isAuto);
        public static async Task SelectLocation(Card card) => await Command.StateCommand.WaitForSelectLocation(card);
    }
    //好像不需要
    public class TurnSystem
    {
        public static async Task WhenTurnStart() => await TriggerLogic(TriggerInfo.Build(null, targetCard: null)[TriggerType.TurnStart]);
        public static async Task WhenTurnEnd() => await TriggerLogic(TriggerInfo.Build(null, targetCard: null)[TriggerType.TurnEnd]);
        public static async Task WhenRoundStart() => await TriggerLogic(TriggerInfo.Build(null, cardSet[RegionTypes.Battle].cardList)[TriggerType.RoundStart]);
        public static async Task WhenRoundEnd() => await TriggerLogic(TriggerInfo.Build(null, cardSet[RegionTypes.Battle].cardList)[TriggerType.RoundEnd]);
       
    }
}