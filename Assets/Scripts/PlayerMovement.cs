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
    //[SerializeField] private float jumpMultiplier = 10f;
    [SerializeField] private float gravity = 0.2f;

    private float yVelo = 0f;


    public void SetHeightAboveGround()
    {

        bool isGrounded = playerController.playerState.GetIsGrounded();

        if (isGrounded)
        {

            RaycastCone cone = playerController.playerState.GetHeightCone();

            /*
            int closestIndex = cone.GetClosestIndex();

            Vector3 closestPoint = cone.GetPoint(closestIndex);
            Vector3 closestNormal = cone.GetNormal(closestIndex);
            */

            RaycastHit hit;

            if (Physics.Raycast(playerController.playerState.GetPosition(), -playerController.playerState.GetUpDirection(), out hit)) {
                playerController.playerState.SetPosition(hit.point + hit.normal);
            }

            //Vector3 position = cone.GetAveragePoint() + cone.GetAverageNormal();

            //playerController.playerState.SetPosition(position);

        }
        else
        {

        }
    }

    public void MoveOnInputs(float horizontalInput, float verticalInput)
    {

        //if (playerController.playerState.GetIsGrounded())
        //{
            Vector3 v = new Vector3(horizontalInput, 0f, verticalInput).normalized;

            v *= movementSpeed * Time.deltaTime;

        //}
    }

    // Create a Vector3 and keep adding to it based on conditions
    public void Move(float horizontalInput, float verticalInput, bool isSprinting, bool isJumping, float previousYVelo, Vector3 previousPosition)
    {

        Vector3 v = new Vector3();
        //v += new Vector3(horizontalInput, 0f, verticalInput).normalized * movementSpeed;

        // Check grounded
        bool isGrounded = playerController.playerState.GetIsGrounded();

        if (isGrounded)
        {

            // Move faster when moving forward
            v = verticalInput > 0f ? v * forwardMuliplier : v;

            // Add sprint multiplier when applicable
            v = isSprinting ? v * sprintMulitplier : v;

            //yVelo = isJumping ? previousYVelo + jumpMultiplier : 0f;

            playerController.playerState.SetPreviousYVelo(yVelo);

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
