using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartnerMainBlock_Controller : MonoBehaviour
{
    public List<BlockObject> partnerSecuence;
    public Transform mainBlockBottomAttach;
    private List<GameObject> visualPartnerBlocks = new List<GameObject>();

    public void updatePartnerBlocks(List<BlockObject> newPartnerSecuence)
    {
        // Limpiar la secuencia actual
        ClearPreviousBlocks();
        partnerSecuence = newPartnerSecuence;

        // Posición inicial del primer bloque
        Transform currentAttachPoint = mainBlockBottomAttach;

        // Obtener la rotación original y restarle 90 grados en el eje Y
        Quaternion adjustedRotation = Quaternion.Euler(0, -90, 0) * currentAttachPoint.rotation;

        // Iterar sobre la secuencia de bloques
        foreach (BlockObject blockObject in partnerSecuence)
        {
            // Instanciar el prefab del BlockObject
            GameObject newBlock = Instantiate(blockObject.blockPrefab, currentAttachPoint.position, adjustedRotation);
            visualPartnerBlocks.Add(newBlock);

            DisableScriptsOnParent(newBlock);

            // Buscar el siguiente punto de anclaje en el nuevo bloque instanciado
            Transform nextAttachPoint = newBlock.transform.Find("BottomSocket/Attach");

            if (nextAttachPoint != null)
            {
                // Actualizar el punto de anclaje para el próximo bloque
                currentAttachPoint = nextAttachPoint;
            }
            else
            {
                Debug.LogError("No se encontró el punto de anclaje en el bloque " + blockObject.blockName);
                break; // Romper el bucle si falta el siguiente punto de anclaje
            }
        }
    }

    private void ClearPreviousBlocks()
    {
        foreach (GameObject block in visualPartnerBlocks)
        {
            Destroy(block);  // Destruir los objetos instanciados previamente
        }
        visualPartnerBlocks.Clear();  // Limpiar la lista
    }

    private void DisableScriptsOnParent(GameObject obj)
    {
        // Obtener todos los componentes de tipo MonoBehaviour en el objeto principal
        MonoBehaviour[] scripts = obj.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            Destroy(script);  // Eliminar el componente script del objeto principal
        }
    }
}
