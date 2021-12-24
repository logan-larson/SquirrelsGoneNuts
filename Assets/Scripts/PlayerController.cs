using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private PlayerInput playerInput;

    [SerializeField]
    private PlayerMovement playerMovement;


    void Start()
    {
        
    }

    void FixedUpdate()
    {
        // Get inputs
        playerInput.OnUpdate();

        // Move player
        playerMovement.Move(playerInput.keyboardInput.x, playerInput.keyboardInput.y, playerInput.shiftHold);

    }
}
