using System;

namespace TriggerType
{
    public class PlayCard : Attribute { }

    public class Discard : Attribute { }

    public class BeforeDisCard : Attribute { }
    public class WhenDisCard : Attribute { }
    public class AfterDisCard : Attribute { }

    public class BeforeCardBanish : Attribute { }
    public class WhenCardBanish : Attribute { }
    public class AfterCardBanish : Attribute { }

    public class BeforeCardDeploy : Attribute { }
    public class WhenCardDeploy : Attribute { }
    public class AfterCardDeploy : Attribute { }

    public class BeforeCardDead : Attribute { }
    public class WhenCardDead : Attribute { }
    public class AfterCardDead : Attribute { }

    public class Hurt : Attribute { }
    public class Dead : Attribute { }
    public class WhenTurnStart : Attribute { }
    public class WhenTurnEnd : Attribute { }
    public class WhenCountDown : Attribute { }

    public class BeforeCardGain : Attribute { }
    public class WhenCardGain : Attribute { }
    public class AfterCardGain : Attribute { }
    
}
