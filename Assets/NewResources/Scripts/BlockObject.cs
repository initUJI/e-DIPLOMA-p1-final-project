using UnityEngine;

[CreateAssetMenu(fileName = "New BlockObject", menuName = "BlockProgramming/BlockObject", order = 1)]
public class BlockObject : ScriptableObject
{
    public string blockName;         // Nombre del bloque (ej. "MoveForward")
    public GameObject blockPrefab;   // Prefab asociado al bloque

    // Aqu� puedes a�adir m�s propiedades espec�ficas para cada tipo de bloque, como par�metros o valores predeterminados.
}
