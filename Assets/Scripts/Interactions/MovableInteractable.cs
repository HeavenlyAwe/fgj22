using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableInteractable : Interactable
{
    [SerializeField]
    private Vector3 moveDirection;

    private Vector3 targetPosition;

    private bool moving = false;

    // Start is called before the first frame update
    void Start()
    {
        targetPosition = transform.position;
    }

    public override void Interact(Vector3 moveOffset)
    {
        //targetPosition = transform.position + moveOffset;
        transform.position += moveOffset;
        //moving = true;
    }

    // Update is called once per frame
    void Update()
    {
        //if (moving)
        //{
        //    Debug.Log("Updating the position");
        //    transform.position = targetPosition;
        //    moving = false;
        //}
    }

    public override bool IsGrabbable()
    {
        return true;
    }
}

