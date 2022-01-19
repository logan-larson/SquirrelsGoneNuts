using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{

    public float radius = 0.3f;
    public int numCasts = 8;
    public float drawDistance = 5f;

    [SerializeField]
    private PlayerController playerController;

    [SerializeField]
    private float groundedDistance = 2f;

    [SerializeField]
    private bool isGrounded;

    private float previousYVelo;
    [SerializeField]
    private Vector3 position;
    private Quaternion rotation;
    [SerializeField]
    private Vector3 upDirection;
    private Vector3 rightDirection;
    private Vector3 intendedUpDirection;

    // Cone shot directly underneath the player to get height and normal of surface below
    private RaycastCone heightCone;

    void Awake()
    {
        position = transform.position;
        rotation = transform.rotation;
        upDirection = transform.up;
        rightDirection = transform.right;

        heightCone = new RaycastCone(position, -upDirection, rightDirection, radius, numCasts, 2f, drawDistance);

        isGrounded = CheckGrounded();
    }

    public void UpdateState()
    {

        // This will now set the transform.position and transform.rotation based on the 'previous' values

        transform.SetPositionAndRotation(position, rotation);

        heightCone = new RaycastCone(position, -upDirection, rightDirection, radius, numCasts, 2f, drawDistance);

        isGrounded = CheckGrounded();

    }

    private bool CheckGrounded()
    {

        return true;
        //return position - .GetAveragePoint < groundedDistance;

        /*

        RaycastHit ground;

        // Send raycast downward locally
        // Check if player is close enough to ground to be considered grounded
        if (Physics.Raycast(position, -upDirection, out ground))
        {
            if (ground.distance < 2f)
            {
                Debug.DrawRay(position, -upDirection * ground.distance, Color.green, Time.deltaTime);
                return true;
            }
            else
            {
                Debug.DrawRay(position, -upDirection * ground.distance, Color.red, Time.deltaTime);
                return false;
            }
        }
        else
        {
            Debug.DrawRay(position, -upDirection * 10f, Color.red, Time.deltaTime);
        }

        return false;

        */
    }

    public bool GetIsGrounded()
    {
        return isGrounded;
    }

    public float GetPreviousYVelo()
    {
        return previousYVelo;
    }

    public void SetPreviousYVelo(float prevYVelo)
    {
        previousYVelo = prevYVelo;
    }

    public Vector3 GetPosition()
    {
        return position;
    }

    public void SetPosition(Vector3 p)
    {
        position = p;
    }

    public void AddVectorToPosition(Vector3 v)
    {
        position += v;
    }

    public Quaternion GetRotation()
    {
        return rotation;
    }

    public void SetRotation(Quaternion r)
    {
        rotation = r;
    }

    public Vector3 GetUpDirection()
    {
        return upDirection;
    }

    public void SetUpDirection(Vector3 up)
    {
        upDirection = up;
    }

    public Vector3 GetIntendedUpDirection()
    {
        return intendedUpDirection;
    }

    public void SetIntendedUpDirection(Vector3 up)
    {
        intendedUpDirection = up;
    }

    public RaycastCone GetHeightCone() {
        return this.heightCone;
    }

}
