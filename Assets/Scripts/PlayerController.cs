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

        // Get current player state 
        playerState.UpdateState();

        // Orient player
        playerOrientation.Orient(playerState.GetPreviousRotation());

        // Move player
        playerMovement.Move(
            playerInput.getKeyboardInput().x,
            playerInput.getKeyboardInput().y,
            playerInput.getShiftHold(),
            playerInput.getSpaceToggle(),
            playerState.GetPreviousYVelo(),
            playerState.GetPreviousPosition()
        );

    }

}
