using CardModel;
using Command;
using GameEnum;
using Info;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class test : MonoBehaviour
{
    [ShowInInspector]
    public CardSet cardSet => AgainstInfo.cardSet;
    [ShowInInspector]
    public CardSet FiltercardSet;

    public string text;
    [Button]
    public void test1(GameEnum.Language language)
    {
        Translate.currentLanguage = language.ToString();
    }
    [Button("翻译标签")]
    public void ShowText(GameEnum.CardTag tag)
    {
        text = tag.TransTag();
    }
    [Button("查找集合")]

    public void filterCardSet(List<GameEnum.CardTag> tags)
    {
        FiltercardSet  = cardSet[tags.ToArray()];
    }
    
}
