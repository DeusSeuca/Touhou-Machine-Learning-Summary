using Extension;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
namespace Control
{
    namespace GameUI
    {
        public class NoticeControl : MonoBehaviour
        {
            //Animator Anim;
            public float Aplha = 0;
            //public GameObject[] NoticeTex;
            ////public GameObject NoticeModel;
            //void Start() => Anim = GetComponent<Animator>();
            //void Update() => NoticeTex.ForEach(x => x.GetComponent<Image>().material.SetFloat("_Value", Aplha));
            //[Button]
            //public void AnimPlay() => Anim.SetTrigger("Play");
            void Update() => GetComponent<Image>().material.SetFloat("_Value", Aplha);

        }
    }
}