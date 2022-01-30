using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TupleTriggerObjective : MonoBehaviour
{
    public PressurePlate[] pressurePlates;
    public CallbackAction callbackAction;

    private Dictionary<int, bool> pressedDict = new Dictionary<int, bool>();

    // Start is called before the first frame update
    void Start()
    {
        foreach (PressurePlate plate in pressurePlates)
        {
            plate.pressed.AddListener(CheckPressed);
            pressedDict.Add(plate.GetInstanceID(), false);
        }
    }

    public void CheckPressed(int id, bool pressed)
    {
        pressedDict[id] = pressed;
    }

    void Update()
    {
        bool allPressed = true;
        foreach (int key in pressedDict.Keys)
        {
            allPressed &= pressedDict[key];
        }

        if (allPressed)
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
