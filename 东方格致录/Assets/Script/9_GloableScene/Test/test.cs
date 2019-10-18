using CardModel;
using Command;
using GameEnum;
using Info;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    [Button]
    public void test1()
    {
        print("ya");
        var a = AgainstInfo.cardSet[Orientation.Down];
        var b = a[RegionTypes.Leader];
        b.Add(new Card());
       
        print("yaya");
    }
}
