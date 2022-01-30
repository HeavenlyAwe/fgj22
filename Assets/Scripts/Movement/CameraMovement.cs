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

    float SignedAngleBetween(Vector3 a, Vector3 b, Vector3 n)
    {
        // angle in [0,180]
        float angle = Vector3.Angle(a, b);
        float sign = Mathf.Sign(Vector3.Dot(n, Vector3.Cross(a, b)));

        // angle in [-179,180]
        float signed_angle = angle * sign;

        // angle in [0,360] (not used but included here for completeness)
        //float angle360 =  (signed_angle + 180) % 360;

        return signed_angle;
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

        mainCamera.transform.LookAt(lookAtPosition, Vector3.up);
        
        mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, desiredPosition, ref _velocity, smoothSpeed);
        // Emils code end
        
        //mainCamera.transform.position = lookAtPosition + offset;
    }
}
