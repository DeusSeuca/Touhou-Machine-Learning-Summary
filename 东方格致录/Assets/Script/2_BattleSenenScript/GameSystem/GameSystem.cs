using CardSpace;
using System.Threading.Tasks;
using UnityEngine;
namespace Command
{
    namespace GameSystem
    {
        public class PointSystem : MonoBehaviour
        {
            public static void Hurt(Card card, int Point)
            {
                //Command.GameSystem
            }
            public static async Task DisCard(Card card)
            {
                //触发丢弃卡牌前
                await Command.CardCommand.DisCard(card);
                card.Trigger<TriggerType.WhenDiscard>();
                //触发丢弃卡牌后
            }
        }
    }
}

