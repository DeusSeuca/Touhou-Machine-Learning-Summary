using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public partial class SingleCardLibrary
{
    [HideLabel, PreviewField(55, ObjectFieldAlignment.Right)]
    [HorizontalGroup("Split", 55, LabelWidth = 70)]
    public Texture2D Icon;

    [VerticalGroup("Split/Meta")]
    [LabelText("当前卡牌数量")]
    public int CardNum;

    [TabGroup("卡片制作")]
    [HideLabel, PreviewField(128, ObjectFieldAlignment.Right)]
    public Texture2D icon;

    [TabGroup("卡片制作")]
    [LabelText("所属势力")]
    public Sectarian sectarian;

    [TabGroup("卡片制作")]
    [LabelText("卡片名称")]
    public string CardName;

    [TabGroup("卡片制作")]
    public int Point;

    [TabGroup("卡片制作")]
    [Button("添加卡牌")]
    public void AddCardModely()
    {
        //卡牌添加条件，暂时为名字不为空
        if (CardName != "")
        {
            if (CardModelInfos == null)
            {
                CardModelInfos = new List<CardModelInfo>();
            }
            int NewCardId = int.Parse($"{10}{(int)sectarian}{CardModelInfos.Count}");
            Command.CardLibraryCommand.CreatScript(NewCardId);
            CardModelInfos.Add(new CardModelInfo(icon, NewCardId, CardName, Point, sectarian));
            CardNum = CardModelInfos.Count;
        }
    }
    [TabGroup("卡片管理")]
    public List<CardModelInfo> CardModelInfos;
}
