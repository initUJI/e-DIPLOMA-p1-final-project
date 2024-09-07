using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public abstract class ActionBlock : ExecutableBlock, WithBottomSocket
{
    [SerializeField] public XRSocketInteractor bottomSocket;

    protected bool isFinished;

    public XRSocketInteractor getBottomSocket()
    {
        return bottomSocket;
    }

    public override bool IsFinished()
    {
        return isFinished;

    }

    protected void ToogleIsFinished()
    {
        isFinished = !isFinished;
    }

    protected void WaitBeforeFinish(float time)
    {
        StartCoroutine(Tool.c_InvokeAfterWait(time, ToogleIsFinished));
    }



}

