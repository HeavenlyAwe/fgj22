using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushInteractable : Interactable
{

    // TODO: The commented code could be useful if smoothly animating the movement...

    //public float MoveSpeed = 5.0f;

    [SerializeField]
    private Vector3 pushDirection;

    //private Vector3 targetPosition;

    //private bool moving = false;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    targetPosition = transform.position;
    //}

    public override bool Interact(Transform interactor, int factionSize)
    {
        if (!base.Interact(interactor, factionSize))
        {
            return false;
        }

        //Debug.Log("interacting with " + gameObject.name);
        //Push(interactor);

        pushDirection = (transform.position - interactor.position);
        if (Mathf.Abs(pushDirection.z) > Mathf.Abs(pushDirection.x))
        {
            pushDirection = new Vector3(0, 0, pushDirection.z).normalized;
        }
        else
        {
            pushDirection = new Vector3(pushDirection.x, 0, 0).normalized;
        }

        transform.position += pushDirection;

        return true;
    }

    //public void Push(Transform interactor)
    //{
    //    if (!moving)
    //    {
    //        pushDirection = (transform.position - interactor.position);
    //        if (Mathf.Abs(pushDirection.z) > Mathf.Abs(pushDirection.x))
    //        {
    //            pushDirection = new Vector3(0, 0, pushDirection.z).normalized;
    //        }
    //        else
    //        {
    //            pushDirection = new Vector3(pushDirection.x, 0, 0).normalized;
    //        }

    //        targetPosition = transform.position + pushDirection;
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
