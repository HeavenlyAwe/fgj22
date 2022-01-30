using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectives : MonoBehaviour
{
    public PlayerPressurePlate pressurePlate1;
    public PlayerPressurePlate pressurePlate2;

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
        Debug.Log("Pressed 1: " + pressed);
        pressed1 = pressed;
    }

    public void CheckPressed2(bool pressed)
    {
        Debug.Log("Pressed 2: " + pressed);
        pressed2 = pressed;
    }

    void Update()
    {
        if (pressed1 && pressed2)
        {
            Debug.Log("Mission complete!");
        }
    }
}
