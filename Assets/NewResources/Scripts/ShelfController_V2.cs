using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShelfController_V2 : MonoBehaviour
{
    public BlockObject block;
    [SerializeField] Transform blockPlacer;
    [SerializeField] TextMeshProUGUI blockQuantityVisual;

    public GameObject currentBlock;
    public int availableBlockQuantity;

    public void SetupShelfController()
    {
        if(currentBlock != null)
        {
            Destroy(currentBlock);
        }

        //Se actualiza el texto del holder
        if(availableBlockQuantity == 0)
        {
            blockQuantityVisual.text = "";
        }else
        {
            blockQuantityVisual.text = availableBlockQuantity.ToString();

            //Se instancia el Holder
            currentBlock = Instantiate(block.blockPrefab, blockPlacer.position, blockPlacer.rotation, null);
            //currentBlock.transform.parent = blockPlacer;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == currentBlock)
        {
            availableBlockQuantity -= 1;

            if(availableBlockQuantity > 0)
            {
                blockQuantityVisual.text = availableBlockQuantity.ToString();

                //Se instancia el Holder
                currentBlock = Instantiate(block.blockPrefab, blockPlacer.position, blockPlacer.rotation, null);
            }else
            {
                availableBlockQuantity = 0;

                blockQuantityVisual.text = availableBlockQuantity.ToString();
            }
        }
    }
}
