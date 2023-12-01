using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : Singleton<MusicManager>
{
    [SerializeField] private AudioMixerGroup pitchBendGroup;
    
    private AudioSource _music;


    protected override void Awake()
    {
        _music = GetComponent<AudioSource>();
        if (InstanceExists() && instance._music.clip != _music.clip)
        {
            instance._music.clip = _music.clip;
        }
        
        base.Awake();
        instance._music.volume = 1;
        
        if(!instance._music.isPlaying)
            instance._music.Play();
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        ResetMusicSpeed();
    }

    public void SetMusicSpeed(float pitch)
    {
        pitch = Mathf.Clamp(pitch, -3, 3);
        _music.pitch = pitch; 
        pitchBendGroup.audioMixer.SetFloat("pitchBend", 1f / pitch);
    }

    private void ResetMusicSpeed()
    {
        _music.pitch = 1; 
        pitchBendGroup.audioMixer.SetFloat("pitchBend", 1f);
    }

    public void PauseMusic()
    {
        _music.Pause();
    }

    public void SetVolume(float volume)
    {
        _music.volume = volume;
    }
}
