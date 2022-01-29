using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableInteractable : Interactable
{
    public override void Interact(Vector3 moveOffset)
    {
        transform.position += moveOffset;
    }

    public override bool IsGrabbable()
    {
        return true;
    }
}

