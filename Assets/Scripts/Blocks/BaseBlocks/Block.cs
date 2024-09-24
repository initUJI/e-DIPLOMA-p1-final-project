using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public abstract class Block : MonoBehaviour
{
    [HideInInspector] public Block bottomBlock;

    public virtual void Start()
    {

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

    public void OnSocketPlaced()
    {
        XRController[] xrCont = (XRController[]) GameObject.FindObjectsOfType(typeof(XRController));

        foreach(XRController control in xrCont)
        {
            control.SendHapticImpulse(0.3f, 0.2f);
        }
    }

    public void ResetNextBlocks()
    {
        Block currentBlock = bottomBlock;
        while (currentBlock != null)
        {
            currentBlock = getSocketBlock(((WithBottomSocket)currentBlock).getBottomSocket());

        }
    }

    public override string ToString()
    {
        return GetType().ToString();
    }

    public void UpdatedBottomBlock()
    {
        // Find GameManager 
        GameManager_V2 gameManager = FindObjectOfType<GameManager_V2>();
        gameManager.sendLocalSecuenceToServer();
    }
}
