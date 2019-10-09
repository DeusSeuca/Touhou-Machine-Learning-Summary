using GameEnum;
using Sirenix.OdinInspector;
using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;


[Serializable]
public class CardModelInfo
{
    [HorizontalGroup("Split", 55, LabelWidth = 70)]
    [HideLabel, PreviewField(55, ObjectFieldAlignment.Right)]
    //[LabelText("卡片贴图")]
    public Texture2D icon;
    [VerticalGroup("Split/Meta")]
    [LabelText("ID")]
    public int cardId;
    [VerticalGroup("Split/Meta")]
    [LabelText("名字")]
    public string cardName;
    [VerticalGroup("Split/Meta")]
    [LabelText("点数")]
    public int point;
    [VerticalGroup("Split/Meta")]
    [LabelText("所属势力")]
    public Sectarian sectarian;
    [VerticalGroup("Split/Meta")]
    [LabelText("卡片等级")]
    public CardLevel level;
    [VerticalGroup("Split/Meta")]
    [LabelText("部署区域")]
    public Region cardProperty = Region.All;
    [VerticalGroup("Split/Meta")]
    [LabelText("部署所属")]
    public Territory cardTerritory = Territory.My;
    [LabelText("卡片标签")]
    public string tag = "";
    [LabelText("卡片介绍")]
    public string describe = "";
    public CardModelInfo(int cardId, string cardName, string describe, string tag, Sectarian sectarian, CardLevel level, Region cardProperty, Territory cardTerritory, int point, int ramification, Texture2D icon)
    {
        this.icon = icon;
        this.cardId = cardId;
        this.cardName = cardName;
        this.describe = describe;
        this.tag = tag;
        this.point = point;
        this.sectarian = sectarian;
        this.cardProperty = cardProperty;
        this.cardTerritory = cardTerritory;
        this.level = level;
    }
    [Button("打开脚本")]
    public void OpenCardScript()
    {
        string targetPath = Application.dataPath + $@"\Script\9_GloableScene\CardSpace\Card{cardId}.cs";
        Command.CardLibraryCommand.CreatScript(cardId);
        Process.Start(targetPath);
    }


}

