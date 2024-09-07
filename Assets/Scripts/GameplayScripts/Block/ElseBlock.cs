using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ElseBlock : LocalContextBlock
{

    /* Calls the c_Execute coroutine */
    public override void Execute(Dictionary<string, int> variables)
    {
        this.variables = variables;
        Execute();
    }





}
