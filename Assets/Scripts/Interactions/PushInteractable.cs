using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushInteractable : Interactable
{
    public Transform playerRef;

    public float MoveSpeed = 5.0f;

    [SerializeField]
    private Vector3 pushDirection;

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
        pushDirection = (transform.position - playerRef.transform.position);
        if (Mathf.Abs(pushDirection.z) > Mathf.Abs(pushDirection.x))
        {
            pushDirection = new Vector3(0, 0, pushDirection.z).normalized;
        }
        else
        {
            pushDirection = new Vector3(pushDirection.x, 0, 0).normalized;
        }
    }

    public override void Interact()
    {
        Debug.Log("interacting with " + gameObject.name);
        Push();
    }

    public void Push()
    {
        if (!moving)
        {
            targetPosition = transform.position + pushDirection;
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
