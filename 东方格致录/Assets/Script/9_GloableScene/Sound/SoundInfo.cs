using UnityEngine;
namespace Info
{
    public class SoundInfo : MonoBehaviour
    {
        public AudioClip[] Clips;
        public static SoundInfo Instance;
        public static AudioClip[] StaticClips => Instance.Clips;
        void Awake() => Instance = this;
    }
}
