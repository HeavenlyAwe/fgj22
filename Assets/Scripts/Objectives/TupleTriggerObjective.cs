using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TupleTriggerObjective : MonoBehaviour
{
    public PlayerPressurePlate pressurePlate1;
    public PlayerPressurePlate pressurePlate2;

    public CallbackAction callbackAction;

    private bool pressed1 = false;
    private bool pressed2 = false;


    // Start is called before the first frame update
    void Start()
    {
        pressurePlate1.pressed.AddListener(CheckPressed1);
        pressurePlate2.pressed.AddListener(CheckPressed2);
    }

    public void CheckPressed1(bool pressed)
    {
        pressed1 = pressed;
    }

    public void CheckPressed2(bool pressed)
    {
        pressed2 = pressed;
    }

    void Update()
    {
        if (pressed1 && pressed2)
        {
            Debug.Log("Mission complete!");
            if (callbackAction != null)
            {
                callbackAction.Callback();
            }
            this.enabled = false;
        }
    }
}
