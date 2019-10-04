using CardSpace;
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
            public static void DisCard(Card card)
            {
                //触发丢弃卡牌前
                Command.CardCommand.DisCard(card);
                card.Trigger<TriggerType.WhenDiscard>();
                //触发丢弃卡牌后
            }
        }
    }
}

