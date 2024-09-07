using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BreakBlock : ActionCharacterBlock
{

    public override void Execute(Dictionary<string, int> variables)
    {
        Debug.Log(GameManager.character_P1);
        character.Break();
    }
    
}
