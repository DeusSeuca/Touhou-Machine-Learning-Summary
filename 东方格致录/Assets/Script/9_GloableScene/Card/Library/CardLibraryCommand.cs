using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardLibraryCommand : SerializedMonoBehaviour
{

    public static CardLibraryCommand Instance;
    public GameObject Card_Model;
    CardLibrarySaveData SaveData;
    public List<CardLibrary> CardLibraryList => SaveData.cardLibrarieList;
    private void Awake()
    {
        Instance = this;
        SaveData = Resources.Load<CardLibrarySaveData>("SaveData");
    }

    private void AddComponent(string ClassName)
    {
        gameObject.AddComponent(Type.GetType(ClassName));
    }
    public static CardModelInfo GetCardStandardInfo(int id)
    {
        return Instance.CardLibraryList[0].CardModelInfos.First(info => info.cardId == id);
    }
}
