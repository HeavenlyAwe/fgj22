using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
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

    public int factionSizeLimit = 1;


    void Awake()
    {
        originalColor = gameObject.GetComponent<Renderer>().material.color;
    }

    public void Highlight(Color highlightColor)
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


    public virtual bool Interact(Vector3 moveOffset, int factionSize)
    {
        return factionSize >= factionSizeLimit;
    }


    public virtual bool Interact(Transform interactor, int factionSize)
    {
        return factionSize >= factionSizeLimit;
    }


    public abstract bool IsGrabbable();
}
