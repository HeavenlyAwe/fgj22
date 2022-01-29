using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullInteractable : Interactable
{

    // TODO: The commented code could be useful if smoothly animating the movement...

    //public float MoveSpeed = 5.0f;

    [SerializeField]
    private Vector3 pullDirection;

    //private Vector3 targetPosition;

    //private bool moving = false;

    // Start is called before the first frame update
    //void Start()
    //{
    //    targetPosition = transform.position;
    //}

    public override void Interact(Transform interactor)
    {
        //Pull(interactor);

        //if (!moving)
        //{
        Vector3 pushDirection = (transform.position - interactor.transform.position);
        if (Mathf.Abs(pushDirection.z) > Mathf.Abs(pushDirection.x))
        {
            pushDirection = new Vector3(0, 0, pushDirection.z).normalized;
        }
        else
        {
            pushDirection = new Vector3(pushDirection.x, 0, 0).normalized;
        }
        pullDirection = -pushDirection;

        transform.position += pullDirection;
            //moving = true;
        //}
    }

    //public void Pull(Transform interactorTransform)
    //{
    //    if (!moving)
    //    {
    //        Vector3 pushDirection = (transform.position - interactorTransform.transform.position);
    //        if (Mathf.Abs(pushDirection.z) > Mathf.Abs(pushDirection.x))
    //        {
    //            pushDirection = new Vector3(0, 0, pushDirection.z).normalized;
    //        }
    //        else
    //        {
    //            pushDirection = new Vector3(pushDirection.x, 0, 0).normalized;
    //        }
    //        pullDirection = -pushDirection;

    //        targetPosition = transform.position + pullDirection;
    //        moving = true;
    //    }
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if (moving)
    //    {
    //        transform.position = targetPosition;
    //        moving = false;
    //    }
    //}

    public override bool IsGrabbable()
    {
        return false;
    }
}
