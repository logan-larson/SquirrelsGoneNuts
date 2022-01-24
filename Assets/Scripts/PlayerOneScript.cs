using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOneScript : MonoBehaviour
{
    // Scripts
    public PlayerInput playerInputScript;
    public FollowPlayer followPlayerScript;

    // Public Variables
    public float movementSpeed = 5f;
    public float sprintMultiplier = 2f;
    public float groundedHeight = 1.25f;
    public float sensitivity = 300f;

    [Header("TEMP")]
    public Vector3 fwd;
    public float rotateAngle;
    public float prevRotateAngle;

    // Input Variables
    Vector2 keyboardInput, mouseInput;
    public bool shiftHold, spaceToggle, isUnlocked, prevIsUnlocked;

    // State Variables
    [SerializeField]
    bool isGrounded;

    // Misc. Variables
    float lerpSmoother = 5f;
    Vector3 velocity;
    Vector3 lastVelocity;
    Vector3 prevPosition;
    Quaternion lastRotation;

    [Header("Movement Vectors")]
    public Vector3 movementVectorX;
    public Vector3 movementVectorY;
    public Vector3 movementVectorZ;
    public Vector3 momentum;
    public float accelerationX = 1f, accelerationY = 1f, accelerationZ = 1f;
    public float maxSpeedX = 0.25f, maxSpeedY, maxSpeedZ = 0.5f;
    public float friction;


    void Start() {
        velocity = new Vector3();
        prevPosition = transform.position;
        lastRotation = transform.rotation;

        isUnlocked = false;
        prevIsUnlocked = false;

        rotateAngle = 180f;
    }

    void FixedUpdate() {

        playerInputScript.OnUpdate();

        GetInputs();

        // Used for movement
        Vector3 newPosition = transform.position;
        // Used for orientation
        Vector3 newUp = transform.up;

        // ORIENTATION
        RaycastHit hit;
        if (Physics.Raycast(newPosition, -newUp, out hit, groundedHeight)) {
            // Set up direction to match surface normal
            newUp = hit.normal;
        } else {
            // Calculate momentum and set orientation
            Vector3 currPosition = Move(newPosition);

            momentum = (currPosition - prevPosition).normalized;

            if (Physics.Raycast(newPosition, momentum, out hit, 5f)) {
                newUp = hit.normal;
            }

            Debug.DrawLine(newPosition, newPosition + (momentum * 5f), Color.red);

        }

        fwd = Vector3.Cross(newUp, transform.right);

        transform.rotation = Quaternion.LookRotation(fwd, newUp);

        // Rotate on mouse
        if (!isUnlocked) {
            rotateAngle = (mouseInput.x * sensitivity * Time.deltaTime) + 180f; // it gets funky with 0f

            transform.RotateAround(transform.position, newUp, rotateAngle);
        } else {
            transform.RotateAround(transform.position, newUp, 180f);
        }

        // HEIGHT ABOVE GROUND
        // Send raycast down to set height
        RaycastHit ground;
        if (Physics.Raycast(newPosition, -newUp, out ground, groundedHeight)) {
            // Set height above surface
            newPosition = ground.point + ground.normal;
        }

        SetIsGrounded(newPosition, newUp);

        // MOVEMENT
        newPosition = Move(newPosition);

        SetPreviousValues(newPosition);

        Debug.DrawLine(newPosition, newPosition + newUp * 2f, Color.green);
    }

    // Input
    void GetInputs() {
        keyboardInput.x = Input.GetAxis("Horizontal");
        keyboardInput.y = Input.GetAxis("Vertical");

        mouseInput.x = Input.GetAxisRaw("Mouse X");
        mouseInput.y = Input.GetAxisRaw("Mouse Y");

        // Camera lock input
        isUnlocked = Input.GetMouseButton(2);

        shiftHold = Input.GetKey(KeyCode.LeftShift);
        spaceToggle = Input.GetKey(KeyCode.Space);
    }

    void SetPreviousValues(Vector3 position) {
        prevIsUnlocked = isUnlocked;
        prevPosition = position;
    }

    void SetIsGrounded(Vector3 position, Vector3 up) {
        RaycastHit hit;
        if (Physics.Raycast(position, -up, out hit)) {
            isGrounded = hit.distance < groundedHeight ? true : false;
        }
    }

    Vector3 Move(Vector3 position) {

        bool isJumping = false;

        // Gravity
        if (isGrounded) {
            // Jumping
            if (spaceToggle) {
                float yMultiplier = Mathf.Clamp(followPlayerScript.cameraDirection.y, 0.25f, 1f);
                movementVectorY += transform.up * yMultiplier;
                movementVectorZ += transform.forward * (1f - yMultiplier);
                isJumping = true;
            } else {
                movementVectorY = new Vector3();
            }
        } else {
            movementVectorY += Vector3.down * accelerationY;

            movementVectorY = Vector3.ClampMagnitude(movementVectorY, maxSpeedY);
        }


        float sprintMax = shiftHold ? sprintMultiplier : 1f;

        // Forward/Backward movement
        if (keyboardInput.y != 0) {
            movementVectorZ += transform.forward * accelerationZ * keyboardInput.y;

            movementVectorZ = Vector3.ClampMagnitude(movementVectorZ, maxSpeedZ * sprintMax);
        } else if (!isJumping) {
            movementVectorZ *= friction;
        }

        // Left/Right movement
        if (keyboardInput.x != 0) {
            movementVectorX += transform.right * accelerationX * keyboardInput.x;

            movementVectorX = Vector3.ClampMagnitude(movementVectorX, maxSpeedX);
        } else if (!isJumping) {
            movementVectorX *= friction;
        }


        position += movementVectorX + movementVectorY + movementVectorZ;
        
        transform.position = position;

        return position;
    }

}
