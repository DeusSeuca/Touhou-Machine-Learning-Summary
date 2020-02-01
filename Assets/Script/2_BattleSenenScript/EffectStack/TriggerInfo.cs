using CardModel;
using System.Collections.Generic;

public class TriggerInfo
{
    public TriggerTime triggerTime;
    public TriggerType triggerType;
    public Card triggerCard;
    public List<Card> targetCards;
    public Card targetCard => targetCards[0];
    public int point;
    public TriggerInfo this[TriggerTime triggerTime] => Clone(triggerTime: triggerTime);
    public TriggerInfo this[TriggerType triggerType] => Clone(triggerType: triggerType);
    public TriggerInfo this[List<Card> targetCards] => Clone(targetCards: targetCards);
    public TriggerInfo this[Card targetCard] => Clone(targetCards:new List<Card> { targetCard });

    private TriggerInfo Clone(TriggerTime? triggerTime = null, TriggerType? triggerType = null, List<Card> targetCards = null)
    {
        TriggerInfo triggerInfo = new TriggerInfo();
        triggerInfo.triggerTime = triggerTime ?? this.triggerTime;
        triggerInfo.triggerType = triggerType ?? this.triggerType;
        triggerInfo.triggerCard = triggerCard;
        triggerInfo.targetCards = targetCards ?? this.targetCards;
        triggerInfo.point = point;
        return triggerInfo;
    }
    public static TriggerInfo Build(Card triggerCard, List<Card> targetCards, int point = 0)
    {
        TriggerInfo triggerInfo = new TriggerInfo();
        triggerInfo.triggerCard = triggerCard;
        triggerInfo.targetCards = targetCards;
        triggerInfo.point = point;
        return triggerInfo;
    }
    public static TriggerInfo Build(Card triggerCard, Card targetCard, int point = 0)
    {
        TriggerInfo triggerInfo = new TriggerInfo();
        triggerInfo.triggerCard = triggerCard;
        triggerInfo.targetCards = new List<Card>() { targetCard };
        triggerInfo.point = point;
        return triggerInfo;
    }
}
