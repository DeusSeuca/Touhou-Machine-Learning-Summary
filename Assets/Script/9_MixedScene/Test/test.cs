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
    public void test1()
    {
        //SceneManager.LoadSceneAsync(2);
        Command.StateCommand.Surrender();
        //Debug.Log(AgainstInfo.IsCurrectPass);
        //Task.Run(() =>
        //{
        //    //Debug.Log(AgainstInfo.IsCurrectPass);
        //});

        //if (AgainstInfo.IsCardEffectCompleted)
        //{
        //    Debug.Log("执行完毕？");
        //    AgainstInfo.IsCardEffectCompleted = false;
        //}
        //if (AgainstInfo.IsCurrectPass)
        //{
        //    Debug.Log("当前pass？");
        //    AgainstInfo.IsCardEffectCompleted = false;
        //}


        //print("ya");
        //var a = AgainstInfo.cardSet[Orientation.Down];
        //var b = a[RegionTypes.Leader];
        //b.Add(new Card());

        //print("yaya");
    }
}
