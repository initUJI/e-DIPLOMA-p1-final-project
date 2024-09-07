using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/* Block is a script associated with the GameObject of the same name, used to manage its logic. 
 * A Block has 3 sockets, one to its right, one below it and a third to place a local context, 
 * particularly in the case of while or if statements.
*/
public abstract class Block : MonoBehaviour
{

    public int value;

    public LocalContextBlock referentBlock;

    public Block bottomBlock;

    [SerializeField] Material[] materials;

    Material[] newMaterials;

    private Renderer objectRenderer;

    bool glowing;

    public virtual void Start()
    {
        value = 1;
        referentBlock = null;
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {  
            newMaterials = objectRenderer.materials;
        }
        else
        {
            objectRenderer = transform.GetChild(0).GetComponent<Renderer>();
            newMaterials = objectRenderer.materials;
        }
        if (newMaterials.Length > 1)
        {
            newMaterials[1] = materials[0];
        }

        objectRenderer.materials = newMaterials;
        glowing = false;
    }

    public virtual Block getSocketBlock(XRSocketInteractor socket)
    {
        
            Block block = null;

            if (socket != null && socket.GetOldestInteractableSelected() != null)
            {
                block = socket.GetOldestInteractableSelected().transform.gameObject.GetComponent<Block>();
            } 

            return block;
        
    }

    public virtual void InformReferent()
    {
        if (referentBlock != null)
        {
            referentBlock.BrowseChildAndUpdate();
        }
    }

    public void TransmitNextBlocksToReferent()
    {

        bottomBlock = getSocketBlock(((WithBottomSocket)this).getBottomSocket());

        Block currentBlock = bottomBlock;
        while (currentBlock != null)
        {
            currentBlock.referentBlock = referentBlock;
            if (currentBlock is WithBottomSocket)
            {
                currentBlock = getSocketBlock(((WithBottomSocket)currentBlock).getBottomSocket());
            }
        }

        InformReferent();

    }

    public void ResetNextBlocks()
    {

        Block currentBlock = bottomBlock;
        while (currentBlock != null)
        {
            currentBlock.referentBlock = null;
            currentBlock = getSocketBlock(((WithBottomSocket)currentBlock).getBottomSocket());

        }

        InformReferent();
    }


    public void ToogleGlow()
    {
        
        if (!glowing)
        {
            ChangeBlockMaterial(materials[1]);
        }
        else
        {
            ChangeBlockMaterial(materials[0]);
        }

        glowing = !glowing;
    }

    public void SetGlowing(bool glowing)
    {
        this.glowing = glowing;

        if (glowing)
        {
            ChangeBlockMaterial(materials[1]);
        }
        else
        {
            ChangeBlockMaterial(materials[0]);
        }
    }

    public override string ToString()
    {
        return GetType().ToString();
    }

    public void ChangeBlockMaterial(Material material)
    {
        if (newMaterials.Length > 1)
        {
            newMaterials[1] = material;
        }

        objectRenderer.materials = newMaterials;
    }

    public void InitBlockMaterial()
    {
        ChangeBlockMaterial(materials[0]);
    }

}
