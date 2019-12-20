﻿using Model;
using System;
using System.Collections.Generic;
namespace Network
{
    [Serializable]
    public class NetInfoModel
    {
        public class Location
        {
            public int x;
            public int y;
            public Location(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }
        public class PlayerInfo
        {
            public string _id;
            public string Name;
            public string Password;
            public int Level;
            public int Rank;
            public Dictionary<string, int> Resource { get; set; }
            public int UseDeckNum;
            public List<CardDeck> Deck;
            public CardDeck UseDeck => Deck[UseDeckNum];

            public PlayerInfo(string Name, string Password, List<CardDeck> Deck)
            {
                this.Name = Name;
                this.Deck = Deck;
                this.Password = Password;
                Level = 0;
                Rank = 0;
                UseDeckNum = 0;
                Resource = new Dictionary<string, int>();
                Resource.Add("faith", 0);
                Resource.Add("recharge", 0);
            }

        }
        [Serializable]
        public class GeneralCommand
        {
            public object[] Datas;
            public GeneralCommand()
            {
            }
            public GeneralCommand(params object[] Datas)
            {
                this.Datas = Datas;
            }
        }
        [Serializable]
        public class GeneralCommand<T>
        {
            public T[] Datas;
            public GeneralCommand()
            {
            }
            public GeneralCommand(params T[] Datas)
            {
                this.Datas = Datas;
            }
        }
    }
}
