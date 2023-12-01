using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BeanieAnimatorController : MonoBehaviour
{
    [SerializeField] Rigidbody playerRb;
    [SerializeField] Player player;
    [SerializeField] private Transform cameraFollow;
    [SerializeField] private float forwardOffset = .1f;
    [SerializeField] private float upOffset = -.1f;
    [SerializeField] private float smoothDampTime = 1;
    private Vector3 smoothDampVelocity;

    private Animator _animator;
    
    private static readonly int WalkForward = Animator.StringToHash("WalkForward");
    private static readonly int Celebrate = Animator.StringToHash("Celebrate");
    private bool _cutscene;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_cutscene)
            return;

        _animator.speed = Mathf.Clamp(playerRb.velocity.magnitude / 5, 0, 3);
        
        var dot = Vector3.Dot(playerRb.velocity, transform.forward);
        _animator.SetBool(WalkForward, dot >= 0);
    }

    private void LateUpdate()
    {
        var targetPosition = cameraFollow.position + cameraFollow.forward * forwardOffset/*desired offset value*/;
        targetPosition.y += upOffset;
        transform.position = targetPosition;

        var cameraFollowEulers = cameraFollow.rotation.eulerAngles;
        
        transform.rotation = Quaternion.Euler(-cameraFollowEulers.x, cameraFollowEulers.y,
            -cameraFollowEulers.z);
    }

    public void CelebrateAnimation()
    {

        _animator.speed = 1;
        _animator.SetBool(Celebrate, true);
        _cutscene = true;
    }

    private IEnumerator CelebrateCoroutine()
    {
        _animator.SetBool(Celebrate, true);

        _cutscene = true;
        
        yield return null;
        yield return new WaitUntil(() => 
            _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= .7f);
        
        _cutscene = false;
    }
}