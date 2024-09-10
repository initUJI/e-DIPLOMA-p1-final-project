using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ForwardBlock : ActionCharacterBlock
{
    public override void Execute()
    {
        StartCoroutine(c_Execute());
    }

    IEnumerator c_Execute()
    {
        isFinished = false;

        // Iniciar el movimiento hacia adelante y esperar a que termine
        yield return StartCoroutine(character.Forward());

        // Movimiento hacia adelante finalizado, ahora continuar con la ejecución
        isFinished = true;
    }

    public override bool IsFinished()
    {
        return isFinished;
    }
}
