﻿using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoticeControl : MonoBehaviour
{
    public GameObject[] NotcreTex;
    public float Aplha = 0;
    Animator Anim;
    void Start() => Anim = GetComponent<Animator>();
    void Update() => NotcreTex.ForEach(x => x.GetComponent<Image>().material.SetFloat("_Value", Aplha));
    [Button]
    public void AnimPlay()
    {
        Anim.SetTrigger("Play");


    }
}
