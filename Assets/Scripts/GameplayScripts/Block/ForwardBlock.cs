using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ForwardBlock : ActionCharacterBlock, WithRightSocket
{
    [SerializeField] protected XRSocketInteractor rightSocket;

    public XRSocketInteractor getRightSocket()
    {
        return rightSocket;
    }

    public override void Execute(Dictionary<string, int> variables)
    {
        StartCoroutine(c_Execute(variables));
    }

    IEnumerator c_Execute(Dictionary<string, int> variables)
    {
        isFinished = false;

        Block rightBlock = getSocketBlock(rightSocket);

        //Debug.Log(rightBlock == null);

        if (rightBlock != null)
        {

            switch (rightBlock.GetType().ToString())
            {
                case "VariableBlock":

                    string variableName = ((VariableBlock)rightBlock).getAssociatedString();
                    if (variables.ContainsKey(variableName))
                    {
                        for (int i = 0; i < variables[variableName]; i++)
                        {
                            yield return new WaitUntil(() => !MainBlock.paused);
                            character.Forward();
                            yield return new WaitUntil(() => base.IsFinished());
                        }
                    }
                    else
                    {
                        GameManager.ReportError(this, "The variable has not been declared");
                    }

                    break;
                case "ConstantBlock":
                    for (int i = 0; i < Int64.Parse(((ConstantBlock)rightBlock).getAssociatedString()); i++)
                    {
                        yield return new WaitUntil(() => !MainBlock.paused);
                        character.Forward();
                        yield return new WaitUntil(() => base.IsFinished());

                    }

                    break;
            }
        }
        else
        {
            character.Forward();
        }

        isFinished = true;
    }

    public override bool IsFinished()
    {
        return /*base.IsFinished() && */isFinished;
    }



}
