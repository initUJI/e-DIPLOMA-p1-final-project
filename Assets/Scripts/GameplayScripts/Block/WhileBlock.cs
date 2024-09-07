using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;
//using UnityEngine.WSA;
//using UnityEngine.XR.Interaction.Toolkit;

public class WhileBlock : ConditionalBlock
{
    bool whileIsFinished;
    Coroutine whileCoroutine;

    public override void Start()
    {
        base.Start();
        whileIsFinished = false;
    }
    public override void Execute(Dictionary<string, int> variables)
    {
        this.variables = variables;

        if(whileCoroutine != null)
        {
            StopCoroutine(whileCoroutine);
        }

        whileCoroutine = StartCoroutine(c_ExecuteWhile());
    }

    public IEnumerator c_ExecuteWhile()
    {
        whileIsFinished = false;

        while (conditionChecked(variables))
        {
            Debug.Log("Condition vérifiée !");

            if(currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }

            // Launch and wait the end of c_Execute
            yield return (currentCoroutine = StartCoroutine(c_Execute()));
        }

        whileIsFinished = true;
    }

    public override bool IsFinished()
    {
        return whileIsFinished;
    }

}
