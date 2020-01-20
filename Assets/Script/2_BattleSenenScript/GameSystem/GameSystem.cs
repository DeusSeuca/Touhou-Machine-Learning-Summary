using CardModel;
using CardSpace;
using Control;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using static Info.AgainstInfo;
namespace GameSystem
{
    /// <summary>
    /// 改变卡牌点数的相关机制
    /// </summary>
    public class PointSystem
    {
        public static async Task Gain(Card card, int Point)
        {
            //Command.GameSystem
        }
        public static async Task Gain(List<Card> cards, int Point)
        {

        }
        public static async Task Hurt(Card card, int Point)
        {
            //Command.GameSystem
        }
        public static async Task Hurt(List<Card> cards, int Point)
        {

        }
        public static async Task RangeGain(List<Card> cards, int Point)
        {

        }
    }
    /// <summary>
    /// 转移卡牌位置、所属区域的相关机制
    /// </summary>
    public class TransSystem
    {
        public static async Task DrawCard(Card card)
        {
        }
        public static async Task PlayCard(List<Card> card)
        {
        }
        public static async Task RecycleCard(List<Card> card)
        {
        }
        public static async Task DeployCard(Card card)
        {
            //await MoveTo(Info.AgainstInfo.SelectRegion, Info.AgainstInfo.SelectLocation);
            await Command.CardCommand.DeployCard(card, SelectRegion, SelectLocation);
            await CardEffectStackControl.Trigger_NewAsync<TriggerType.BeforeDeployCard>(cardSet.BroastCardList(card));
            await CardEffectStackControl.Trigger_NewAsync<TriggerType.WhenDeployCard>(card);
            await CardEffectStackControl.Trigger_NewAsync<TriggerType.AfterDeployCard>(cardSet.BroastCardList(card));

        }
        public static async Task BanishCard(Card card)
        {
            Debug.Log("丢弃" + card.name);

            await CardEffectStackControl.Trigger_NewAsync<TriggerType.BeforeBanishCard>(cardSet.BroastCardList(card));
            await CardEffectStackControl.Trigger_NewAsync<TriggerType.WhenBanishCard>(card);
            await CardEffectStackControl.Trigger_NewAsync<TriggerType.AfterBanishCard>(cardSet.BroastCardList(card));

            //await card.TriggerAsync<TriggerType.BeforeBanishCard>();
            //await card.TriggerAsync<TriggerType.WhenBanishCard>();
            //await card.TriggerAsync<TriggerType.AfterBanishCard>();
            //List<Card> cards= Info.AgainstInfo.cardSet[GameEnum.Orientation.All].cardList.Except(new List<Card> { card }).ToList();

        }
        public static async Task DisCard(Card card)
        {
            await CardEffectStackControl.Trigger_NewAsync<TriggerType.BeforeDisCard>(cardSet.BroastCardList(card));
            await CardEffectStackControl.Trigger_NewAsync<TriggerType.WhenDisCard>(card);
            await CardEffectStackControl.Trigger_NewAsync<TriggerType.AfterDisCard>(cardSet.BroastCardList(card));
            //await card.TriggerAsync<TriggerType.BeforeDisCard>();
            //await card.TriggerAsync<TriggerType.WhenDisCard>();
            //await card.TriggerAsync<TriggerType.AfterDisCard>();
        }
    }
    /// <summary>
    /// 选择单位、区域、场景属性的相关机制
    /// </summary>
    public class SelectSystem
    {
        public static async Task SelectUnite(Card card, List<Card> targetCards, int num)
        {
            await Command.StateCommand.WaitForSelecUnit(card, targetCards, num);
        }
        public static async Task SelectLocation(Card card)
        {
            await Command.StateCommand.WaitForSelectLocation(card);
        }
    }
    public class TurnSystem
    {
        public static async Task WhenTurnStart(Card card, List<Card> targetCards, int num)
        {
            await Command.StateCommand.WaitForSelecUnit(card, targetCards, num);
        }
        public static async Task WhenTurnEnd(Card card)
        {
            await CardEffectStackControl.Trigger_NewAsync<TriggerType.AfterDisCard>(cardSet.BroastCardList(card));

        }
    }
}
//触发丢弃卡牌前

//await Command.CardCommand.DisCard(card);

//触发丢弃卡牌后
