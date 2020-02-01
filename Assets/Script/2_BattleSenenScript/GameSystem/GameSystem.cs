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
        //这个为啥没执行啊= =
        public static async Task Gain(TriggerInfo triggerInfo) => await TriggerLogic(triggerInfo[TriggerType.Gain]);
        public static async Task Hurt(TriggerInfo triggerInfo) => await TriggerLogic(triggerInfo[TriggerType.Hurt]);
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
            await Command.CardCommand.DeployCard(triggerInfo.triggerCard, SelectRegion, SelectLocation);
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
        public static async Task SelectUnite(Card card, List<Card> targetCards, int num) => await Command.StateCommand.WaitForSelecUnit(card, targetCards, num);
        public static async Task SelectLocation(Card card) => await Command.StateCommand.WaitForSelectLocation(card);
    }
    //好像不需要
    public class TurnSystem
    {
        public static async Task WhenRoundStart()
        {
            
        }
        public static async Task WhenRoundEnd()
        {
            Debug.Log("出发了回合结束");
            await TriggerLogic(TriggerInfo.Build(null, cardSet[RegionTypes.Battle].cardList)[TriggerType.RoundEnd]);
        }
        public static async Task WhenTurnStart()
        {

        }
        public static async Task WhenTurnEnd()
        {
            await TriggerLogic(TriggerInfo.Build(null, targetCard: null)[TriggerType.TurnEnd]);

            //await CardEffectStackControl.Trigger(TriggerTime.When, TriggerType.TurnEnd, cardSet[RegionTypes.Battle].cardList);

            //foreach (var card in Info.AgainstInfo.cardSet[Orientation.Down][RegionTypes.Battle].cardList)
            //{

            //    await Command.CardCommand.RemoveFromBattle(card, Orientation.Down);
            //    await Task.Delay(150);
            //}
            //foreach (var card in cardSet[RegionTypes.Battle].cardList)
            //{
            //    //await Command.CardCommand.RemoveFromBattle(card);
            //    await Task.Delay(150);
            //}
            //await CardEffectStackControl.Trigger_NewAsync<TriggerType.AfterDisCard>(cardSet.BroastCardList(card));
        }
    }
}