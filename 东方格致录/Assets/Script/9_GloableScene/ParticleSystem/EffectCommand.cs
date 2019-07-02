using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Command
{
    public class EffectCommand
    {
        public static void AudioEffectPlay(int Rank)
        {
            Info.GlobalBattleInfo.PlaySoundRank = Rank;
            Info.GlobalBattleInfo.IsPlaySound = true;
        }
        public static void ParticlePlay(int Rank, Vector3 Position)
        {
            Info.GlobalBattleInfo.PlayParticleRank = Rank;
            Info.GlobalBattleInfo.PlayParticlePos = Position;
            Info.GlobalBattleInfo.IsPlayParticle = true;
        }
        
    }
}

