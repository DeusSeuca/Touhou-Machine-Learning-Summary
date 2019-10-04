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
   // private void OnInspectorUpdate() => ForceMenuTreeRebuild();
    protected override OdinMenuTree BuildMenuTree()
    {

        CardLibrarySaveData SaveData = Command.CardLibraryCommand.GetLibrarySaveData();
        var tree = new OdinMenuTree(true);
        tree.DefaultMenuStyle.Height = 60;
        tree.DefaultMenuStyle.IconSize = 48.00f;
        tree.Config.DrawSearchToolbar = true;
        //Command.CardLibraryCommand.LoadFromCsv();
        Debug.Log(SaveData);
        SaveData.cardLibrarieList.Clear();
        SaveData.cardLibrarieList.Add(new CardLibrary(GameEnum.Sectarian.Neutral));
        SaveData.cardLibrarieList.Add(new CardLibrary(GameEnum.Sectarian.Buddhism));
        SaveData.cardLibrarieList.Add(new CardLibrary(GameEnum.Sectarian.Shintoism));
        SaveData.cardLibrarieList.Add(new CardLibrary(GameEnum.Sectarian.science));
        SaveData.cardLibrarieList.Add(new CardLibrary(GameEnum.Sectarian.Taoism));
        tree.Add("基础牌库", SaveData);
        foreach (var library in SaveData.cardLibrarieList)
        {
            tree.Add("基础牌库/" + library.sectarian, library);
            foreach (var cardModel in library.CardModelInfos)
            {
                tree.Add($"基础牌库/{library.sectarian}/{cardModel.level}/{cardModel.cardName}", cardModel);
                Command.CardLibraryCommand.CreatScript(cardModel.cardId);
            }
        }
        tree.EnumerateTree().AddIcons<CardLibrarySaveData>(x => x.Icon);
        tree.EnumerateTree().AddIcons<CardLibrary>(x => x.sectarianIcon);
        tree.EnumerateTree().AddIcons<CardModelInfo>(x => x.icon);
        return tree;
    }
}
#endif