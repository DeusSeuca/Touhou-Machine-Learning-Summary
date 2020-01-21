using System;

namespace GameAttribute
{
    public class CardProperty : Attribute
    {
        public GameEnum.CardField cardProperty;
        public CardProperty(GameEnum.CardField cardProperty)
        {
            this.cardProperty = cardProperty;
        }
    }
}