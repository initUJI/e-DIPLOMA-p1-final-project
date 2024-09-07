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

    [Header("Player Scenarios")]
    public List<GameObject> PlayableScenarios;  // Escenarios donde los jugadores pueden interactuar
    public List<GameObject> NonPlayableScenarios;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
