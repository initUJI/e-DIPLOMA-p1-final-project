using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IfBlock : ConditionalBlock
{
    Coroutine currentCoroutine;

    /* Calls the c_Execute coroutine */
    public override void Execute()
    {
        Execute();
    }

    public override bool IsFinished()
    {
        return isFinished;
    }
}
