using UnityEngine;
namespace Info
{
    public class ParticleInfo : MonoBehaviour
    {
        public static ParticleInfo Instance;
        public ParticleSystem[] ParticleEffect;
        private void Awake() => Instance = this;
    }
}

