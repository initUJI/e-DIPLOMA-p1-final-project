using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Tool : MonoBehaviour
{
    public static IEnumerator c_InvokeAfterWait(float time, Action action)
    {
        //Debug.Log("Waiting..");
        yield return new WaitForSeconds(time);
        //Debug.Log("Invoke !");
        action.Invoke();

        yield return null;
    }

    public static IEnumerator c_InvokeAfterWaitBool(bool toWait, Action action)
    {

        yield return new WaitUntil(() => !toWait);
        action.Invoke();

        yield return null;
    }

    public static IEnumerator c_InvokeBeforeWait(float time, Action WhenTimeFinish)
    {
        //Debug.Log("Invoke !");
        WhenTimeFinish.Invoke();
        //Debug.Log("Waiting..");
        yield return new WaitForSeconds(time);

        yield return null;
    }

    public static IEnumerator c_ChangeBoolAfterWait(bool toChange, float toWait)
    {
        yield return new WaitForSeconds(toWait);
        toChange = !toChange;
        yield return null;
    }


    public static bool CheckType(System.Object obj, string type, bool message)
    {
        bool result = false;

        if (obj.GetType().ToString().Equals(type))
        {
            result = true;
        }
        else
        {
            if (message)
            {
                //Debug.Log(obj.ToString() + " isn't of type " + type + ", but of type " + obj.GetType().ToString());
            }
        }

        return result;
    }

    public static bool CheckType(System.Object obj, string type)
    {
        return CheckType(obj, type, false);
    }

    public static bool CheckNotNull(System.Object obj)
    {
        bool result = false;

        if (obj != null)
        {
            result = true;
        }
        else
        {
            //Debug.Log("This object is null");
        }

        return result;

    }

    public static GameObject FindChildWithTag(GameObject parent, string tag)
    {
        GameObject result = null;

        foreach (Transform child in parent.transform)
        {
            if (child.CompareTag(tag))
            {
                result = child.gameObject;
            }
        }

        return result;
    }

    public static void displayVariables(Dictionary<string, int> variables)
    {
        string toDisplay = "";

        foreach (KeyValuePair<string, int> variable in variables)
        {
            toDisplay += "(" + variable.Key.ToString() + "; " + variable.Value.ToString() + ") ";
        }

        //Debug.Log(toDisplay);
    }

    public static int InterpretArithmeticalExpression(string expression, Dictionary<string, int> variables)
    {
        string result = ReplaceVariablesInExpression(expression, variables);
        DataTable dt = new DataTable();
        return (int)dt.Compute(result, "");
    }


    public static string ReplaceVariablesInExpression(string expression, Dictionary<string, int> variables)
    {

        string result = expression;

        foreach (KeyValuePair<string, int> variable in variables)
        {
            result = result.Replace(variable.Key.ToString(), variable.Value.ToString());
        }

        return result;
    }
}
