using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndForBlock : ActionCharacterBlock
{
    public override void Execute(Dictionary<string, int> variables)
    {
        StartCoroutine(c_Execute(variables));
    }

    IEnumerator c_Execute(Dictionary<string, int> variables)
    {
        isFinished = false;

        yield return new WaitUntil(() => base.IsFinished());
        isFinished = true;
    }

    public override bool IsFinished()
    {
        return base.IsFinished() && isFinished;
    }
}
