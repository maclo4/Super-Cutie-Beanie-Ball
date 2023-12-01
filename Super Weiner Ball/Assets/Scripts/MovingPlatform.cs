using System;
using System.Collections.Generic;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] List<Transform> path;
    [SerializeField] float moveSpeed;
    private int _currPathIndex;
    private Vector3 _smoothDampVelocity;
    private Transform _ridingPlayer;
    private BoxCollider _boxCollider;
    private float _distance;
    private float _prevPlayerYPos;
    private Rigidbody _playerRb;
    private float _prevYPosition;


    private void Start()
    {
        _boxCollider = GetComponent<BoxCollider>();
    }
    
    // Update is called once per frame
    void Update()
    {
       // transform.position = Vector3.MoveTowards(
       //     transform.position, path[_currPathIndex].position, moveSpeed * Time.deltaTime);


       var position = transform.position;
       _prevYPosition = position.y;
       
       position = Vector3.SmoothDamp(position, path[_currPathIndex].position,
           ref _smoothDampVelocity, moveSpeed);
       transform.position = position;

       if (Vector3.Distance(transform.position, path[_currPathIndex].position) < .001f)
       {
           _currPathIndex= (_currPathIndex + 1) % path.Count;
       }
    }

    private void LateUpdate()
    {
        
        if (_ridingPlayer != null && _prevYPosition > transform.position.y)
        {
           var ridingPlayerPosition = _playerRb.position;
            _playerRb.MovePosition( new Vector3(ridingPlayerPosition.x, transform.position.y + _distance,
                ridingPlayerPosition.z));
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;

        var direction =  other.transform.position - transform.position;
        
        if (Vector3.Dot(Vector3.up, direction) > .5)
        {
            _ridingPlayer = other.transform;
            _distance = other.gameObject.GetComponent<SphereCollider>().radius + _boxCollider.bounds.extents.y;
            _playerRb = other.gameObject.GetComponent<Rigidbody>();
        }
    }
    
    
    private void OnCollisionExit(Collision other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;

        _ridingPlayer = null;
    }
}
