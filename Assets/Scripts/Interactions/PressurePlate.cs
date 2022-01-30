using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour
{
    public UnityEvent<int, bool> pressed = new UnityEvent<int, bool>();
    
    protected void InvokePressed(bool state)
    {
        pressed.Invoke(this.GetInstanceID(), state);
    }
}
