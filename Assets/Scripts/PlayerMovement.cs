using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField]
    private PlayerController playerController;

    [SerializeField] private float movementSpeed = 3f;
    [SerializeField] private float forwardMuliplier = 2f;
    [SerializeField] private float sprintMulitplier = 1.5f;
    [SerializeField] private float jumpMultiplier = 10f;
    [SerializeField] private float gravity = 0.2f;
    [SerializeField] private bool isGrounded;

    private float yVelo = 0f;

    // Create a Vector3 and keep adding to it based on conditions
    public void Move(float horizontalInput, float verticalInput, bool isSprinting, bool isJumping, float previousYVelo)
    {

        Vector3 v = new Vector3(horizontalInput, 0f, verticalInput).normalized * movementSpeed;

        // Check grounded
        isGrounded = checkGrounded();

        if (isGrounded)
        {
            // Move faster when moving forward
            v = verticalInput > 0f ? v * forwardMuliplier : v;

            // Add sprint multiplier when applicable
            v = isSprinting ? v * sprintMulitplier : v;

            yVelo = isJumping ? previousYVelo + jumpMultiplier : 0f;

            v = v + new Vector3(0f, yVelo, 0f);

            transform.Translate(v * Time.deltaTime, Space.Self);
        }
        else
        {
            yVelo = previousYVelo - gravity;

            // Move downward when not grounded
            v = v - Vector3.down * yVelo;

            transform.Translate(v * Time.deltaTime, Space.World);
        }


    }

    private bool checkGrounded()
    {
        RaycastHit ground = new RaycastHit();

        // Send raycast downward locally
        // Check if player is close enough to ground to be considered grounded
        if (Physics.Raycast(transform.position, -transform.up, out ground))
        {
            Debug.DrawRay(transform.position, -transform.up, Color.green, Time.deltaTime);
            return ground.distance < 1.5f;
        }
        else
        {
            Debug.DrawRay(transform.position, -transform.up * 10f, Color.red, Time.deltaTime);
        }

        return false;
    }

    public float getYVelo()
    {
        return yVelo;
    }

}
