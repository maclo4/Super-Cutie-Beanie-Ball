using System;
using UnityEngine;
using UnityEngine.Audio;

public class TimerManager : Singleton<TimerManager>
{
    [SerializeField] private float timer = 60;
    [SerializeField] private AudioClip timeOut;
    [SerializeField] private AudioMixerGroup pitchBendGroup;
    private bool _active;

    private LevelTransitionManager _levelTransitionManager;
    private AudioSource _audioSource;
    private MusicManager _musicManager;
    private float _pitch1 = 1.1f;
    private float _pitch2 = 1.2f;
    private float _pitch1CurrVel;
    private float _pitch2CurrVel;

    private void Start()
    {
        _levelTransitionManager = LevelTransitionManager.Instance;
        _audioSource = GetComponent<AudioSource>();
        _musicManager = MusicManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_active) return;

        timer -= Time.deltaTime;


        if (timer <= 0)
        {
            _audioSource.clip = timeOut;
            _audioSource.loop = false;
            _musicManager.PauseMusic();
            _audioSource.Play();
            _levelTransitionManager.TimedOut();
            timer = 0;
            _active = false;
        }
        else if (timer <= 5)
        {
            var smoothedPitch = 
                Mathf.SmoothDamp(_audioSource.pitch, _pitch2, ref _pitch2CurrVel, 1f);
            _audioSource.pitch = smoothedPitch;
            pitchBendGroup.audioMixer.SetFloat("pitchBend", 1f / smoothedPitch);
        }
        else if (timer <= 15)
        {
            if (!_audioSource.isPlaying)
            {
                _musicManager.SetVolume(.9f);
                _audioSource.volume = 2f;
                _audioSource.Play();
            }            
            
            var smoothedPitch = 
                Mathf.SmoothDamp(_audioSource.pitch, _pitch1, ref _pitch1CurrVel, 2f);
            _audioSource.pitch = smoothedPitch;
            pitchBendGroup.audioMixer.SetFloat("pitchBend", 1f / smoothedPitch);
            
        }
    }

    public float GetTimer()
    {
        return timer;
    }

    public void SetActive(bool active)
    {
        _active = active;
        _audioSource.enabled = active;
    }
}
