namespace GameEnum
{
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
}