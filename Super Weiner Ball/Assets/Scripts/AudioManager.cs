using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public SoundEffects soundEffects;
    private AudioSource _audioSource;
    [SerializeField] private AudioSource musicAudioSource;


    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlaySoundEffectPauseMusic(AudioClip soundEffect)
    {
        StartCoroutine(SoundEffectCoroutine(soundEffect));
    }
    
    public void RaiseMusicVolume()
    {
        musicAudioSource.volume = .7f;
    }
    public void LowerMusicVolume()
    {
        musicAudioSource.volume = .4f;
    }

    public void PauseMusic()
    {
        musicAudioSource.Pause();
    }
    public void PlaySoundEffect(AudioClip soundEffect)
    {
        _audioSource.PlayOneShot(soundEffect);
    }

    IEnumerator SoundEffectCoroutine(AudioClip clip)
    {
        var clipLength = clip.length;
        musicAudioSource.volume = .5f;
        _audioSource.PlayOneShot(clip);
        yield return new WaitForSeconds(clipLength); 
        musicAudioSource.volume = 1;
    }
}