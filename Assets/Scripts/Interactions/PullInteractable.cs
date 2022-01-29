using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullInteractable : Interactable
{
    public Transform playerRef;

    public float MoveSpeed = 5.0f;

    [SerializeField]
    private Vector3 pullDirection;

    private Vector3 targetPosition;

    private bool moving = false;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        targetPosition = transform.position;
    }

    private void GetPushPullDirection()
    {
        Vector3 pushDirection = (transform.position - playerRef.transform.position);
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

    public override void Interact()
    {
        Pull();
    }

    public void Pull()
    {
        if (!moving)
        {
            targetPosition = transform.position + pullDirection;
            moving = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        GetPushPullDirection();

        if (moving)
        {
            transform.position = targetPosition;
            moving = false;
        }
    }
}
