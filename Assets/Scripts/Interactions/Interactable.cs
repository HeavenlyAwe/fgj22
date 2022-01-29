using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public Color highlightColor = Color.yellow;
    private Color originalColor;

    private bool reserved;
    public bool Reserved
    {
        private set
        {
            reserved = value;
        }
        get
        {
            return reserved;
        }
    }

    void Awake()
    {
        originalColor = gameObject.GetComponent<Renderer>().material.color;
    }

    public void Highlight()
    {
        gameObject.GetComponent<Renderer>().material.color = highlightColor;
    }

    public void Unhighlight()
    {
        gameObject.GetComponent<Renderer>().material.color = originalColor;
    }

    public void Grab()
    {
        Reserved = true;
    }

    public void Release()
    {
        Reserved = false;
    }

    public virtual void Interact(Vector3 moveOffset)
    {
        Debug.Log("'Interact(Vector3 moveOffset)' Not yet implemented...");
    }

    public virtual void Interact(Transform interactor)
    {
        Debug.Log("'Interact(Transform interactor)' Not yet implemented...");
    }

    public abstract bool IsGrabbable();
}
