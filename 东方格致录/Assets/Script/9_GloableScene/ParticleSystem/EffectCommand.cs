using UnityEngine;
namespace Command
{
    public class EffectCommand
    {
        public static void AudioEffectPlay(int Rank)
        {
            MainThread.Run(() =>
            {
                AudioSource Source = Info.SoundInfo.Instance.gameObject.AddComponent<AudioSource>();
                Source.clip = Info.SoundInfo.Instance.Clips[Rank];
                Source.Play();
                GameObject.Destroy(Source, Source.clip.length);
            });
        }
        public static void ParticlePlay(int Rank, Vector3 Position)
        {
            MainThread.Run(() =>
            {
                ParticleSystem TargetParticle = GameObject.Instantiate(Info.ParticleInfo.Instance.ParticleEffect[Rank]);
                TargetParticle.transform.position = Position;
                TargetParticle.Play();
                GameObject.Destroy(TargetParticle,2);
            });
        }
    }
}

