using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class TestInput : MonoBehaviour
{

    public InputActionReference joystickInput;

    public Vector2 joystickValue;
    public Vector2 inputValue;
    public XRController controller;

    // Start is called before the first frame update
    void Awake()
    {
        joystickInput.action.performed += SetNewValue;
    }

    private void SetNewValue(InputAction.CallbackContext obj)
    {
        inputValue = obj.ReadValue<Vector2>();
        Debug.Log(inputValue);
    }

    private void Update()
    {

        UnityEngine.XR.InputDevice device = controller.inputDevice;
        if (device != null && device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out joystickValue))
        {
            // Utilisez le joystickValue comme vous le souhaitez
            // Par exemple, vous pouvez acc�der aux composantes x et y :
            float joystickX = joystickValue.x;
            float joystickY = joystickValue.y;
            //Debug.Log("(" + joystickX + " ; " + joystickY + ")");

            // Faites quelque chose avec les valeurs du joystick
        }
    }
}
