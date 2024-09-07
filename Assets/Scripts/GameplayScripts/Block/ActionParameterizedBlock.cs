using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public abstract class ActionParametrizedBlock : ActionBlock, WithRightSocket
{

    [SerializeField] public XRSocketInteractor rightSocket;

    public XRSocketInteractor getRightSocket()
    {
        return rightSocket;
    }

}

