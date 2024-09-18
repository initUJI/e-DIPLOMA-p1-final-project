using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartnerMainBlock_Controller : MonoBehaviour
{
    public List<BlockObject> partnerSecuence;
    [SerializeField] Transform mainBlockBottomAttach;
    private List<GameObject> visualPartnerBlocks = new List<GameObject>();

    public void updatePartnerBlocks(List<BlockObject> newPartnerSecuence)
    {
        // Limpiar la secuencia actual
        ClearPreviousBlocks();
        partnerSecuence = newPartnerSecuence;

        // Posición inicial del primer bloque
        Transform currentAttachPoint = mainBlockBottomAttach;


        // Iterar sobre la secuencia de bloques con for
        for (int i = 0; i < partnerSecuence.Count; i++)
        {
            Quaternion adjustedRotation = Quaternion.Euler(0, -90, 0) * mainBlockBottomAttach.rotation;

            BlockObject blockObject = partnerSecuence[i];

            // Instanciar el prefab del BlockObject
            GameObject newBlock = Instantiate(blockObject.blockPrefab, currentAttachPoint.position, adjustedRotation);
            DisableScriptsOnParent(newBlock);
            visualPartnerBlocks.Add(newBlock);

            
            // Caso especial FOR_BLOCK
            if (newBlock.name.Contains("For"))
            {
                if ((i + 1) < partnerSecuence.Count)
                {
                    BlockObject possibleRightSocket = partnerSecuence[i + 1];

                    if (possibleRightSocket != null)
                    {
                        if (possibleRightSocket.blockName == "n2" || possibleRightSocket.blockName == "n3" || possibleRightSocket.blockName == "n4"
                            || possibleRightSocket.blockName == "n5" || possibleRightSocket.blockName == "n6" || possibleRightSocket.blockName == "n7"
                            || possibleRightSocket.blockName == "n8" || possibleRightSocket.blockName == "n9")
                        {
                            Transform directionAttachPoint = newBlock.transform.Find("Right Socket/Attach");
                            Debug.Log(directionAttachPoint);
                            adjustedRotation = Quaternion.Euler(0, 90, 0) * newBlock.transform.rotation;
                            
                            GameObject rightTurnSocketBlock = Instantiate(partnerSecuence[i + 1].blockPrefab, directionAttachPoint.position, adjustedRotation);
                            visualPartnerBlocks.Add(rightTurnSocketBlock);
                            DisableScriptsOnParent(rightTurnSocketBlock);
                            i++;
                        }
                    }
                }
            }
            // Caso especial de TURN_BLOCK
            else if (newBlock.name.Contains("Turn"))
            {
                adjustedRotation = Quaternion.Euler(0, 90, 0) * newBlock.transform.rotation;
                newBlock.transform.rotation = adjustedRotation;

                if ((i+1) < partnerSecuence.Count)
                {
                    BlockObject possibleRightSocket = partnerSecuence[i + 1];

                    if (possibleRightSocket != null)
                    {
                        if (possibleRightSocket.blockName == "Right" || possibleRightSocket.blockName == "Left")
                        {
                            Transform directionAttachPoint = newBlock.transform.Find("Right Socket/Attach");
                            GameObject rightTurnSocketBlock = Instantiate(partnerSecuence[i + 1].blockPrefab, directionAttachPoint.position, adjustedRotation);
                            visualPartnerBlocks.Add(rightTurnSocketBlock);
                            DisableScriptsOnParent(rightTurnSocketBlock);
                            i++;
                        }
                    }
                }
            }


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

    public void ClearPreviousBlocks()
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
