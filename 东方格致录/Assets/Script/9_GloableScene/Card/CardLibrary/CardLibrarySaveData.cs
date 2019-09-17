using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[CreateAssetMenu(fileName = "SaveData", menuName = "CreatCardDataAsset")]
public partial class CardLibrarySaveData : ScriptableObject
{
    
    [LabelText("牌库图标")]
    public Texture2D Icon;
    [ShowInInspector]
    [LabelText("牌库卡牌数量")]
    public int LibraryCardCount;
    [ShowInInspector]
    public static  List<CardModelInfo> cards;
    [System.Obsolete("已废弃")]
    public List<SingleCardLibrary> SingleCardLibrarieDatas;
    [System.Obsolete("已废弃")]
    public void Init() => LibraryCardCount = SingleCardLibrarieDatas.Select(Cards => Cards.CardNum).Sum();
    [HorizontalGroup("Button", 155, LabelWidth = 70)]
    [Button("载入卡牌数据从csv表格")]
    public void Load() => Command.CardLibraryCommand.LoadFromCsv();
    [HorizontalGroup("Button", 155, LabelWidth = 70)]
    [Button("保存卡牌数据到csv表格")]
    public void Save() => Command.CardLibraryCommand.SaveToCsv();
    [HorizontalGroup("Button", 155, LabelWidth = 70)]
    [Button("添加势力")]
    public void AddSingleCardLibrary()
    {
        if (SingleCardLibrarieDatas == null)
        {
            SingleCardLibrarieDatas = new List<SingleCardLibrary>();
        }
        SingleCardLibrarieDatas.Add(new SingleCardLibrary());
    }
}
