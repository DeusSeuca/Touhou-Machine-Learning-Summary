using CardModel;
using CardSpace;
using System.Threading.Tasks;
using UnityEngine;

namespace GameSystem
{
    /// <summary>
    /// 改变卡牌点数的相关机制
    /// </summary>
    public class PointSystem
    {
        public static void Hurt(Card card, int Point)
        {
            //Command.GameSystem
        }
        public static void Gain(Card card, int Point)
        {
            //Command.GameSystem
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
        public static async Task BanishCard(Card card)
        {
            card.Trigger<TriggerType.BeforeBanishCard>();
            card.Trigger<TriggerType.WhenBanishCard>();
            card.Trigger<TriggerType.AfterBanishCard>();
        }
        public static async Task DisCard(Card card)
        {
            card.Trigger<TriggerType.BeforeDisCard>();
            card.Trigger<TriggerType.WhenDisCard>();
            card.Trigger<TriggerType.AfterDisCard>();
        }
    }
    /// <summary>
    /// 选择单位、区域、场景属性的相关机制
    /// </summary>
    public class SelectSystem
    {
        public static async Task SelectUnite(Card card)
        {
            card.Trigger<TriggerType.BeforeBanishCard>();
            card.Trigger<TriggerType.WhenBanishCard>();
            card.Trigger<TriggerType.AfterBanishCard>();
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
