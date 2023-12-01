using UnityEngine;
using UnityEngine.InputSystem;
// ReSharper disable Unity.InefficientPropertyAccess

public class Player : MonoBehaviour
{
    
    [SerializeField]
    private Transform cameraFollow; // Maximum tilt angle in degrees
    
    [SerializeField]
    private float maxTiltAngle = 45.0f; // Maximum tilt angle in degrees

    [SerializeField] 
    private float gravityConstant = 9.5f;
    
    [SerializeField] 
    private float turnSpeed = 180f;

    [SerializeField]
    private LevelCompleteAnimationController levelCompleteAnimationController;
    
    [SerializeField]
    private float maxFallSpeed;
    
    [SerializeField] 
    private float maxHorizontalSpeed = 25f;
    
    private Rigidbody _rb;
    private SphereCollider _sphereCollider;
    private bool _disableInput = true;
    private bool _disableGravity;
    private float _targetZTiltAngle;
    private float _targetXTiltAngle;
    private float _smoothDampVelocity;
    private Vector2 _tiltInput;

    private void Start()
    {
        maxFallSpeed = Mathf.Abs(maxFallSpeed);
        gravityConstant = Mathf.Abs(gravityConstant);
        _rb = GetComponentInChildren<Rigidbody>();
        _sphereCollider = GetComponentInChildren<SphereCollider>();
    }

    public void Tilt(InputAction.CallbackContext context)
    {
        if (_disableInput) return;
        
        var input = context.ReadValue<Vector2>();

        if (Mathf.Abs(input.x) < .1f)
            input.x = 0;
        if (Mathf.Abs(input.y) < .1f)
            input.y = 0;

        _tiltInput = input;
    }

    private void FixedUpdate()
    {
        if (_disableGravity) return;
        
        // Limit horizontal speed
        var horizontalVelocity = new Vector2(_rb.velocity.x, _rb.velocity.z);
        var currentHorizontalSpeed = horizontalVelocity.magnitude;
        var dot = Vector2.Dot(new Vector2(horizontalVelocity.normalized.x, horizontalVelocity.normalized.y), 
            new Vector2(cameraFollow.up.x, cameraFollow.up.z));

        var multiplier = maxHorizontalSpeed / dot / currentHorizontalSpeed;
        
        if (currentHorizontalSpeed > maxHorizontalSpeed && dot < -0.01f && multiplier > -1)
        {
            var scaledForce = cameraFollow.up * multiplier * gravityConstant;
            var force = cameraFollow.up *  -gravityConstant;
            _rb.AddForce(new Vector3(scaledForce.x, force.y, scaledForce.z) * Time.deltaTime, ForceMode.Impulse);
        }
        else
        {
            _rb.AddForce(cameraFollow.up * -gravityConstant * Time.deltaTime, ForceMode.Impulse);
        }

        // Limit vertical speed
        if (_rb.velocity.y < -maxFallSpeed)
        {
            _rb.velocity = new Vector3(_rb.velocity.x, -maxFallSpeed, _rb.velocity.z);
        }
    }

    private void Update()
    {
        // Calculate the desired rotation angle based on the input
        _targetZTiltAngle = Mathf.Clamp(_tiltInput.x * maxTiltAngle, -maxTiltAngle, maxTiltAngle);

        // Calculate the desired rotation angle based on the input
        _targetXTiltAngle = Mathf.Clamp(-_tiltInput.y * maxTiltAngle, -maxTiltAngle, maxTiltAngle);

        var currYRotation = Quaternion.LookRotation(cameraFollow.forward);
        
        
        
        var horizontalVelocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
        
        if(horizontalVelocity == Vector3.zero)
        {
            cameraFollow.rotation = Quaternion.Euler(_targetXTiltAngle, cameraFollow.rotation.eulerAngles.y,
                _targetZTiltAngle);
            cameraFollow.position = transform.position;
            return;
        }
        
        var targetYRotation = Quaternion.LookRotation(horizontalVelocity, Vector3.up);

        var targetEulerRotation = targetYRotation.eulerAngles;
        var currEulerRotation = currYRotation.eulerAngles;

        currEulerRotation.y %= 360;
        targetEulerRotation.y %= 360;

        var velocityTurnMultiplier = horizontalVelocity.magnitude > 2 ? 1 : horizontalVelocity.magnitude / 2;
        var turnAmount = velocityTurnMultiplier * turnSpeed * Time.deltaTime;

        if (Mathf.Abs(targetEulerRotation.y - currEulerRotation.y) <= turnAmount)
        {
            currEulerRotation.y = targetEulerRotation.y;
        }
        else if (targetEulerRotation.y > currEulerRotation.y)
        {
            if (targetEulerRotation.y - currEulerRotation.y < 180)
            {
                currEulerRotation.y += turnAmount;
            }
            else
            {
                currEulerRotation.y -= turnAmount;
            }
        }        
        else
        {
            if (currEulerRotation.y - targetEulerRotation.y < 180)
            {
                currEulerRotation.y -= turnAmount;
            }
            else
            {
                currEulerRotation.y += turnAmount;
            }
        }
        
        cameraFollow.rotation = Quaternion.Euler(_targetXTiltAngle, currEulerRotation.y, _targetZTiltAngle);
        cameraFollow.position = transform.position;
    }
    
    public void DisableInputAndGravity()
    {
        _disableInput = true;
        _disableGravity = true;
    }
    public void EnableInputAndGravity()
    {
        _disableInput = false;
        _disableGravity = false;
    }

    public void DisableCollision()
    {
        _sphereCollider.enabled = false;
    }

    public bool IsInputDisabled()
    {
        return _disableInput;
    }
}
