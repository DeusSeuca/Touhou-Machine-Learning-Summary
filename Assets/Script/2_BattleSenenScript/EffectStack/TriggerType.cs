using System;

namespace TriggerType
{
    public class PlayCard : Attribute { }
    public class Use : Attribute { }
    public class Discard : Attribute { }
    public class BeforeDisCard : Attribute { }
    public class WhenDisCard : Attribute { }
    public class AfterDisCard : Attribute { }
    public class BeforeBanishCard : Attribute { }
    public class WhenBanishCard : Attribute { }
    public class AfterBanishCard : Attribute { }
    public class Deploy : Attribute { }
    public class Hurt : Attribute { }
    public class WhenOtherHurt : Attribute { }
    public class Dead : Attribute { }
    public class TurnStart : Attribute { }
    public class CountDown : Attribute { }
}
