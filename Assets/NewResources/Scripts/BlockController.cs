using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BlockController : MonoBehaviour
{
    public Transform blockToAttach;

    public void OnBlockCatched()
    {
        this.transform.parent = null;
        blockCatched();
    }

    public void updateBlockToAttach(Transform newBlockToAttach)
    {
        blockToAttach = newBlockToAttach;
    }

    public void releaseGrabOnBlock()
    {
        if(blockToAttach != null)
        {
            this.transform.position = blockToAttach.transform.position;
            this.transform.rotation = blockToAttach.transform.rotation;

            Transform atachLight = blockToAttach.transform.parent.Find("PointLight");
            if(atachLight != null)
            {
                atachLight.gameObject.SetActive(false);
            }
            blockToAttach.gameObject.SetActive(false);
        }
    }

    public void blockCatched()
    {
        if(blockToAttach != null)
        {
            blockToAttach.gameObject.SetActive(true);

            blockToAttach = null;
        }
    }

    void OnSelectExited(SelectExitEventArgs args)
    {
        releaseGrabOnBlock();
    }
}
