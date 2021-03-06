using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerPressurePlate : PressurePlate
{
    public GameObject player;
    public Color highlightColor;

    private Color originalColor;

    private List<Collider> colliders = new List<Collider>();

    void Awake()
    {
        originalColor = this.GetComponent<Renderer>().material.color;
    }

    void OnTriggerEnter(Collider other)
    {
        if (player != null && other.gameObject != player)
        {
            return;
        }

        colliders.Add(other);

        if (colliders.Count > 0)
        {
            this.GetComponent<Renderer>().material.color = highlightColor;
            InvokePressed(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        colliders.Remove(other);
        if (colliders.Count == 0)
        {
            this.GetComponent<Renderer>().material.color = originalColor;
            InvokePressed(false);
        }
    }
}
