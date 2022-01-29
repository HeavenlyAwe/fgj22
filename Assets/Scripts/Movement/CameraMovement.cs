using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Vector3 offset = new Vector3(0, 5, -10);
    public Transform[] followTransforms;
    public float smoothSpeed = 1.0f;
    
    private Camera mainCamera;

    private Vector3 _velocity = Vector3.zero;

    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lookAtPosition = Vector3.zero;
        foreach (Transform t in followTransforms)
        {
            lookAtPosition += t.position;
        }
        lookAtPosition /= followTransforms.Length;
        
        // Emils code begin
        Vector3 desiredPosition = lookAtPosition + offset;
        mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, desiredPosition, ref _velocity, smoothSpeed);
        // Emils code end
        
        //mainCamera.transform.position = lookAtPosition + offset;
    }
}
