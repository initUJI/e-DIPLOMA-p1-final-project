using UnityEngine;

[CreateAssetMenu(fileName = "GameBoardConfig", menuName = "Game/Board Configuration", order = 1)]
public class GameBoardConfig : ScriptableObject
{
    public GameObject boardPrefab;  // Prefab del tablero a cargar
    public string description;      // Descripción de la dinámica del tablero
}
