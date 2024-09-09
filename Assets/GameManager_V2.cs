using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_V2 : MonoBehaviour
{
    public enum GameState
    {
        WAITING_FOR_PLAYERS,  // Esperando a que ambos jugadores se conecten
        CODING,               // Fase en la que los jugadores construyen sus funciones
        EXECUTING             // Fase en la que se ejecutan las funciones y los coches se mueven
    }

    public GameState actualGameState;

    private bool localPlayerInitialized = false;
    private int localPlayer_ID;

    [Header("Player Scenarios")]
    public List<GameObject> PlayableScenarios;  // Escenarios donde los jugadores pueden interactuar
    public List<GameObject> NonPlayableScenarios;

    private void Awake()
    {
        actualGameState = GameState.WAITING_FOR_PLAYERS;

        InitializeLocalPlayer();
    }

    private void InitializeLocalPlayer()
    {
        //Mandar al servidor petición para que nos diga si somos player 1 o 2

        //Después de sabe qué ID nos pertenece...

        //IniciarLocalPlayerScenario() --> Coloca el Rig de la parte del escenario que toque y pone la interfaz partner en el contrario.
    }
}
