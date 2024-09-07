using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class VariableBlock : ActionParametrizedBlock, WithAssociatedString
{


    public new string name;

    TMP_Text textMeshPro;

    public override void Start()
    {
        base.Start();
        textMeshPro = GetComponentInChildren<TMP_Text>();
        textMeshPro.text = name;
    }

    public void setName(string name)
    {
        this.name = name;
        if (textMeshPro != null)
        {
            textMeshPro.text = name;
        }
    }

    public string getAssociatedString()
    {
        return name;
    }

    public override void Execute(Dictionary<string, int> variables)
    {
        String toInterpret = "";
        Block block = getSocketBlock(rightSocket);

        if(block != null) {
            block = getSocketBlock(((WithRightSocket)block).getRightSocket());
        } else
        {
            GameManager.ReportError(this, "The new value of the variable is missing");
        }

        while(block != null)
        {
            toInterpret += ((WithAssociatedString)block).getAssociatedString();
            block = getSocketBlock(((WithRightSocket) block).getRightSocket());
        }

        try
        {
            if(!variables.ContainsKey(name))
            {
                GameManager.ReportWarning(this, "It is preferable to declare the variable before using it");
            }

            variables[name] = Tool.InterpretArithmeticalExpression(toInterpret, variables);
        } catch
        {
            GameManager.ReportError(this, "The expression is incomplete or incorrect");
        }
        

        WaitBeforeFinish(1f);
    }

    public override string ToString()
    {
        return base.ToString() + " (" + name + " )";
    }


}
