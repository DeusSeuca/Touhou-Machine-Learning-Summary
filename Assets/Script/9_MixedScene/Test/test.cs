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

    [Button]
    public void test1(GameEnum.Language language)
    {
        Translate.currentLanguage = language.ToString();
    }
}
