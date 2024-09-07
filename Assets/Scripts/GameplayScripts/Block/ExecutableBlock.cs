using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public abstract class ExecutableBlock : Block
{
    public abstract void Execute(Dictionary<string, int> variables);

    public abstract bool IsFinished();

}
