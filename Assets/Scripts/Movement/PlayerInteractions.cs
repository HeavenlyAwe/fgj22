using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerInteractions : MonoBehaviour
{
    public int visionRange = 8;
    private CharacterController characterController;

    private Interactable currentlySelectedInteractable = null;

    // Start is called before the first frame update
    void Start()
    {
        characterController = gameObject.GetComponent<CharacterController>();
    }

    public void OnFire(InputValue inputValue)
    {
        if (currentlySelectedInteractable != null)
        {
            currentlySelectedInteractable.Interact();
        }
    }

    Color originalColor;

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(interactables.Count);

        RaycastHit hit;

        Vector3 origin = transform.position + characterController.center;
        //float distanceToObstacle = 0;

        //Debug.DrawRay(origin, transform.forward, Color.blue, 2f, true);

        if (currentlySelectedInteractable != null)
        {
            currentlySelectedInteractable.gameObject.GetComponent<Renderer>().material.color = originalColor;
        }
        currentlySelectedInteractable = null;

        LayerMask mask = LayerMask.GetMask("Interactables");
        if (Physics.SphereCast(origin, characterController.height / 2, transform.forward, out hit, visionRange, mask))
        {
            //distanceToObstacle = hit.distance;
            //Debug.Log(hit);
            currentlySelectedInteractable = hit.transform.gameObject.GetComponent<Interactable>();

            originalColor = currentlySelectedInteractable.gameObject.GetComponent<Renderer>().material.color;
            currentlySelectedInteractable.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
        }
    }

    //void OnDrawGizmos()
    //{
    //    for (int i = 0; i < visionRange; i++)
    //    {
    //        Gizmos.DrawSphere(transform.position + i * transform.forward, characterController.height / 2);
    //    }
    //}
}
