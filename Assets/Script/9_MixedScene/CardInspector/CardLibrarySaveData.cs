using Command.CardInspector;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace CardInspector
{
    [CreateAssetMenu(fileName = "SaveData", menuName = "CreatCardDataAsset")]
    public partial class CardLibrarySaveData : ScriptableObject
    {
        [LabelText("牌库图标")]
        public Texture2D Icon;
        [ShowInInspector]
        [LabelText("牌库卡牌数量")]
        public int LibraryCardCount;
        //[ShowInInspector]
        public List<CardModelInfo> cards = new List<CardModelInfo>();
        public List<CardLibrary> cardLibrarieList = new List<CardLibrary>();
        [HorizontalGroup("Button", 155, LabelWidth = 70)]
        [Button("载入卡牌数据从csv表格")]
        public void Load() => CardLibraryCommand.LoadFromCsv();
        [HorizontalGroup("Button", 155, LabelWidth = 70)]
        [Button("保存卡牌数据到csv表格")]
        public void Save() => CardLibraryCommand.SaveToCsv();
    }

}
