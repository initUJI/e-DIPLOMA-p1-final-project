using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DeclareBlock : ActionParametrizedBlock
{

    public override void Execute(Dictionary<string, int> variables)
    {
        isFinished = false;

        Block block = getSocketBlock(rightSocket);

        if (block.GetType().ToString().Equals("VariableBlock"))
        {
            VariableBlock varBlock = (VariableBlock) block;
            switch(varBlock.getAssociatedString())
            {
                case "Variable":
                    Debug.Log("Error in DeclareBlock : Bad name for variable");
                    break;

                default:
                    string varName = varBlock.getAssociatedString();
                    if (!variables.ContainsKey(varName))
                    {
                        variables.Add(varName, 0);
                    } else
                    {
                        GameManager.printScreenTMP.text = "ERROR : The variable " + varName + " is already declared.";
                    }
                    break;
            }

           
        }
        else
        {
            Debug.Log("Error in DeclareBlock : Right member isn't a VariableBlock, is a " + block.GetType().ToString());
            
            
        }

        WaitBeforeFinish(1f); 
    }
}