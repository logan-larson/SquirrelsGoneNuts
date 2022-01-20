using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private Vector2 keyboardInput;
    private Vector2 mouseInput;
    private bool middleMouseHold;
    private bool shiftHold;
    private bool spaceToggle;

    public void OnUpdate()
    {
        keyboardInput.x = Input.GetAxisRaw("Horizontal");
        keyboardInput.y = Input.GetAxisRaw("Vertical");

        //mouseInput.x = Input.GetAxisRaw("Mouse X");
        //mouseInput.y = Input.GetAxisRaw("Mouse Y");
        mouseInput.x = Input.GetAxis("Mouse X");
        mouseInput.y = Input.GetAxis("Mouse Y");

        // Camera freelook on middle mouse button hold
        middleMouseHold = Input.GetMouseButton(2);

        shiftHold = Input.GetKey(KeyCode.LeftShift);
        spaceToggle = Input.GetKeyDown(KeyCode.Space);
    }

    public Vector2 GetKeyboardInput()
    {
        return keyboardInput;
    }

    public Vector2 GetMouseInput()
    {
        return mouseInput;
    }

    public bool GetMiddleMouseHold() {
        return middleMouseHold;
    }

    public bool GetShiftHold()
    {
        return shiftHold;
    }

    public bool GetSpaceToggle()
    {
        return spaceToggle;
    }
}
