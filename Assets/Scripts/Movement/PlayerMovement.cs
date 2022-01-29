using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

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
        controller = gameObject.AddComponent<CharacterController>();
    }

    public void OnJump()
    {
        if (jumpsRemaining > 0)
        {
            jumping = true;
        }
    }

    public void OnMove(InputValue input)
    {
        Vector2 inputVec = input.Get<Vector2>();
        move = new Vector3(inputVec.x, 0, inputVec.y);
    }

    public void OnFire(InputValue value)
    {
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

    void Update()
    {
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
