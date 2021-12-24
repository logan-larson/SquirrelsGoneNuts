using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;

    private Vector2 keyboardInput;
    private Vector2 mouseInput;
    private bool shiftHold;
    [SerializeField] private bool spaceToggle;

    public void OnUpdate()
    {
        keyboardInput.x = Input.GetAxisRaw("Horizontal");
        keyboardInput.y = Input.GetAxisRaw("Vertical");

        mouseInput.x = Input.GetAxisRaw("Mouse X");
        mouseInput.y = Input.GetAxisRaw("Mouse Y");

        shiftHold = Input.GetKey(KeyCode.LeftShift);
        spaceToggle = Input.GetKeyDown(KeyCode.Space);
    }

    public Vector2 getKeyboardInput()
    {
        return keyboardInput;
    }

    public Vector2 getMouseInput()
    {
        return mouseInput;
    }

    public bool getShiftHold()
    {
        return shiftHold;
    }

    public bool getSpaceToggle()
    {
        return spaceToggle;
    }
}
