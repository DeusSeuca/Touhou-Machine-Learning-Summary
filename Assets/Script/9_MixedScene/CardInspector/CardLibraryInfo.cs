﻿using Command.CardInspector;
using GameEnum;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

using System.Linq;
using UnityEngine;
using static Info.CardInspector.CardLibraryInfo.LevelLibrary.SectarianCardLibrary.RankLibrary;
//你干啥呢？
// 我好像只能看你这一个文件右边没有其他的吗？

namespace Info
{
    namespace CardInspector
    {
        [CreateAssetMenu(fileName = "SaveData", menuName = "CreatCardDataAsset")]
        public partial class CardLibraryInfo : SerializedScriptableObject
        {
            //[LabelText("单人牌库图标")]
            //public Texture2D singleIcon;
            //[LabelText("多人牌库图标")]
            //public Texture2D MultiIcon;

            [HorizontalGroup("Button", 120, LabelWidth = 70)]
            [Button("刷新卡牌数据")]
            public void Refresh() => CardLibraryCommand.Refresh();

            [HorizontalGroup("Button", 120, LabelWidth = 70)]
            [Button("载入卡牌数据从表格")]
            public void Load() => CardLibraryCommand.LoadFromCsv();

            [HorizontalGroup("Button", 120, LabelWidth = 70)]
            [Button("清空卡牌数据")]
            public void Clear() => CardLibraryCommand.ClearCsvData();

            [HorizontalGroup("Button", 120, LabelWidth = 70)]
            [Button("保存卡牌数据到表格")]
            public void Save() => CardLibraryCommand.SaveToCsv();

            [ShowInInspector]
            [LabelText("单人模式牌库卡牌数量")]
            public int singleModeCardCount => singleModeCards.Count;
            [ShowInInspector]
            [LabelText("多人模式牌库卡牌数量")]
            int multiModeCardCount => multiModeCards.Count;
            [LabelText("使用语言")]
            public static string useLanguage => Translate.currentLanguage;
            //[ShowInInspector]
            public List<CardModelInfo> singleModeCards = new List<CardModelInfo>();
            public List<CardModelInfo> multiModeCards = new List<CardModelInfo>();
            [HideInInspector]
            public List<LevelLibrary> levelLibries = new List<LevelLibrary>();
            [ShowInInspector]
            public List<string> includeLevel => singleModeCards.Select(x => x.level).Distinct().ToList();




            [ShowInInspector]
            public Dictionary<Sectarian, Texture2D> sectarianIcons;
            [ShowInInspector]
            public Dictionary<CardRank, Texture2D> rankIcons;

            public class LevelLibrary
            {
                internal bool isSingleMode;
                public string level;
                public List<SectarianCardLibrary> sectarianCardLibraries = new List<SectarianCardLibrary>();
                [TabGroup("卡片管理")]
                public List<CardModelInfo> cardModelInfos;
                public List<Sectarian> includeSectarian => cardModelInfos.Select(x => x.sectarian).Distinct().ToList();

                public LevelLibrary(List<CardModelInfo> singleModeCardsModels, string level)
                {
                    this.level = level;
                    isSingleMode = level != "多人";
                    cardModelInfos = singleModeCardsModels.Where(cards => cards.level == level).ToList();
                }

                public class SectarianCardLibrary
                {
                    [HideLabel, PreviewField(55, ObjectFieldAlignment.Right)]
                    [HorizontalGroup("Split", 55, LabelWidth = 70)]
                    public Texture2D icon;

                    [VerticalGroup("Split/Meta")]
                    [LabelText("当前卡牌数量")]
                    public int cardNum;
                    //public bool isSingleMode;
                    [TabGroup("卡片制作")]
                    [HideLabel, PreviewField(128, ObjectFieldAlignment.Right)]
                    public Texture2D cardIcon;

                    [TabGroup("卡片制作")]
                    [LabelText("所属势力")]
                    public Sectarian sectarian;

                    [TabGroup("卡片制作")]
                    [LabelText("卡片名称")]
                    public string cardName;

                    [TabGroup("卡片制作")]
                    public int point;

                    [TabGroup("卡片制作")]
                    [Button("添加卡牌")]
                    public void AddCardModel()
                    {
                        //CardModelInfo newCardModelInfo = new CardModelInfo(
                        //    1,
                        //    cardName,
                        //    "无效",
                        //    "默认",
                        //    sectarian,
                        //    CardLevel.Copper,
                        //    point,
                        //    00,
                        //    icon
                        //    );
                        //Command.CardLibraryCommand.GetLibrarySaveData().cards.Add(newCardModelInfo);
                        //添加条件，暂时为名字不为空
                        //if (CardName != "")
                        //{
                        //    if (CardModelInfos == null)
                        //    {
                        //        CardModelInfos = new List<CardModelInfo>();
                        //    }
                        //    int NewCardId = int.Parse($"{10}{(int)sectarian}{CardModelInfos.Count}");
                        //    Command.CardLibraryCommand.CreatScript(NewCardId);
                        //    CardModelInfos.Add(new CardModelInfo(icon, NewCardId, CardName, Point, sectarian));
                        //    CardNum = CardModelInfos.Count;
                        //}
                    }
                    [TabGroup("卡片管理")]
                    public List<CardModelInfo> cardModelInfos;
                    public List<RankLibrary> rankLibraries = new List<RankLibrary>();
                    public List<CardRank> includeRank => cardModelInfos.Select(x => x.Rank).Distinct().ToList();


                    public SectarianCardLibrary(List<CardModelInfo> CardsModels, Sectarian sectarian)
                    {
                        this.sectarian = sectarian;
                        icon = CardLibraryCommand.GetLibraryInfo().sectarianIcons[sectarian];
                        cardModelInfos = CardsModels.Where(card => card.sectarian == sectarian).ToList();
                    }
                    public class RankLibrary
                    {
                        [HideLabel, PreviewField(55, ObjectFieldAlignment.Right)]
                        [HorizontalGroup("Split", 55, LabelWidth = 70)]
                        public Texture2D icon;
                        public CardRank rank;
                        [TabGroup("卡片管理")]
                        public List<CardModelInfo> cardModelInfos;
                        public RankLibrary(List<CardModelInfo> cardsModels, CardRank rank)
                        {
                            this.rank = rank;
                            icon = CardLibraryCommand.GetLibraryInfo().rankIcons[rank];
                            cardModelInfos = cardsModels.Where(cards => cards.Rank == rank).ToList();
                        }
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
                            [LabelText("所属关卡")]
                            public string level;
                            [VerticalGroup("Split/Meta")]
                            [LabelText("名字")]
                            public string cardName;
                            [VerticalGroup("Split/Meta")]
                            [LabelText("点数")]
                            public int point;

                            [VerticalGroup("Split/Meta")]
                            [LabelText("卡片等级"), EnumToggleButtons]
                            public CardRank Rank;
                            [VerticalGroup("Split/Meta")]
                            [LabelText("所属势力"), EnumToggleButtons]
                            public Sectarian sectarian;
                            [VerticalGroup("Split/Meta")]
                            [LabelText("部署区域"), EnumToggleButtons]
                            public Region cardProperty = Region.All;
                            [VerticalGroup("Split/Meta")]
                            [LabelText("部署所属"), EnumToggleButtons]
                            public Territory cardTerritory = Territory.My;
                            [LabelText("卡片标签"), EnumToggleButtons]
                            public string tag = "";
                            [LabelText("卡片介绍")]
                            public string introduction = "";
                            [LabelText("效果描述")]
                            public string describe = "";
                            public CardModelInfo(int cardId, string level, string cardName, string describe, string tag, Sectarian sectarian, CardRank rank, Region cardProperty, Territory cardTerritory, int point, int ramification, Texture2D icon)
                            {
                                this.icon = icon;
                                this.cardId = cardId;
                                this.level = level;
                                this.cardName = cardName;
                                this.describe = describe;
                                this.tag = tag;
                                this.point = point;
                                this.sectarian = sectarian;
                                this.cardProperty = cardProperty;
                                this.cardTerritory = cardTerritory;
                                this.Rank = rank;
                            }
                            [Button("打开脚本")]
                            public void OpenCardScript()
                            {
                                string targetPath = Application.dataPath + $@"\Script\9_MixedScene\CardSpace\Card{cardId}.cs";
                                CardLibraryCommand.CreatScript(cardId);
                                System.Diagnostics.Process.Start(targetPath);
                            }
                        }
                    }
                }
            }


        }
    }
}