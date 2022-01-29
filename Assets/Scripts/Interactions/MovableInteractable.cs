using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableInteractable : Interactable
{
    public override bool Interact(Vector3 moveOffset, int factionSize)
    {
        if (!base.Interact(moveOffset, factionSize))
        {
            return false;
        }

        transform.position += moveOffset;

        return true;
    }

    public override bool IsGrabbable()
    {
        return true;
    }
}

