using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class DogTreatTrigger : MonoBehaviour
{
    private AudioSource _audioSource;
    private BoxCollider _boxCollider;
    private MeshRenderer _meshRenderer;
    private List<MeshRenderer> _childMeshRenderers;
    [SerializeField] private int pointValue = 1;
    [SerializeField] private int rotationSpeed = 90;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _boxCollider = GetComponent<BoxCollider>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _childMeshRenderers = GetComponentsInChildren<MeshRenderer>().ToList();
        var randomAngle = Random.Range(0, 360);
        transform.Rotate(Vector3.up, randomAngle);
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        
        ScoreTrackingManager.Instance.AddScore(pointValue);
        
        _audioSource.Play();
        _boxCollider.enabled = false;
        _meshRenderer.enabled = false;
        _childMeshRenderers.ForEach(_ => _.enabled = false);
        enabled = false;
    }
}
