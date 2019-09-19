using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
namespace Control
{
    namespace GameUI
    {
        public class NoticeControl : MonoBehaviour
        {
            public GameObject[] NoticeTex;
            public GameObject NoticeModel;
            public float Aplha = 0;
            Animator Anim;
            void Start() => Anim = GetComponent<Animator>();
            void Update() => NoticeTex.ForEach(x => x.GetComponent<Image>().material.SetFloat("_Value", Aplha));
            [Button]
            public void AnimPlay() => Anim.SetTrigger("Play");
        }
    }
}