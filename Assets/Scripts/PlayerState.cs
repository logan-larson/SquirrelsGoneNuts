using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{

    [SerializeField]
    private PlayerController playerController;

    private bool isGrounded;
    private float previousYVelo;

    public void UpdateState()
    {
        isGrounded = checkGrounded();
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

    public bool getIsGrounded()
    {
        return isGrounded;
    }

    public float getPreviousYVelo()
    {
        return previousYVelo;
    }

    public void setPreviousYVelo(float prevYVelo)
    {
        previousYVelo = prevYVelo;
    }
}
