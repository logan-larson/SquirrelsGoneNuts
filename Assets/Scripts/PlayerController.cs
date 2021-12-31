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

        // -- NEW PLAN --
        playerMovement.SetHeightAboveGround();

        playerOrientation.MatchRotationToGround(playerState.GetPosition(), playerState.GetRotation(), playerState.GetUpDirection());

        // Move player
        playerMovement.MoveOnInputs(playerState.GetPosition(), playerInput.GetKeyboardInput().x, playerInput.GetKeyboardInput().y);

        // Rotate based on mouse input
        playerOrientation.RotateOnInputs(playerState.GetRotation());

        // Update the  current player state 
        playerState.UpdateState();

    }

}
