using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public abstract class ExecutableBlock : Block
{
    public abstract void Execute();

    public abstract bool IsFinished();

}
