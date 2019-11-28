using GameEnum;
using Sirenix.OdinInspector;
using System;
using System.Diagnostics;
using UnityEngine;

namespace CardInspector
{
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
        [LabelText("卡片等级"), EnumToggleButtons, GUIColor(0, 1, 0)]
        public CardLevel level;
        [VerticalGroup("Split/Meta")]
        [LabelText("所属势力"), EnumToggleButtons]
        public Sectarian sectarian;
        [VerticalGroup("Split/Meta")]
        [LabelText("部署区域"),EnumToggleButtons]
        public Region cardProperty = Region.All;
        [VerticalGroup("Split/Meta")]
        [LabelText("部署所属"), EnumToggleButtons]
        public Territory cardTerritory = Territory.My;
        [LabelText("卡片标签"), EnumToggleButtons]
        public string tag = "";
        [LabelText("卡片介绍")]
        public string describe = "";
        [Flags]
        public enum te
        {
            //无 = 0,
            我方 = 1 << 1,
            敌方 = 1 << 2,
            任意 = 我方 | 敌方,
        };
        [EnumToggleButtons,HideLabel]
        public te range;
        [ShowInInspector]
        public te 部署范围 => range;

        [Flags]
        public enum region
        {
            //无 = 0,
            近战 = 1 << 1,
            远程 = 1 << 2,
            攻城 = 1 << 3,
            任意 = 近战 | 远程 | 攻城,
        };
        [EnumToggleButtons, HideLabel]
        public region test;
        [ShowInInspector]
        public region 部署区域 => test;

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
            string targetPath = Application.dataPath + $@"\Script\9_MixedScene\CardSpace\Card{cardId}.cs";
            Command.CardInspector.CardLibraryCommand.CreatScript(cardId);
            Process.Start(targetPath);
        }
    }
}


