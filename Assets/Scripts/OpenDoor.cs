using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : CallbackAction
{

    public Animator fireDoorAnimator;
    public Animator waterDoorAnimator;
    
    
    public override void Callback()
    {
        GetComponent<BoxCollider>().enabled = false;
        fireDoorAnimator.SetBool("isOpen", true);
        waterDoorAnimator.SetBool("isOpen", true);
    }
    
}
