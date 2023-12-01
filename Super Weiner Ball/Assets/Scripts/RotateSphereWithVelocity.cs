using System;
using UnityEngine;
using UnityEngine.Serialization;

public class RotateSphereWithVelocity : MonoBehaviour
{
    public Rigidbody rb;
    public SphereCollider sphere;
    [FormerlySerializedAs("camera")] public Transform cameraTransform;
    [SerializeField] private float soundFxTimeout;

    private float _rotationTracker;
    private float _timeSinceAudioTrigger;
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Get the velocity of the Rigidbody
        var velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        var axis = Vector3.Cross(velocity, -1 * cameraTransform.up);
        var angle = (velocity.magnitude * 180 / (2 * Mathf.PI * sphere.radius));

        var angleDeltaTime = angle * Time.deltaTime;
        transform.Rotate(axis, angleDeltaTime, Space.World);
        
        transform.position = rb.transform.position;

        _rotationTracker += angleDeltaTime;
        _timeSinceAudioTrigger += Time.deltaTime;
        
        if (_rotationTracker > 270 && _timeSinceAudioTrigger > soundFxTimeout)
        {
            _timeSinceAudioTrigger = 0;
            _rotationTracker = 0;
            _audioSource.Play();
        }
    }
}