
using System;
using System.Data;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public abstract class ConditionalBlock : ExecutableBlock, WithRightSocket, WithBottomSocket
{
    [SerializeField] public XRSocketInteractor bottomSocket;
    [SerializeField] protected XRSocketInteractor rightSocket;

    int substraction = 0;
    [HideInInspector] public bool isFinished;

    //Asignar el coche del jugador correspondiente
    [HideInInspector] public CarController carController;
    public XRSocketInteractor getRightSocket()
    {
        return rightSocket;
    }
    public XRSocketInteractor getBottomSocket()
    {
        return bottomSocket;
    }

    public override void Start()
    {
        base.Start();
        isFinished = false;
    }
    public bool conditionChecked()
    {
        String toInterpret = "";
        Block block = getSocketBlock(rightSocket);

        if (block == null)
        {
            Debug.Log("Missing condition");
        }

        while (block != null)
        {
            toInterpret += ((WithAssociatedString)block).getAssociatedString();
            if (block is WithRightSocket)
            {
                block = getSocketBlock(((WithRightSocket)block).getRightSocket());
            }
            else
            {
                block = null;
            }
        }

        bool result = InterpretConditionalExpression(toInterpret);

        if (!result)
        {
            isFinished = true;
        }

        return InterpretConditionalExpression(toInterpret);
    }

    public bool InterpretConditionalExpression(string expression)
    {
        bool result = false;
        switch (expression)
        {
            case "OBSTACLE":
                if (carController.GetCollidingObject() != null)
                {
                    result = carController.GetCollidingObject().CompareTag(expression);
                }
                break;
            case "!OBSTACLE":
                result = true;
                if (carController.GetCollidingObject() != null)
                {
                    string expressionCut = expression.Substring(1);
                    result = !(carController.GetCollidingObject().CompareTag(expressionCut));
                }
                break;

            default:
                int number;
                if (int.TryParse(expression, out number))
                {
                    if (number - substraction > 0)
                    {
                        result = true;
                        substraction++;
                    }
                    else
                    {
                        substraction = 0;
                        result = false;
                    }
                }
                else
                {
                    try
                    {
                        DataTable dt = new DataTable();
                        result = (bool)dt.Compute(expression, "");
                    }
                    catch
                    {
                        Debug.Log("Incorrect Boolean expression");
                    }

                }

                break;
        }
        return result;
    }
}
