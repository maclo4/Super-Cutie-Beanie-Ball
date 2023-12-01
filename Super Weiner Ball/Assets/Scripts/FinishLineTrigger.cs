using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FinishLineTrigger : MonoBehaviour
{
    private AudioSource _audioSource;
    private AudioManager _audioManager;
    private LevelTransitionManager _levelTransitionManager;

    private void Start()
    {
        _audioManager = AudioManager.Instance;
        _levelTransitionManager = LevelTransitionManager.Instance;
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        _audioManager.PlaySoundEffect(_audioManager.soundEffects.crowdCheering);
        _levelTransitionManager.LoadNextScene();
    }
}
