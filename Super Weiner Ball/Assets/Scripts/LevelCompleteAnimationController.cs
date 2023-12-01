using System.Collections;
using Cinemachine;
using UnityEngine;
// ReSharper disable Unity.InefficientPropertyAccess

public class LevelCompleteAnimationController : MonoBehaviour
{
    [SerializeField] Rigidbody playerRb;
    [SerializeField] CinemachineVirtualCamera cmCamera;
    [SerializeField] float smoothTime;
    [SerializeField] float spinAccel = 360;
    [SerializeField] float upForce = 35;
    [SerializeField] BeanieAnimatorController beanieAnimatorController;

    private RotateSphereWithVelocity _rotateSphereWithVelocity;
    private Vector3 _smoothDampVelocity;
    [SerializeField] private int maxYSpeed = 25;
    private float _spinSpeed;

    private void Start()
    {
        _rotateSphereWithVelocity = GetComponent<RotateSphereWithVelocity>();
    }

    public void BeginSpinning()
    {
        _rotateSphereWithVelocity.enabled = false;
        StartCoroutine(SpinningCoroutine());
    }

    public void BeginCelebration()
    {
        beanieAnimatorController.CelebrateAnimation();
    }
    public void FlyAway()
    {
        _rotateSphereWithVelocity.enabled = false;
        StopAllCoroutines();
        StartCoroutine(FlyAwayCoroutine());
    }

    private IEnumerator SpinningCoroutine()
    {
        var timer = 0f;
        _spinSpeed = 0.1f;
        
        var cameraUp = cmCamera.transform.up;
        while (timer < 10)
        {
            transform.Rotate(cameraUp, _spinSpeed * Time.deltaTime);
            playerRb.velocity = Vector3.SmoothDamp(
                playerRb.velocity, Vector3.zero, ref _smoothDampVelocity, smoothTime);
            
            timer += Time.deltaTime;
            
            if(_spinSpeed <= 720) 
                _spinSpeed += spinAccel * Time.deltaTime;
            
            transform.position = playerRb.transform.position;
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator FlyAwayCoroutine()
    {
        var timer = 0f;
        var currUpForce = .1f;
        
        while (timer < 10)
        {
            // Limit vertical speed
            if (playerRb.velocity.y >= maxYSpeed - 1)
            {
                playerRb.velocity = new Vector3(0, maxYSpeed, 0);
            }
            else
            {
                playerRb.AddForce(Vector3.up * upForce);
            }
            transform.Rotate(Vector3.up, _spinSpeed * Time.deltaTime);
            
            timer += Time.deltaTime;
            if(_spinSpeed <= 720) 
                _spinSpeed += spinAccel * Time.deltaTime;
            currUpForce += upForce * Time.deltaTime;
            
            transform.position = playerRb.transform.position;
            yield return new WaitForFixedUpdate();
        }
    }

    public void SmoothVelocityToZero()
    {
        StartCoroutine(SmoothVelocityToZeroCoroutine());
    }
    private IEnumerator SmoothVelocityToZeroCoroutine()
    {
        var timer = 0f;
        _spinSpeed = 0.1f;
        
        while (timer < 10)
        {
            playerRb.velocity = Vector3.SmoothDamp(
                playerRb.velocity, Vector3.zero, ref _smoothDampVelocity, smoothTime);
            
            timer += Time.deltaTime;
            
            transform.position = playerRb.transform.position;
            yield return new WaitForFixedUpdate();
        }
    }
}
