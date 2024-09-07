using UnityEngine;

[CreateAssetMenu(fileName = "New BlockObject", menuName = "BlockProgramming/BlockObject", order = 1)]
public class BlockObject : ScriptableObject
{
    public string blockName;         // Nombre del bloque (ej. "MoveForward")
    public GameObject blockPrefab;   // Prefab asociado al bloque

    // Aquí puedes añadir más propiedades específicas para cada tipo de bloque, como parámetros o valores predeterminados.
}
