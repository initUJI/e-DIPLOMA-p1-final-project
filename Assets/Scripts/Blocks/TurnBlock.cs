using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TurnBlock : ActionCharacterBlock, WithRightSocket
{
    [SerializeField] protected XRSocketInteractor rightSocket;

    public XRSocketInteractor getRightSocket()
    {
        return rightSocket;
    }

    public override void Execute()
    {
        StartCoroutine(c_Execute());
    }

    IEnumerator c_Execute()
    {
        isFinished = false;

        Block rightBlock = getSocketBlock(rightSocket);

        if (rightBlock != null)
        {
            switch (rightBlock.GetType().ToString())
            {
                case "TurnRightBlock":
                    ((TurnRightBlock)rightBlock).Execute();
                    yield return new WaitUntil(() => base.IsFinished());
                    break;
                case "TurnLeftBlock":
                    ((TurnLeftBlock)rightBlock).Execute();
                    yield return new WaitUntil(() => base.IsFinished());
                    break;
            }
        }

        isFinished = true;
    }

    public string getRightSocketString()
    {
        Block block = getSocketBlock(rightSocket);

        if (block != null)
        {
            Debug.Log("Hay Right Socket");
            return block.name;
        }
        else
        {
            Debug.Log("No hay Right Socket");
        }

        return null;
    }

    public override bool IsFinished()
    {
        return base.IsFinished() && isFinished;
    }

}
