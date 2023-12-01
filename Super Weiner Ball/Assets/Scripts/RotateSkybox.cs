using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSkybox : MonoBehaviour
{
    public Transform mainCamera;
    private float _yForward;
    
    // Start is called before the first frame update
    void Start()
    {
        _yForward = mainCamera.forward.y;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Camera.main is not null && mainCamera == Camera.main.transform)
        {
            var cameraForward = Camera.main.transform.forward;
            transform.forward = new Vector3(cameraForward.x, _yForward, cameraForward.z);
        }
        else if (Camera.main is not null)
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
}
