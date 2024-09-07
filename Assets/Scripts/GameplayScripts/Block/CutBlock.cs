using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CutBlock : ActionCharacterBlock, WithRightSocket
{
    [SerializeField] public XRSocketInteractor rightSocket;

    public XRSocketInteractor getRightSocket()
    {
        return rightSocket;
    }

    public override void Execute(Dictionary<string, int> variables)
    {
        Debug.Log(GameManager.character_P1);
        character.Cut();
    }

}
