using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    public Transform playerTransform;
    public float sensitivityX = 100f;
    public float sensitivityY = 100f;
    public float cameraHeight = 1.5f;
    public float minCameraHeight = -0.5f;
    public float maxCameraHeight = 3.5f;
    public float cameraDistance = 5f;

    private Vector2 mouseInput;
    private bool isUnlocked = false, prevIsUnlocked;

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
        prevIsUnlocked = false;
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

        // Previous partial solution
        /*

        RaycastHit cameraPosition;

        if (Physics.Raycast(playerTransform.position + playerTransform.up, -playerTransform.forward, out cameraPosition, 5f)) {
            transform.position = cameraPosition.point;
        } else {
            transform.position = playerTransform.position + playerTransform.up + (-playerTransform.forward * 5f);
        }

        */
    }

    // -- TEMP: Eventually put in input manager script
    void GetInputs() {
        // Camera lock input
        isUnlocked = Input.GetMouseButton(2);

        // Mouse inputs
        mouseInput.x += Input.GetAxis("Mouse X") * sensitivityX * Time.fixedDeltaTime;
        mouseInput.y += Input.GetAxis("Mouse Y") * sensitivityY * Time.fixedDeltaTime;
    }

    // -- Restrict mouse inputs to certain range
    void ConstrainViewAngles() {
        if (!isUnlocked) {
            // Restrict Y axis viewing angle
            mouseInput.y = Mathf.Clamp(mouseInput.y, -50f, 40f);
        }
    }


    void AdjustCameraRotation() {
        if (isUnlocked) {
            // If unlocked, adjust camera rotation based on mouse input
            transform.rotation = Quaternion.Euler(-mouseInput.y, mouseInput.x, 0)/*, 0.05f)*/;
        } else {
            // If locked, look towards player and adjust player rotation based on camera
            // -- Get direction of player relative to camera
            Vector3 cameraDirection = ((playerTransform.position + playerTransform.up) - transform.position).normalized;
            // -- Change camera rotation
            transform.localRotation = Quaternion.Lerp(prevCameraRotation, Quaternion.Euler(-mouseInput.y, cameraDirection.x, 0), 0.2f);
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
            float height = Mathf.Clamp((cameraHeight - (mouseInput.y / 20f)), minCameraHeight, maxCameraHeight);
            // Get camera height
            currentCameraHeight = playerTransform.up * height;
            // Get camera distance
            currentCameraDistance = -playerTransform.forward * cameraDistance;


            // Lerp position relative to player, height and distance
            transform.position = Vector3.Lerp(prevCameraPosition, playerTransform.position + currentCameraHeight + currentCameraDistance, 0.5f);
            //transform.position = playerTransform.position + currentCameraHeight + currentCameraDistance;
        }
    }

    void AdjustCameraPositionForClipping() {
        Ray cameraRay = new Ray(playerTransform.position, (transform.position - playerTransform.position).normalized);
        RaycastHit hitInfo;
        if (Physics.Raycast(cameraRay, out hitInfo, 5f)) {
            Debug.DrawRay(cameraRay.origin, cameraRay.direction, Color.red);
            transform.position = Vector3.Lerp(transform.position, hitInfo.point, 0.8f);
        }
    }

    void OnDrawGizmos() {
        Gizmos.DrawSphere(playerTransform.position + currentCameraHeight + currentCameraDistance, 0.5f);
    }

}
