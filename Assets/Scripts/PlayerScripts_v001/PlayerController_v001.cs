using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_v001 : MonoBehaviour
{
    [SerializeField]
    public PlayerInput playerInput;
    [SerializeField]
    public PlayerMovement_v001 playerMovement;
    [SerializeField]
    public PlayerState_v001 playerState;
    [SerializeField]
    public PlayerOrientation_v001 playerOrientation;

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
