using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPressurePlate : MonoBehaviour
{
    //public GameObject player;
    public Color highlightColor;

    public CallbackAction callbackAction;

    private Color originalColor;
    //private bool hasTriggered;

    private List<Collider> colliders = new List<Collider>();

    void Awake()
    {
        originalColor = this.GetComponent<Renderer>().material.color;
    }


    //void FixedUpdate()
    //{
    //    RaycastHit hit;
    //    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out hit, 3))
    //    {
    //        //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * hit.distance, Color.yellow);
    //        if (player == null)
    //        {
    //            Debug.Log("Anything can trigger");
    //            hasTriggered = true;
    //        }
    //        else
    //        {
    //            if (!hasTriggered && hit.collider.gameObject == player)
    //            {
    //                hasTriggered = true;

    //                if (callbackAction != null)
    //                {
    //                    callbackAction.Callback();
    //                }
    //                else
    //                {
    //                    Debug.Log("No callback specified...");
    //                }
    //            }
    //        }
    //    }
    //    else
    //    {
    //        hasTriggered = false;
    //        // Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * 5, Color.white);
    //    }

    //    if (hasTriggered)
    //    {
    //        this.GetComponent<Renderer>().material.color = highlightColor;
    //    }
    //    else
    //    {
    //        this.GetComponent<Renderer>().material.color = originalColor;
    //    }
    //}

    void OnTriggerEnter(Collider other)
    {
        colliders.Add(other);
        if (colliders.Count > 0)
        {
            this.GetComponent<Renderer>().material.color = highlightColor;
        }
    }

    void OnTriggerExit(Collider other)
    {
        colliders.Remove(other);
        if (colliders.Count == 0)
        {
            this.GetComponent<Renderer>().material.color = originalColor;
        }
    }
}
