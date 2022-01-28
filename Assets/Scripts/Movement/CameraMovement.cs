using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Vector3 offset = new Vector3(0, 5, -10);
    public Transform[] followTransforms;
    
    private Camera mainCamera;

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

        mainCamera.transform.position = lookAtPosition + offset;
    }
}
