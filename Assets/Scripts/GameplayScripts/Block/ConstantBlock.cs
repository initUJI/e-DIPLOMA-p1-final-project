using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ConstantBlock : Block, WithAssociatedString, WithRightSocket
{
    [SerializeField] public XRSocketInteractor rightSocket;

    [SerializeField] int constant;

    TMP_Text textMeshPro;

    public override void Start()
    {
        textMeshPro = GetComponentInChildren<TMP_Text>();
        textMeshPro.text = constant.ToString();
    }

    public void SetConstant(int constant)
    {
        this.constant = constant;
        if(textMeshPro != null)
        {
            textMeshPro.text = constant.ToString();
        }
        
    }

    public string getAssociatedString()
    {
        return "" + constant.ToString(); 
    }

    public int getValue()
    {
        return constant;
    }
    public XRSocketInteractor getRightSocket()
    {
        return rightSocket;
    }

}
