using CardModel;
using CardSpace;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace GameSystem
{
    /// <summary>
    /// 改变卡牌点数的相关机制
    /// </summary>
    public class PointSystem
    {
        public static async Task Hurt(Card card, int Point)
        {
            //Command.GameSystem
        }
        public static async Task Hurt(List<Card> cards, int Point)
        {
        }
        public static async Task RangeHurt(List<Card> cards, int Point)
        {
        }
        public static async Task Gain(Card card, int Point)
        {
            //Command.GameSystem
        }
        public static async Task Gain(List<Card> cards, int Point)
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
        }
        public static async Task BanishCard(Card card)
        {
            await card.TriggerAsync<TriggerType.BeforeBanishCard>();
            await card.TriggerAsync<TriggerType.WhenBanishCard>();
            await card.TriggerAsync<TriggerType.AfterBanishCard>();
        }
        public static async Task DisCard(Card card)
        {
            await card.TriggerAsync<TriggerType.BeforeDisCard>();
            await card.TriggerAsync<TriggerType.WhenDisCard>();
            await card.TriggerAsync<TriggerType.AfterDisCard>();
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
}
//触发丢弃卡牌前

//await Command.CardCommand.DisCard(card);

//触发丢弃卡牌后
