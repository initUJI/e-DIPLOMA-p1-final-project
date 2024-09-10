using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnRightBlock : ActionCharacterBlock
{
    public override void Execute()
    {
        character.TurnRight();
    }
}