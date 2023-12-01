using UnityEngine;

public class IntroLookAtController : MonoBehaviour
{
    private Player _player;
    private Vector3 _smoothDampVelocity;
    private float _smoothDampTime;
    private bool _moveTowardsPlayer;
    private float _speed;

    // Start is called before the first frame update
    void Start()
    {

        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_moveTowardsPlayer) return;
        
        transform.position = Vector3.MoveTowards(
            transform.position, _player.transform.position, _speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, _player.transform.position) < .01)
            enabled = false;
    }

    public void SmoothDampToPlayer(float animationLength)
    {

        var distance = Vector3.Distance(transform.position, _player.transform.position);
        _speed =  distance / animationLength;
       // _smoothDampTime = animationLength;
        _moveTowardsPlayer = true;
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        //_smoothDampTime *= multiplier;
        _speed += multiplier;
    }
}
