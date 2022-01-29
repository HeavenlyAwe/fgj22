using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public Color highlightColor = Color.yellow;
    private Color originalColor;

    protected void Start()
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

    public virtual void Interact(Transform interactor)
    {
        Debug.Log("Not yet implemented");
    }
}
