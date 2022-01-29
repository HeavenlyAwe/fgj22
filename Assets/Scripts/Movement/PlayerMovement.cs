using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    private Interactable currentlySelectedInteractable = null;
    private bool interactableGrabbed = false;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    public int visionRange = 8;
    public float playerSpeed = 2.0f;
    public float jumpHeight = 1.0f;
    public float gravityValue = -9.81f;

    // This allows the player to perform multiple jumps if necessary
    public int maxNumberOfJumps = 1;

    public float influenceRange = 5;

    private int jumpsRemaining = 0;
    private bool jumping = false;
    private Vector3 move = Vector3.zero;

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
    }

    public void OnJump()
    {
        if (interactableGrabbed) return;

        if (jumpsRemaining > 0)
        {
            jumping = true;
        }
    }

    public void OnMove(InputValue input)
    {
        Vector2 inputVec = input.Get<Vector2>();

        if (interactableGrabbed)
        {
            Debug.Log(inputVec);
            currentlySelectedInteractable.Interact(new Vector3(inputVec.x, 0, inputVec.y));
            return;
        }

        move = new Vector3(inputVec.x, 0, inputVec.y);
    }

    public void OnFire(InputValue value)
    {
        Debug.Log("Doing stuff...");

        if (currentlySelectedInteractable != null)
        {
            if (currentlySelectedInteractable.IsGrabbable() && !interactableGrabbed)
            {
                currentlySelectedInteractable.Grab();
                interactableGrabbed = true;
            }
            else if (currentlySelectedInteractable.IsGrabbable() && interactableGrabbed)
            {
                currentlySelectedInteractable.Release();
                interactableGrabbed = false;
            }
            else
            {
                currentlySelectedInteractable.Interact(transform);
            }
        }

        Vector3 center = gameObject.transform.position;
        Collider[] hitColliders = Physics.OverlapSphere(center, influenceRange);
        foreach (var hitCollider in hitColliders)
        {
            AIController aic = hitCollider.gameObject.GetComponent<AIController>();
            if (aic != null)
            {
                if (gameObject.CompareTag("Fire"))
                    aic.ChangeFaction(AIController.Faction.Fire);
                else if (gameObject.CompareTag("Water"))
                    aic.ChangeFaction(AIController.Faction.Water);
            }
        }
    }

    private void UpdatePlayerInteractions()
    {
        //Debug.Log(interactables.Count);
        if (interactableGrabbed)
        {
            return;
        }

        RaycastHit hit;

        Vector3 origin = transform.position + controller.center;
        //float distanceToObstacle = 0;

        //Debug.DrawRay(origin, transform.forward, Color.blue, 2f, true);

        if (currentlySelectedInteractable != null)
        {
            currentlySelectedInteractable.Unhighlight();
        }
        currentlySelectedInteractable = null;

        LayerMask mask = LayerMask.GetMask("Interactables");
        if (Physics.SphereCast(origin, controller.height / 2, transform.forward, out hit, visionRange, mask))
        {
            //distanceToObstacle = hit.distance;
            currentlySelectedInteractable = hit.transform.gameObject.GetComponent<Interactable>();
            currentlySelectedInteractable.Highlight();
        }
    }

    void Update()
    {
        UpdatePlayerInteractions();

        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            jumpsRemaining = maxNumberOfJumps;
            playerVelocity.y = 0f;
        }

        //Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        // Changes the height position of the player..
        if (jumping)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            jumping = false;
            jumpsRemaining -= 1;
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}
