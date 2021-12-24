using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private PlayerInput playerInput;

    [SerializeField]
    private PlayerMovement playerMovement;

    [SerializeField]
    private PlayerState playerState;

    private float previousYVelo;

    void Start()
    {

    }

    void FixedUpdate()
    {

        // Get inputs
        playerInput.OnUpdate();

        playerState.UpdateState();

        // Move player
        previousYVelo = playerState.getPreviousYVelo();

        playerMovement.Move(
            playerInput.getKeyboardInput().x,
            playerInput.getKeyboardInput().y,
            playerInput.getShiftHold(),
            playerInput.getSpaceToggle(),
            previousYVelo
        );

    }
}
