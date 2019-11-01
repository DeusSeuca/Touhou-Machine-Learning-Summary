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
            public Color a;
            Color targetColor = new Color(0, 0, 0, 0);
            private static readonly Image image = Info.GameUI.UiInfo.NoticeBoard.GetComponent<Image>();
            private static readonly Text text = Info.GameUI.UiInfo.NoticeBoard.transform.GetChild(0).GetComponent<Text>();
            Vector3 targetAugel = new Vector3(0, 0, 0);
            Vector3 currentAugel => Info.GameUI.UiInfo.NoticeBoard.transform.eulerAngles;
            //public GameObject[] NoticeTex;
            ////public GameObject NoticeModel;
            //void Start() => Anim = GetComponent<Animator>();
            //void Update() => NoticeTex.ForEach(x => x.GetComponent<Image>().material.SetFloat("_Value", Aplha));
            //[Button]
            //public void AnimPlay() => Anim.SetTrigger("Play");
            void Update()
            {
                if (Info.GameUI.UiInfo.isNoticeBoardShow)
                {
                    targetColor = new Color(1, 1, 1, 1);
                    targetAugel = new Vector3(0, 0, 0);

                }
                else
                {
                    targetColor = new Color(0, 0, 0, 0);
                    targetAugel = new Vector3(90, 0, 0);
                }
                image.color = Color.Lerp(image.color, targetColor, Time.deltaTime);
                text.color = Color.Lerp(text.color, targetColor, Time.deltaTime);
                Info.GameUI.UiInfo.NoticeBoard.transform.eulerAngles = Vector3.Lerp(currentAugel, targetAugel, Time.deltaTime);

                // Info.GameUI.UiInfo.NoticeBoard.GetComponent<Image>().material.SetFloat("_Value", Aplha);
                // Info.GameUI.UiInfo.NoticeBoard.GetComponent<Image>().color = new Color(1, 1, 1, Aplha);
            }
        }
    }
}