using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PrintBlock : ActionParametrizedBlock
{
    public override void Execute(Dictionary<string, int> variables)
    {
        isFinished = false;

        Debug.Log("PrintVariableBlock : BEGIN");
        Block block = getSocketBlock(rightSocket);
        if (block != null)
        {
            string variableName = ((VariableBlock)block).getAssociatedString();
            if (variables.ContainsKey(variableName))
            {
                GameManager.DisplayOnPrompt("Variable name : " + variableName + " ; Value : " + variables[variableName]);
            } else
            {
                GameManager.ReportError(this, "The variable has not been declared");
            }
         
        } else
        {
            GameManager.ReportError(this, "Missing parameter");
        }

        Debug.Log("PrintVariableBlock : END");

        WaitBeforeFinish(0.5f);
    }

}
