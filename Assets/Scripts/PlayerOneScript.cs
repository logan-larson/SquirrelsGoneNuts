using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOneScript : MonoBehaviour
{
    // Scripts
    public PlayerInput playerInputScript;

    // Public Variables
    public float movementSpeed = 5f;
    public float groundedHeight = 1.5f;
    public float sensitivity = 300f;

    [Header("TEMP")]
    public Vector3 fwd;
    public float rotateAngle;
    public float prevRotateAngle;
    public Quaternion newRotation;

    // Input Variables
    Vector2 keyboardInput, mouseInput;
    bool shiftHold, spaceToggle, isUnlocked;

    // State Variables
    [SerializeField]
    bool isGrounded;

    // Misc. Variables
    float lerpSmoother = 5f;
    Vector3 velocity;
    Vector3 lastVelocity;
    Vector3 lastPosition;
    Quaternion lastRotation;


    void Start() {
        velocity = new Vector3();
        lastPosition = transform.position;
        lastRotation = transform.rotation;

        rotateAngle = 180f;
    }

    void Update() {
        GetInputs();

        playerInputScript.OnUpdate();
    }

    void FixedUpdate() {

        // Send raycasts from front and back
        Vector3 front = transform.position + (transform.forward / 2);
        Vector3 back = transform.position + (-transform.forward / 2);

        Vector3 frontDirection = ((transform.position - transform.up) - front).normalized;
        Vector3 backDirection = ((transform.position - transform.up) - back).normalized;

        RaycastCone frontCone = new RaycastCone(front, frontDirection, transform.forward, 0.3f, 8, groundedHeight, groundedHeight);
        RaycastCone backCone = new RaycastCone(back, backDirection, transform.forward, 0.3f, 8, groundedHeight, groundedHeight);


        Vector3 newPosition = transform.position;
        Vector3 newUp = transform.up;

        // Set up direction to match surface normal
        // Set height above surface
        // ORIENTATION
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit)) {
            newUp = hit.normal;
        }

        newRotation = transform.rotation;

        fwd = Vector3.Cross(newUp, transform.right);

        transform.rotation = Quaternion.LookRotation(fwd, newUp);

        // Rotate on mouse
        if (!isUnlocked) {
            rotateAngle = (mouseInput.x * sensitivity * Time.deltaTime) + 180; // it gets funky with 0

            transform.RotateAround(transform.position, newUp, rotateAngle);
        } else {
            transform.RotateAround(transform.position, newUp, 180f);
        }

        // HEIGHT ABOVE GROUND
        // Send raycast down to set height
        RaycastHit ground;
        if (Physics.Raycast(newPosition, -newUp, out ground)) {
            newPosition = ground.point + ground.normal;
        }


        // MOVEMENT
        // Will have to create vector to match surface angle
        // Extrapolate x and y from this vector to get normalized values
        Vector3 v = new Vector3(keyboardInput.x, 0f, keyboardInput.y).normalized;

        if (shiftHold) {
            v *= 2f;
        }

        if (keyboardInput.y != 0) {
            newPosition += transform.forward * v.z * movementSpeed * Time.fixedDeltaTime * 1.5f;
        }

        if (keyboardInput.x != 0) {
            newPosition += transform.right * v.x * movementSpeed * Time.fixedDeltaTime;
        }

        transform.position = newPosition;

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
        spaceToggle = Input.GetKeyDown(KeyCode.Space);
    }

    bool CheckIsGrounded() {
        RaycastCone cone = new RaycastCone(transform.position, -transform.up, transform.right, 0.3f, 6, groundedHeight, 2f);

        return cone.GetClosestDistance() < 1.5f;
    }

    void MatchGround() {

    }

    void MatchWorldGround() {

    }

    void AddGravity() {

    }

    void Move() {
    }

}
