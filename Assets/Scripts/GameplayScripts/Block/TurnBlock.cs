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

    public override void Execute(Dictionary<string, int> variables)
    {
        StartCoroutine(c_Execute(variables));
    }

    IEnumerator c_Execute(Dictionary<string, int> variables)
    {
        isFinished = false;

        Block rightBlock = getSocketBlock(rightSocket);

        if (rightBlock != null)
        {
            switch (rightBlock.GetType().ToString())
            {
                case "TurnRightBlock":
                    ((TurnRightBlock)rightBlock).Execute(variables);
                    yield return new WaitUntil(() => base.IsFinished());
                    break;
                case "TurnLeftBlock":
                    ((TurnLeftBlock)rightBlock).Execute(variables);
                    yield return new WaitUntil(() => base.IsFinished());
                    break;
            }
        }
        
        isFinished = true;
    }

    public override bool IsFinished()
    {
        return base.IsFinished() && isFinished;
    }

}
