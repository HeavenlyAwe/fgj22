using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public Interactable keyItem;
    public Color highlightColor;

    public CallbackAction callbackAction;

    private bool hasTriggered;

    void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out hit, 3))
        {
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * hit.distance, Color.yellow);

            if (!hasTriggered && hit.collider.gameObject == keyItem.gameObject)
            {
                hasTriggered = true;
                //Debug.Log("Correct object!");
                keyItem.Highlight(highlightColor);
                if (callbackAction != null)
                {
                    callbackAction.Callback();
                } else
                {
                    Debug.Log("No callback specified...");
                }
            }
        }
        else
        {
            hasTriggered = false;
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * 5, Color.white);
        }
    }
}
