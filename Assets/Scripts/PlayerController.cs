using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Scripts and linked objects
    [Header("Scripts & Linked Objects")]
    public PlayerInput playerInputScript;
    public FollowPlayer followPlayerScript;

    // Public Variables
    [Header("Player Adjustable Variables")]
    public float rotateSensitivity = 300f; // Coincides with player's horizontal look sensitivity

    [Header("Gameplay Adjustable Variables")]
    public float sprintMultiplier = 2f;
    public float groundedHeight = 1.25f;
    public float accelerationX = 1f, accelerationY = 1f, accelerationZ = 1f;
    public float maxSpeedX = 0.25f, maxSpeedY = 0.5f, maxSpeedZ = 0.5f;
    public float friction = 0f;

    // Misc. Variables
    Vector3 prevPosition;

    // Vectors used to adjust player position
    Vector3 movementVectorX;
    Vector3 movementVectorY;
    Vector3 movementVectorZ;


    void Start()
    {
        prevPosition = transform.position;
    }

    void FixedUpdate()
    {
        // Get new inputs
        playerInputScript.OnUpdate();

        // Get new up and forward orientations
        Vector3 newUp = GetNewUp(transform.position, transform.up);
        Vector3 newForward = Vector3.Cross(newUp, transform.right);

        // Set player rotation based on new values
        transform.rotation = Quaternion.LookRotation(newForward, newUp);

        // Rotate player on mouse inputs
        RotateOnInput(newUp);

        // Set position to be modified based on height above ground
        Vector3 newPosition = GetPositionAboveGround(newUp);

        // -------------
        // TEMP Reset player position, used for debugging
        if (playerInputScript.GetPressR())
        {
            newPosition = new Vector3(0f, 5f, 0f);
        }
        // -------------

        // Move player based on keyboard inputs
        newPosition = Move(newPosition, newUp);

        // Set previous position
        prevPosition = newPosition;
    }

    Vector3 GetNewUp(Vector3 position, Vector3 up)
    {
        RaycastHit hit;
        if (Physics.Raycast(position, -up, out hit, groundedHeight))
        {
            // Set up direction to match surface normal
            return hit.normal;
        }
        else
        {
            // Calculate momentum and set orientation
            Vector3 currPosition = Move(position, up);

            Vector3 momentum = (currPosition - prevPosition).normalized;

            if (Physics.Raycast(position, momentum, out hit, 5f))
            {
                return hit.normal;
            }

            return up;
        }

    }

    void RotateOnInput(Vector3 up)
    {
        if (!playerInputScript.GetMiddleMouseHold())
        {
            float rotateAngle = (playerInputScript.GetMouseInput().x * rotateSensitivity * Time.deltaTime) + 180f; // it gets funky with 0f

            transform.RotateAround(transform.position, up, rotateAngle);
        }
        else
        {
            transform.RotateAround(transform.position, up, 180f);
        }
    }

    Vector3 GetPositionAboveGround(Vector3 up)
    {
        // Send raycast down to set height
        RaycastHit ground;
        if (Physics.Raycast(transform.position, -up, out ground, groundedHeight))
        {
            // Set height above surface
            return ground.point + ground.normal;
        }

        return transform.position;
    }

    bool GetIsGrounded(Vector3 position, Vector3 up)
    {
        RaycastHit hit;
        if (Physics.Raycast(position, -up, out hit, groundedHeight))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    Vector3 Move(Vector3 position, Vector3 up)
    {

        bool isJumping = false;

        // Gravity
        if (GetIsGrounded(position, up))
        {
            // Jumping
            if (playerInputScript.GetSpaceToggle())
            {
                if (playerInputScript.GetMiddleMouseHold())
                {
                    // Unlocked collisions are shaky
                    movementVectorX += followPlayerScript.cameraDirection;
                    movementVectorY += followPlayerScript.cameraDirection;
                    movementVectorZ += followPlayerScript.cameraDirection;
                    isJumping = true;
                }
                else
                {
                    // Locked mode jumping needs some work
                    float yMultiplier = Mathf.Clamp(followPlayerScript.cameraDirection.y, 0.25f, 1f);
                    movementVectorY += transform.up * yMultiplier;
                    movementVectorZ += transform.forward;
                    isJumping = true;
                }
            }
            else
            {
                movementVectorY = new Vector3();
            }
        }
        else
        {
            movementVectorY += Vector3.down * accelerationY;

            movementVectorY = Vector3.ClampMagnitude(movementVectorY, maxSpeedY);
        }


        float sprintMax = playerInputScript.GetShiftHold() ? sprintMultiplier : 1f;

        // Forward/Backward movement
        if (playerInputScript.GetKeyboardInput().y != 0)
        {
            movementVectorZ += transform.forward * accelerationZ * playerInputScript.GetKeyboardInput().y;

            movementVectorZ = Vector3.ClampMagnitude(movementVectorZ, maxSpeedZ * sprintMax);
        }
        else if (!isJumping)
        {
            movementVectorZ *= friction;
        }

        // Left/Right movement
        if (playerInputScript.GetKeyboardInput().x != 0)
        {
            movementVectorX += transform.right * accelerationX * playerInputScript.GetKeyboardInput().x;

            movementVectorX = Vector3.ClampMagnitude(movementVectorX, maxSpeedX);
        }
        else if (!isJumping)
        {
            movementVectorX *= friction;
        }


        position += movementVectorX + movementVectorY + movementVectorZ;

        transform.position = position;

        return position;
    }
}
