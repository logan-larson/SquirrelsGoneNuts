using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOrientation : MonoBehaviour
{

    [SerializeField]
    public PlayerController playerController;

    [SerializeField]
    private Quaternion orientation;

    public void MatchRotationToGround(Vector3 position, Quaternion rotation, Vector3 upDirection)
    {

        Quaternion q = new Quaternion();

        RaycastCone cone = playerController.playerState.GetHeightCone();


        q.SetFromToRotation(upDirection, cone.GetAverageNormal());


        this.playerController.playerState.SetUpDirection(cone.GetAverageNormal());
        this.playerController.playerState.SetRotation(q);

        // Don't set transform.rotation, set playerState.rotation
    }

    public void RotateOnInputs(Quaternion previousRotation)
    {

        // Don't set transform.rotation, set playerState.rotation
    }

    public void Orient(Quaternion previousRotation)
    {

        // Orient based on surface below

        RaycastHit belowFront = new RaycastHit();
        RaycastHit belowBack = new RaycastHit();

        if (Physics.Raycast(transform.position + (transform.forward / 2), -transform.up, out belowFront)
        && Physics.Raycast(transform.position + (-transform.forward / 2), -transform.up, out belowBack))
        {
            if (belowFront.distance < belowBack.distance)
            {
                transform.rotation = Quaternion.FromToRotation(Vector3.up, belowFront.normal);
            }
            else
            {
                transform.rotation = Quaternion.FromToRotation(Vector3.up, belowBack.normal);
            }


            //playerController.playerState.SetPreviousRotation(transform.rotation);
        }
        else
        {
            transform.rotation = Quaternion.FromToRotation(Vector3.up, Vector3.up);
        }

        // Orient based on mouse input

    }
}
