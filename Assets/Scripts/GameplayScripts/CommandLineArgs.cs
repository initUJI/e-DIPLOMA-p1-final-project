using UnityEngine;
using System;

public class CommandLineArgs : MonoBehaviour
{
    string[] args;
    EventsManager eventsManager;

    void Start()
    {
        // Obtener los argumentos de la línea de comandos
        args = Environment.GetCommandLineArgs();

        // Procesar los argumentos
        for (int i = 0; i < args.Length; i++)
        {
            Debug.Log($"Argument {i}: {args[i]}");
        }

        eventsManager = GameObject.FindObjectOfType<EventsManager>();

        if (getFirstArgument() != null)
        {
            if (PlayerPrefs.GetString("UserID") != getFirstArgument())
            {
                eventsManager.setUserID(getFirstArgument());
            }          
        }
    }


    public string getFirstArgument()
    {
        string parameter = null;

        if (args.Length > 1)
        {
            parameter = args[1]; // Primer argumento pasado después del nombre del .exe
            Debug.Log($"First parameter: {parameter}");
            // Realiza acciones basadas en el argumento
        }

        return parameter;
    }
}

