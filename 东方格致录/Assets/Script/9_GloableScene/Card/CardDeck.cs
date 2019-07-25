﻿using System;
using System.Collections.Generic;

[Serializable]
public class CardDeck
{
    public string DeckName;
    public int LeaderId;
    public List<int> CardIds;
    public CardDeck(string DeckName, int LeaderId, List<int> CardIds)
    {
        this.DeckName = DeckName;
        this.LeaderId = LeaderId;
        this.CardIds = CardIds;
    }
}