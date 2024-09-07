using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class OperatorBlock : Block, WithAssociatedString, WithRightSocket
{

    [SerializeField] public XRSocketInteractor rightSocket;

    [SerializeField] string ope;

    TMP_Text textMeshPro;

    public override void Start()
    {
        textMeshPro = GetComponentInChildren<TMP_Text>();
        textMeshPro.text = ope;

        
    }

    public void SetOperator(string ope)
    {
        this.ope = ope;
        if (textMeshPro != null)
        {
            textMeshPro.text = ope;
        }

        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        switch (ope)
        {
            case "==":
            case "!=":
            case "<=":
            case ">=":
            case "<":
            case ">":
                // BoolOperators
                grabInteractable.interactionLayers = InteractionLayerMask.GetMask(new string[] { "Bool Operator Block" });
                break;
            case "+":
            case "-":
                // ArithmeticOperators
                grabInteractable.interactionLayers = InteractionLayerMask.GetMask(new string[] { "Arithmetic Operator Block" });
                break;
            case "=":
                // AssignementOperator
                grabInteractable.interactionLayers = InteractionLayerMask.GetMask(new string[] { "Assignment Operator Block" });
                break;


        }
    }
    public string getAssociatedString()
    {
        return ope;
    }

    public XRSocketInteractor getRightSocket()
    {
        return rightSocket;
    }
}

