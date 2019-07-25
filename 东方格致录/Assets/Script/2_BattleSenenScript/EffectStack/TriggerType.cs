using System;


public class TriggerType
{
    public class PlayCard : Attribute { }
    public class Use : Attribute { }
    public class Discard : Attribute { }
    public class Deploy : Attribute { }
    public class Dead : Attribute { }
    public class TurnStart : Attribute { }
    public class CountDown : Attribute { }
}

