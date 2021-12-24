using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;

    public Vector2 keyboardInput;
    public Vector2 mouseInput;
    public bool shiftHold;

    public void OnUpdate()
    {
        keyboardInput.x = Input.GetAxis("Horizontal");
        keyboardInput.y = Input.GetAxis("Vertical");

        mouseInput.x = Input.GetAxisRaw("Mouse X");
        mouseInput.y = Input.GetAxisRaw("Mouse Y");

        shiftHold = Input.GetKey(KeyCode.LeftShift);
    }
}
