using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushPullInteractable : MonoBehaviour
{
    public Transform playerRef;

    [SerializeField]
    private Vector3 pushDirection;
    [SerializeField]
    private Vector3 pullDirection;

    private Vector3 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        targetPosition = transform.position;
    }

    private void GetPushPullDirection()
    {
        pushDirection = (transform.position - playerRef.transform.position);
        if (Mathf.Abs(pushDirection.z) > Mathf.Abs(pushDirection.x))
        {
            pushDirection = new Vector3(0, 0, pushDirection.z).normalized;
        }
        else
        {
            pushDirection = new Vector3(pushDirection.x, 0, 0).normalized;
        }
        pullDirection = -pushDirection;
    }

    public void Push()
    {
        targetPosition = transform.position + pushDirection;
    }

    public void Pull()
    {
        targetPosition = transform.position + pullDirection;
    }

    // Update is called once per frame
    void Update()
    {
        GetPushPullDirection();

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime);
    }
}
