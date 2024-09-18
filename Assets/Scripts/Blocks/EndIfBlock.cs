using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndIfBlock : ActionCharacterBlock
{
    public override void Execute()
    {
        StartCoroutine(c_Execute());
    }

    IEnumerator c_Execute()
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
