using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public PlayerInput playerInputScript;

    public Transform playerTransform;
    public float sensitivityX = 100f;
    public float sensitivityY = 100f;
    public float cameraHeight = 1.5f;
    public float minCameraHeight = -0.5f;
    public float maxCameraHeight = 3.5f;
    public float cameraDistance = 5f;

    [Header("Lerp Values")]
    public float rotationLerp = 0.5f;
    public float positionLerp = 0.5f;
    public float clippingLerp = 0.8f;

    [Header("Camera Stats")]
    public Vector2 cameraRotationChange;
    public Vector3 cameraDirection;
    public bool isUnlocked = false, prevIsUnlocked;
    public float startingX;

    private Vector3 currentCameraHeight;
    private Vector3 currentCameraDistance;

    private Quaternion prevCameraRotation;
    private Vector3 prevCameraPosition;
    private Quaternion prevPlayerRotation;
    private Vector3 prevPlayerPosition;

    void Start() {
        // Hide cursor
        Cursor.lockState = CursorLockMode.Locked;

        // Initialize values
        SetPreviousValues();
    }

    // -- Set previous values to be used in next physics step for lerp
    void SetPreviousValues() {
        prevIsUnlocked = isUnlocked;
        prevCameraRotation = transform.localRotation;
        prevCameraPosition = transform.position;
        prevPlayerRotation = playerTransform.rotation;
        prevPlayerPosition = playerTransform.position;
    }

    void FixedUpdate() {

        GetInputs();

        ConstrainViewAngles();

        AdjustCameraRotation();

        AdjustCameraPosition();

        AdjustCameraPositionForClipping();

        SetPreviousValues();

    }

    void GetInputs() {
        // Camera lock input
        isUnlocked = playerInputScript.GetMiddleMouseHold();

        // Mouse inputs
        cameraRotationChange.x += playerInputScript.GetMouseInput().x * sensitivityX * Time.fixedDeltaTime;
        cameraRotationChange.y += playerInputScript.GetMouseInput().y * sensitivityY * Time.fixedDeltaTime * 3;
    }

    // -- Restrict mouse inputs to certain range
    void ConstrainViewAngles() {
        if (!isUnlocked) {
            // Restrict Y axis viewing angle
            cameraRotationChange.y = Mathf.Clamp(cameraRotationChange.y, -30f, 80f);
        }
    }


    void AdjustCameraRotation() {

        if (isUnlocked) {
            if (!prevIsUnlocked) {
                startingX = cameraRotationChange.x;
            }
            // If unlocked, adjust camera rotation based on mouse input
            cameraDirection = ((playerTransform.position + playerTransform.up) - transform.position).normalized;
            transform.localRotation = Quaternion.Lerp(prevCameraRotation, Quaternion.Euler(-cameraRotationChange.y, cameraRotationChange.x - startingX, 0f), rotationLerp);
        } else {
            // If locked, look towards player and adjust player rotation based on camera
            // -- Get direction of player relative to camera
            cameraDirection = ((playerTransform.position + playerTransform.up) - transform.position).normalized;
            // -- Change camera rotation
            transform.localRotation = Quaternion.Lerp(prevCameraRotation, Quaternion.Euler(-cameraRotationChange.y, cameraDirection.x, 0f), rotationLerp);
        }
    }

    // -- Adjust position relative to player and camera rotation
    void AdjustCameraPosition() {

        if (isUnlocked) {
            // Get camera height
            Vector3 camHeight = playerTransform.up * cameraHeight;
            // Get camera distance
            Vector3 camDistance = -transform.forward * cameraDistance;
            // Set position relative to player, height and distance
            transform.position = playerTransform.position + camDistance + camHeight;
        } else {
            // Clamp camera height
            float height = Mathf.Clamp((cameraHeight - (cameraRotationChange.y / 20f)), minCameraHeight, maxCameraHeight);
            // Get camera height
            currentCameraHeight = playerTransform.up * height;
            // Get camera distance
            currentCameraDistance = -playerTransform.forward * cameraDistance;


            // Lerp position relative to player, height and distance
            transform.position = Vector3.Lerp(prevCameraPosition, playerTransform.position + currentCameraHeight + currentCameraDistance, positionLerp);
        }
    }

    void AdjustCameraPositionForClipping() {
        Vector3 cameraRayDirection = (transform.position - playerTransform.position).normalized;
        Ray cameraRay = new Ray(playerTransform.position, cameraRayDirection);
        RaycastHit hitInfo;
        if (Physics.Raycast(cameraRay, out hitInfo, 5f)) {
            transform.position = Vector3.Lerp(transform.position, hitInfo.point - cameraRayDirection, clippingLerp);
        }
    }

}
