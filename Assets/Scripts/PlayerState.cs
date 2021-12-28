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
    private Vector3 previousPosition;
    [SerializeField]
    private Quaternion previousRotation;

    public void UpdateState()
    {
        isGrounded = CheckGrounded();
    }

    private bool CheckGrounded()
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

    public Vector3 GetPreviousPosition()
    {
        return previousPosition;
    }

    public void SetPreviousPosition(Vector3 prevPosition)
    {
        previousPosition = prevPosition;
    }

    public Quaternion GetPreviousRotation()
    {
        return previousRotation;
    }

    public void SetPreviousRotation(Quaternion prevRotation)
    {
        previousRotation = prevRotation;
    }

}
