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
    [ShowInInspector]
    public CardSet cardSet => AgainstInfo.cardSet;
    public float a, b, c;
    [ShowInInspector] public float exp_a => Mathf.Exp(a);
    [ShowInInspector] public float exp_b => Mathf.Exp(b);
    [ShowInInspector] public float exp_c => Mathf.Exp(c);
    [ShowInInspector] public float P_A => exp_a / (exp_a + exp_b + exp_c);
    [ShowInInspector] public float P_B => exp_b / (exp_a + exp_b + exp_c);
    [ShowInInspector] public float P_C => exp_c / (exp_a + exp_b + exp_c);
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
