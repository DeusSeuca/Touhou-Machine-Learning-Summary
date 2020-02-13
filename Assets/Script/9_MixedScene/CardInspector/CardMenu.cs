#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using Info.CardInspector;
using static Info.CardInspector.CardLibraryInfo;
using static Info.CardInspector.CardLibraryInfo.SectarianCardLibrary;
using static Info.CardInspector.CardLibraryInfo.SectarianCardLibrary.RankLibrary;
using System.Linq;

namespace CardInspector
{
    public class CardMenu : OdinMenuEditorWindow
    {
        static CardMenu instance;
        [MenuItem("Tools/卡组编辑器")]
        private static void OpenWindow()
        {
            CardMenu window = GetWindow<CardMenu>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(700, 700);
        }
        //会导致画面闪烁
        //private void OnInspectorUpdate() => ForceMenuTreeRebuild();
        public static void UpdateInspector()
        {
            instance.ForceMenuTreeRebuild();
            Debug.LogError("更新");
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            CardLibraryInfo cardLibraryInfo = Command.CardInspector.CardLibraryCommand.GetLibraryInfo();
            var tree = new OdinMenuTree(true);
            tree.DefaultMenuStyle.Height = 60;
            tree.DefaultMenuStyle.IconSize = 48.00f;
            tree.Config.DrawSearchToolbar = true;
            Command.CardInspector.CardLibraryCommand.Init();
            
            tree.Add("单人模式牌库", cardLibraryInfo);
            //遍历单人模式牌库每个阵营
            foreach (var library in cardLibraryInfo.cardLibrarieList.Where(library => library.isSingleMode))
            {
                //遍历每个单人关卡
                foreach (var level in cardLibraryInfo.includeLevel)
                {
                    if (library.CardModelInfos.Any(x=>x.level==level))
                    {
                        tree.Add($"单人模式牌库/{level}/{library.sectarian}", library);
                        //遍历单个阵营中每个
                        foreach (var sIngleSectarianLibrary in library.sIngleSectarianLibraries)
                        {
                            var s = sIngleSectarianLibrary[level];
                            tree.Add($"单人模式牌库/{level}/{library.sectarian}/{sIngleSectarianLibrary.rank}", sIngleSectarianLibrary);
                            foreach (var cardModel in sIngleSectarianLibrary.CardModelInfos.Where(card => card.level == level))
                            {
                                //Debug.Log(cardModel.level+"?");
                                tree.Add($"单人模式牌库/{level}/{library.sectarian}/{cardModel.Rank}/{cardModel.cardName}", cardModel);
                                Command.CardInspector.CardLibraryCommand.CreatScript(cardModel.cardId);
                            }
                        }
                    }
                    
                }
                
            }

            tree.Add("多人模式牌库", cardLibraryInfo);
            foreach (var library in cardLibraryInfo.cardLibrarieList.Where(library => !library.isSingleMode))
            {
                tree.Add("多人模式牌库/" + library.sectarian, library);
                foreach (var sIngleSectarianLibrary in library.sIngleSectarianLibraries)
                {
                    tree.Add($"多人模式牌库/{library.sectarian}/{sIngleSectarianLibrary.rank}", sIngleSectarianLibrary);
                    foreach (var cardModel in sIngleSectarianLibrary.CardModelInfos)
                    {
                        tree.Add($"多人模式牌库/{library.sectarian}/{cardModel.Rank}/{cardModel.cardName}", cardModel);
                        Command.CardInspector.CardLibraryCommand.CreatScript(cardModel.cardId);
                    }
                }
            }

            tree.EnumerateTree().AddIcons<CardLibraryInfo>(x => x.icon);
            tree.EnumerateTree().AddIcons<SectarianCardLibrary>(x => x.icon);
            tree.EnumerateTree().AddIcons<RankLibrary>(x => x.icon);
            tree.EnumerateTree().AddIcons<CardModelInfo>(x => x.icon);
            instance = this;
            return tree;
        }
    }
}
#endif