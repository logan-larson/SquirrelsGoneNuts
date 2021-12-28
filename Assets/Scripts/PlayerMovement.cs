using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField]
    PlayerController playerController;

    [SerializeField] private float movementSpeed = 3f;
    [SerializeField] private float forwardMuliplier = 2f;
    [SerializeField] private float sprintMulitplier = 1.5f;
    [SerializeField] private float jumpMultiplier = 10f;
    [SerializeField] private float gravity = 0.2f;
    [SerializeField] private bool isGrounded;

    private float yVelo = 0f;

    // Create a Vector3 and keep adding to it based on conditions
    public void Move(float horizontalInput, float verticalInput, bool isSprinting, bool isJumping, float previousYVelo, Vector3 previousPosition)
    {

        Vector3 v = new Vector3(horizontalInput, 0f, verticalInput).normalized * movementSpeed;

        // Check grounded
        isGrounded = playerController.playerState.GetIsGrounded();

        if (isGrounded)
        {
            // Move faster when moving forward
            v = verticalInput > 0f ? v * forwardMuliplier : v;

            // Add sprint multiplier when applicable
            v = isSprinting ? v * sprintMulitplier : v;

            yVelo = isJumping ? previousYVelo + jumpMultiplier : 0f;

            playerController.playerState.SetPreviousYVelo(yVelo);

            if (yVelo == 0)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, -transform.up, out hit))
                {
                    if (hit.distance < 1.4f)
                    {
                        yVelo = 10f;
                    }
                }
            }

            v = v + new Vector3(0f, yVelo, 0f);

            transform.Translate(v * Time.deltaTime, Space.Self);
        }
        else
        {
            yVelo = previousYVelo - gravity;

            playerController.playerState.SetPreviousYVelo(yVelo);

            // Move downward when not grounded
            v = v - Vector3.down * yVelo;

            transform.Translate(v * Time.deltaTime, Space.World);
        }

    }

}
