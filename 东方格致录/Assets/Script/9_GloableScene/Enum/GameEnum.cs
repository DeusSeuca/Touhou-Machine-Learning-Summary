using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnum
{
    public enum CardBoardMode
    {
        Select,//多次选择模式
        ChangeCard,//单次抽卡模式
        ShowOnly//无法操作模式
    }
    public enum LoadRangeOnBattle
    {
        None = 0x0,
        My_Water = 0x0000_0001,
        My_Fire = 0x0000_0010,
        My_Wind = 0x0000_0100,
        My_Soil = 0x0000_1000,
        My_All = 0x0000_1111,
        Op_Water = 0x0001_0000,
        Op_Fire = 0x0010_0000,
        Op_Wind = 0x0100_0000,
        Op_Soil = 0x1000_0000,
        Op_All = 0x1111_0000,
        All = 0x1111_1111

    }
    public enum LoadRangeOnOther
    {
        None = 0x0,
        My_Leader = 0x0000_0001,
        My_Hand = 0x0000_0010,
        My_Deck = 0x0000_0100,
        My_Grave = 0x0000_1000,

        Op_Leader = 0x0001_0000,
        Op_Hand = 0x0010_0000,
        Op_Deck = 0x0100_0000,
        Op_Grave = 0x1000_0000,

    }
    public enum NetAcyncType
    {
        FocusCard,
        PlayCard,
    }
}
