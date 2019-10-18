using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Info
{
    namespace GameUI
    {
        public class UiInfo : MonoBehaviour
        {
            public static UiInfo Instance;
            public GameObject MyPass;
            public GameObject OpPass;
            public Animator NoticeAnim;
            public GameObject Arrow_Model;
            public GameObject CardInstanceModel;
            public GameObject NoticeBoard_Model;
            public GameObject ArrowEndPoint_Model;
            public Transform ConstantInstance;
            public GameObject CardBoardInstance;
            public GameObject CardIntroductionModel;
            private void Awake() => Instance = this;

            public static string CardBoardTitle = "";
            public static string NoticeBoardTitle = "";

            public static List<GameObject> ShowCardLIstOnBoard = new List<GameObject>();
            public static Dictionary<int, Sprite> CardImage = new Dictionary<int, Sprite>();

            public static GameObject Arrow => Instance.Arrow_Model;
            public static GameObject ArrowEndPoint => Instance.ArrowEndPoint_Model;
            public static Transform Constant => Instance.ConstantInstance;
            public static GameObject CardModel => Instance.CardInstanceModel;
            public static GameObject CardBoard => Instance.CardBoardInstance;
            public static GameObject NoticeBoard => Instance.NoticeBoard_Model;
            public static Text IntroductionTitle => Instance.CardIntroductionModel.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>();
            public static Text IntroductionText => Instance.CardIntroductionModel.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>();
            public static Text IntroductionEffect => Instance.CardIntroductionModel.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>();

            public static RectTransform IntroductionTextBackground => Instance.CardIntroductionModel.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
            public static RectTransform IntroductionEffectBackground => Instance.CardIntroductionModel.transform.GetChild(0).GetChild(1).GetComponent<RectTransform>();
        }
    }
}