using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TurnLeftBlock : ActionCharacterBlock
{
    public override void Execute()
    {
        character.TurnLeft();
    }
}