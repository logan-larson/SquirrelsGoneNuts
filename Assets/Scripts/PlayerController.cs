using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public PlayerInput playerInput;
    [SerializeField]
    public PlayerMovement playerMovement;
    [SerializeField]
    public PlayerState playerState;
    [SerializeField]
    public PlayerOrientation playerOrientation;

    private float previousYVelo;

    void Start()
    {

    }

    void FixedUpdate()
    {

        // Get inputs
        playerInput.OnUpdate();

        // Move player
        playerMovement.MoveOnInputs(playerInput.GetKeyboardInput().x, playerInput.GetKeyboardInput().y);

        // Rotate based on mouse input
        //playerOrientation.RotateOnInputs(playerState.GetRotation());


        // -- NEW PLAN --



        /*
        playerOrientation.MatchRotationToGround(playerState.GetPosition(), playerState.GetRotation(), playerState.GetUpDirection());

        playerMovement.SetHeightAboveGround();


        // Update the  current player state 
        playerState.UpdateState();
        */

    }

}
