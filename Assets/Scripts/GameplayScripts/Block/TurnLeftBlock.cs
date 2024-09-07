using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnLeftBlock : ActionCharacterBlock
{

    public override void Execute(Dictionary<string, int> variables)
    {
        character.TurnLeft();
    }

}
