using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{

    [SerializeField]
    private PlayerController playerController;

    [SerializeField]
    private bool isGrounded;
    [SerializeField]
    private float previousYVelo;
    [SerializeField]
    private Vector3 position;
    [SerializeField]
    private Quaternion rotation;
    [SerializeField]
    private Vector3 upDirection;
    [SerializeField]
    private Vector3 intendedUpDirection;

    void Awake()
    {
        position = transform.position;
        rotation = transform.rotation;
        upDirection = transform.up;

        isGrounded = CheckGrounded();
    }

    public void UpdateState()
    {

        // This will now set the transform.position and transform.rotation based on the 'previous' values
        //transform.position = previousPosition;
        transform.SetPositionAndRotation(position, rotation);

        //position = transform.position;
        //rotation = transform.rotation;

        //upDirection = transform.up;

        isGrounded = CheckGrounded();

    }

    private bool CheckGrounded()
    {
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

}
