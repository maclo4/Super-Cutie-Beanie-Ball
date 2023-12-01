using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SoundEffects", order = 1)]
public class SoundEffects : ScriptableObject
{
    public AudioClip bonk;
    public AudioClip crowdCheering;

    public AudioClip GetRandom(List<AudioClip> soundEffectList)
    {
        return soundEffectList[Random.Range(0, soundEffectList.Count)];
    }
}
