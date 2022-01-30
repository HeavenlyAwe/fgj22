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
    public Color highlightColor;

    // This allows the player to perform multiple jumps if necessary
    public int maxNumberOfJumps = 1;

    public float influenceRange = 5;

    public AudioClip footStepSound;
    public AudioClip waterFollowerSound;
    public AudioClip fireFollowerSound;
    public AudioSource audioSource;

    private int jumpsRemaining = 0;
    private bool jumping = false;
    private Vector3 move = Vector3.zero;
    private Animator _animator;

    private AIController.Faction faction;

    private void Start()
    {
        if (gameObject.CompareTag("Fire"))
        {
            faction = AIController.Faction.Fire;
        }
        else if (gameObject.CompareTag("Water"))
        {
            faction = AIController.Faction.Water;
        }
        controller = gameObject.GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
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
            currentlySelectedInteractable.Interact(new Vector3(inputVec.x, 0, inputVec.y), FactionSizeCounter.Value(faction));

            // Do not move if grabbing anything
            move = Vector3.zero;

            return;
        }

        move = new Vector3(inputVec.x, 0, inputVec.y);
    }

    public void OnFire(InputValue value)
    {
        //
        // Perform actions/interaction
        //
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
                currentlySelectedInteractable.Interact(transform, FactionSizeCounter.Value(faction));
            }
        }

        //
        // Attract followers
        //
        Vector3 center = gameObject.transform.position;
        Collider[] hitColliders = Physics.OverlapSphere(center, influenceRange);
        foreach (var hitCollider in hitColliders)
        {
            AIController aic = hitCollider.gameObject.GetComponent<AIController>();
            if (aic != null)
            {
                if (gameObject.CompareTag("Fire") && aic.faction != AIController.Faction.Fire)
                {
                    aic.ChangeFaction(AIController.Faction.Fire);
                    audioSource.PlayOneShot(fireFollowerSound);
                }
                else if (gameObject.CompareTag("Water") && aic.faction != AIController.Faction.Water)
                {
                    aic.ChangeFaction(AIController.Faction.Water);
                    audioSource.PlayOneShot(waterFollowerSound);
                }
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
            currentlySelectedInteractable.Highlight(highlightColor);
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

        // Update animator to animate running based on speed!
        _animator.SetFloat("Speed", move.magnitude);

        if (move != Vector3.zero)
        {
            //gameObject.transform.forward = move;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(move), Time.deltaTime * 10.0f);
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

    public void FootStep()
    {
        audioSource.PlayOneShot(footStepSound);
    }
}
