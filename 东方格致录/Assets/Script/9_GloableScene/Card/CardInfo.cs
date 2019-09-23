using CardSpace;
using Sirenix.OdinInspector;
using UnityEngine;
namespace Info
{
    public class CardInfo : MonoBehaviour
    {
        public static GameObject cardModel;
        public  GameObject card_Model;
        private void Awake() => cardModel = card_Model;
    }
}

