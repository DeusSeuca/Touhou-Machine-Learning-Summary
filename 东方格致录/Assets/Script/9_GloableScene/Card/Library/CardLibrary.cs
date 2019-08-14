using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static CardLibrarySaveData;
using static CardLibrarySaveData.SingleCardLibrary;

public class CardLibrary : SerializedMonoBehaviour
{

    public static CardLibrary Instance;
    public GameObject Card_Model;
    CardLibrarySaveData SaveData;
    public List<SingleCardLibrary> CardLibraryList => SaveData.SingleCardLibrarieDatas;
    private void Awake()
    {
        Instance = this;
        SaveData = Resources.Load<CardLibrarySaveData>("SaveData");

        //SaveData.SingleCardLibrarieDatas.

    }
    private void Start()
    {
    }

    private void AddComponent(string ClassName)
    {
        gameObject.AddComponent(Type.GetType(ClassName));
    }
    public static CardModelInfo GetCardStandardInfo(int id)
    {
        Debug.LogWarning(Instance.CardLibraryList.Count);
        Debug.LogWarning(Instance.CardLibraryList[0].CardNum);
        Debug.LogWarning(Instance.CardLibraryList[1].CardNum);
       
        return Instance.CardLibraryList[1].CardModelInfos.First(info => info.CardId == id);
    }
}
