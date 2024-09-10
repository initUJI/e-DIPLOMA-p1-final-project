using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionCharacterBlock : ActionBlock
{
    //Asignar
    protected CarController character;

    public override bool IsFinished()
    {
        return character.Motionless();
    }

    public override void Start()
    {
        base.Start();

        // Get the block name
        /*GameManager.InvokeAfterInit(this, () =>
        {
            character = GameManager.character;
        });*/
    }
}
