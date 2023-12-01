using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class PlayerBonkManager : MonoBehaviour
{
    private AudioSource _audioSource;
    //[SerializeField] private VisualEffect visualEffect;
    [SerializeField] private ParticleSystem particleSystem;
    
    [SerializeField] private AudioClip smolBonk;
    private Player _player;
    private bool _timedOut;

    [Range(0,10)]
    [SerializeField] private float impulseMinThreshold = 5;
    [Range(0,50)]
    [SerializeField] private float impulseMaxThreshold = 15;
    [Range(0,1)]
    [SerializeField] private float bonkTimeout = .5f;
    
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _player = GetComponent<Player>();
    }

    private void OnCollisionEnter(Collision other)
    {
        HandleCollision(other);
    }

    private void OnCollisionStay(Collision other)
    {
        HandleCollision(other);
    }
    private void HandleCollision(Collision other)
    {
        if (_player.IsInputDisabled() || _timedOut) return;

        if (!(other.impulse.magnitude > impulseMinThreshold)) return;

        var scale = Mathf.Clamp(other.impulse.magnitude / impulseMaxThreshold, 0, 1);
        _audioSource.PlayOneShot(smolBonk, scale);

        var particleCount = (int) (scale * 25);
        /*visualEffect.SetInt("ParticleCount", particleCount);
        visualEffect.gameObject.transform.position = other.contacts[0].point;
        visualEffect.Play();*/

        particleSystem.emission.SetBurst(0, new ParticleSystem.Burst(0, particleCount));
        particleSystem.gameObject.transform.position = other.contacts[0].point;
        particleSystem.Play();
        StartCoroutine(TimeoutCoroutine());
        StartCoroutine(RumbleCoroutine(scale));
    }


    private IEnumerator TimeoutCoroutine()
    {
        _timedOut = true;
        yield return new WaitForSeconds(bonkTimeout);
        _timedOut = false;
    }   
    private IEnumerator RumbleCoroutine(float scale)
    {
        if (Gamepad.current == null) yield break;
        
        Gamepad.current.SetMotorSpeeds(.6f * scale, 1.5f * scale);
        yield return new WaitForSeconds(.5f * scale);
        Gamepad.current.ResetHaptics();
    }
}
