using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Vector3 offset = new Vector3(0, 5, -10);
    public float smoothSpeed = 1.0f;

    private Transform[] followTransforms;

    private Camera mainCamera;

    private Vector3 _velocity = Vector3.zero;


    private void LookupAllPlayers()
    {
        PlayerMovement[] players = (PlayerMovement[])GameObject.FindObjectsOfType(typeof(PlayerMovement));
        followTransforms = new Transform[players.Length];
        for(int i = 0; i < players.Length; i++)
        {
            followTransforms[i] = players[i].transform;
        }
    }

    void Awake()
    {
        LookupAllPlayers();
    }

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
