using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FollowerPressurePlate : MonoBehaviour
{
    public UnityEvent<bool> pressed = new UnityEvent<bool>();

    public AIController.Faction faction;

    public Color highlightColor;
    private Color originalColor;

    public CallbackAction callbackAction;

    void Awake()
    {
        originalColor = this.GetComponent<Renderer>().material.color;
    }

    void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, 2.0f, transform.TransformDirection(Vector3.up), out hit, 2))
        // if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out hit, 3))
        {
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * hit.distance, Color.yellow);
            
            AIController aic = hit.collider.gameObject.GetComponent<AIController>();

            if (aic != null && aic.faction == faction)
            {
                gameObject.GetComponent<Renderer>().material.color = highlightColor;
                this.enabled = false;

                aic.StayBehind(transform.position);

                if (callbackAction != null)
                {
                    callbackAction.Callback();
                }
                else
                {
                    Debug.Log("No callback specified...");
                }
            }
        }
    }
}
