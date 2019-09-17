#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

public class CardMenu : OdinMenuEditorWindow
{
    [MenuItem("Tools/卡组编辑器")]
    private static void OpenWindow()
    {
        CardMenu window = GetWindow<CardMenu>();
        window.position = GUIHelper.GetEditorWindowRect().AlignCenter(700, 700);
    }
    //会导致画面闪烁
    private void OnInspectorUpdate() => ForceMenuTreeRebuild();
    protected override OdinMenuTree BuildMenuTree()
    {

        CardLibrarySaveData SaveData = Resources.Load<CardLibrarySaveData>("SaveData");
        SaveData.Init();
        var tree = new OdinMenuTree(true);
        tree.DefaultMenuStyle.Height = 60;
        tree.DefaultMenuStyle.IconSize = 48.00f;
        tree.Config.DrawSearchToolbar = true;
        tree.Add("卡牌列表", SaveData);
        if (SaveData.SingleCardLibrarieDatas != null)
        {
            foreach (var SingleLibrary in SaveData.SingleCardLibrarieDatas)
            {
                tree.Add($@"卡牌列表/{SingleLibrary.sectarian}", SingleLibrary);
                if (SingleLibrary.CardModelInfos != null)
                {
                    foreach (var CardModel in SingleLibrary.CardModelInfos)
                    {
                        tree.Add($@"卡牌列表/{SingleLibrary.sectarian}/{CardModel.CardName}", CardModel);
                    }
                }
            }
        }
        tree.EnumerateTree().AddIcons<CardLibrarySaveData>(x => x.Icon);
        tree.EnumerateTree().AddIcons<SingleCardLibrary>(x => x.Icon);
        tree.EnumerateTree().AddIcons<CardModelInfo>(x => x.Icon);
        return tree;
    }
    //protected override OdinMenuTree BuildMenuTree()
    //{

    //    CardLibrarySaveData SaveData = Resources.Load<CardLibrarySaveData>("SaveData");
    //    SaveData.Init();
    //    var tree = new OdinMenuTree(true);
    //    tree.DefaultMenuStyle.Height = 60;
    //    tree.DefaultMenuStyle.IconSize = 48.00f;
    //    tree.Config.DrawSearchToolbar = true;
    //    tree.Add("卡牌列表", SaveData);
    //    if (SaveData.SingleCardLibrarieDatas != null)
    //    {
    //        foreach (var SingleLibrary in SaveData.SingleCardLibrarieDatas)
    //        {
    //            tree.Add($@"卡牌列表/{SingleLibrary.sectarian}", SingleLibrary);
    //            if (SingleLibrary.CardModelInfos != null)
    //            {
    //                foreach (var CardModel in SingleLibrary.CardModelInfos)
    //                {
    //                    tree.Add($@"卡牌列表/{SingleLibrary.sectarian}/{CardModel.CardName}", CardModel);
    //                }
    //            }
    //        }
    //    }
    //    tree.EnumerateTree().AddIcons<CardLibrarySaveData>(x => x.Icon);
    //    tree.EnumerateTree().AddIcons<SingleCardLibrary>(x => x.Icon);
    //    tree.EnumerateTree().AddIcons<CardModelInfo>(x => x.Icon);
    //    return tree;
    //}
}
#endif