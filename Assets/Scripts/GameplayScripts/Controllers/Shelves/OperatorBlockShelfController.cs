using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class OperatorBlockShelfController : ShelfController
{

    XRController principalHandController;
    float continuousValue;
    int value;
    float speed = 0.1f;
    public Vector2 joystickValue;

    OperatorBlock operatorCurrentBlock;
    XRBaseInteractable interactable;

    [SerializeField] string[] operators;
    protected override void Start()
    {
        GameManager.InvokeAfterInit(this, () =>
        {
            principalHandController = GameManager.principalHandController;
        });

        value = 0;

        base.Start();
    }

    protected override void CreateNewBlock()
    {
        base.CreateNewBlock();

        operatorCurrentBlock = currentBlock.GetComponent<OperatorBlock>();
        operatorCurrentBlock.SetOperator(operators[value]);
        interactable = currentBlock.GetComponent<XRBaseInteractable>();
    }


    private void Update()
    {
        if (interactable != null && interactable.isHovered)
        {
            InputDevice device = principalHandController.inputDevice;
            int previousValue;
            if (device != null && device.TryGetFeatureValue(CommonUsages.primary2DAxis, out joystickValue))
            {
                previousValue = value;
                continuousValue += joystickValue.y * speed;
                value = (int)continuousValue;
                value = Mathf.Abs(value);
                value = value % operators.Length;

                operatorCurrentBlock.SetOperator(operators[value]);
                


            }
        }
    }





}
