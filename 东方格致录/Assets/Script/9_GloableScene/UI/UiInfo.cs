using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Info
{
    public class UiInfo : MonoBehaviour
    {
        public GameObject Arrow_model;
        public GameObject ArrowEndPoint_Model;
        public GameObject NoticeBoard_model;
        public static string NoticeBoardTitle { get; set; }
        public static string CardBoardTitle { get; set; }
        public Animator NoticeAnim;
        public GameObject MyPass;
        public GameObject OpPass;
        public Transform ConstantInstance;
        public GameObject CardBoardInstance;
        public GameObject CardIntroductionModel;
        public static Text IntroductionTitle => Instance.CardIntroductionModel.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>();
        public static Text IntroductionText => Instance.CardIntroductionModel.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>();
        public static RectTransform IntroductionTextBackground => Instance.CardIntroductionModel.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
        public static Text IntroductionEffect => Instance.CardIntroductionModel.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>();
        public static RectTransform IntroductionEffectBackground => Instance.CardIntroductionModel.transform.GetChild(0).GetChild(1).GetComponent<RectTransform>();

        public static Dictionary<int, Sprite> CardImage = new Dictionary<int, Sprite>();
        public GameObject CardInstanceModel;
        public static List<GameObject> ShowCardLIstOnBoard = new List<GameObject>();

        public static GameObject Arrow => Instance.Arrow_model;
        public static GameObject ArrowEndPoint => Instance.ArrowEndPoint_Model;
        public static Transform Constant => Instance.ConstantInstance;
        public static GameObject CardModel => Instance.CardInstanceModel;
        public static GameObject CardBoard => Instance.CardBoardInstance;
        public static GameObject NoticeBoard => Instance.NoticeBoard_model;
        public static UiInfo Instance;
        private void Awake() => Instance = this;
    }
}

