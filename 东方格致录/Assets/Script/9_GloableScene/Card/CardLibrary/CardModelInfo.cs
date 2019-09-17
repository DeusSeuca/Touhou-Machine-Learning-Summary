using CardSpace;
using Sirenix.OdinInspector;
using System;
using System.Diagnostics;
using UnityEngine;


[Serializable]
public class CardModelInfo
{
    [HorizontalGroup("Split", 55, LabelWidth = 70)]
    [HideLabel, PreviewField(55, ObjectFieldAlignment.Right)]
    //[LabelText("卡片贴图")]
    public Texture2D Icon;
    [VerticalGroup("Split/Meta")]
    [LabelText("ID")]
    public int CardId;
    [VerticalGroup("Split/Meta")]
    [LabelText("名字")]
    public string CardName;
    [VerticalGroup("Split/Meta")]
    [LabelText("点数")]
    public int Point;
    [VerticalGroup("Split/Meta")]
    [LabelText("所属势力")]
    public Sectarian sectarian;
    [VerticalGroup("Split/Meta")]
    [LabelText("部署区域")]
    public Property CardProperty = Property.All;
    [VerticalGroup("Split/Meta")]
    [LabelText("部署所属")]
    public Territory CardTerritory = Territory.My;
    [LabelText("卡片介绍")]
    public string[] Introduction = new string[] { "" };
    public CardModelInfo(Texture2D icon, int cardId, string cardName, int point, Sectarian sectarian)
    {
        Icon = icon;
        CardId = cardId;
        CardName = cardName;
        Point = point;
        this.sectarian = sectarian;
    }
    public CardModelInfo( int cardId, string cardName, string describe, string tag, Sectarian sectarian, CardLevel level, int point,int ramification, Texture2D icon)
    {
        Icon = icon;
        CardId = cardId;
        CardName = cardName;
        Point = point;
        this.sectarian = sectarian;
    }
    [Button("打开脚本")]
    public void OpenCardScript() => Process.Start(Application.dataPath + $@"\Script\9_GloableScene\Card\CardLibrary\CardModel\Card{CardId}.cs");
}

