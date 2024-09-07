using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class MainBlockMovementController : MonoBehaviour
{
    XRController principalHandController;
    float rotationSpeed = 2f;
    float verticalSpeed = 0.4f; 
    public Vector2 joystickValue;

    XRBaseInteractable interactable;

    private void Start()
    {
        GameManager.InvokeAfterInit(this, () =>
        {
            principalHandController = GameManager.principalHandController;
        });

        interactable = GetComponent<XRBaseInteractable>();
    }

    private void Update()
    {
        if (interactable != null && interactable.isHovered && principalHandController != null)
        {
            InputDevice device = principalHandController.inputDevice;

            if (device != null && device.TryGetFeatureValue(CommonUsages.primary2DAxis, out joystickValue))
            {

                float rotationAmount = joystickValue.x * rotationSpeed;
                transform.Rotate(0f, rotationAmount, 0f, Space.World);

                if(Mathf.Abs(joystickValue.y) > 0.8f) {
                    float verticalMovement = joystickValue.y * verticalSpeed * Time.deltaTime;
                    transform.Translate(0f, verticalMovement, 0f, Space.World);
                }
            }
        }
    }

    
}
