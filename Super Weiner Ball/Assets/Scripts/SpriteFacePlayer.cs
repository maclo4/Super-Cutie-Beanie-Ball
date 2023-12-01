using UnityEngine;

public class SpriteFacePlayer : MonoBehaviour
{
    private Transform _player;

    private void Start()
    {
        // Find the player's transform
        _player = GameObject.FindGameObjectWithTag("Player").transform;

        if (_player == null)
        {
            Debug.LogError("Player not found in the scene!");
        }
    }

    void Update()
    {
        if (_player != null)
        {
            // Calculate the direction from the object to the player (only on the x-z plane)
            var directionToPlayer = _player.position - transform.position;
            directionToPlayer.y = 0; // Set the y component to 0 to ignore vertical difference

            // Calculate the rotation that looks at the player
            var targetRotation = Quaternion.LookRotation(directionToPlayer);

            // Apply the rotation to the object, only rotating on the y-axis
            transform.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
        }
    }
}