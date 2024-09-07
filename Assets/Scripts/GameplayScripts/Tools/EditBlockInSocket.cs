using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class EditBlockInSocket : MonoBehaviour
{
    [Header("Bottom socket")]
    [SerializeField] string[] bottomSocketNewLayers;
    [SerializeField] bool desactivateBottomSocket;
    InteractionLayerMask bottomSocketDefaultLayers;

    [Header("Right socket")]
    [SerializeField] string[] rightSocketNewLayers;
    [SerializeField] bool desactivateRightSocket;
    InteractionLayerMask rightSocketDefaultLayers;



    [Header("Input Socket")]
    [SerializeField] XRSocketInteractor inputSocket;

    Block block, neighborBlock;

    private void Start()
    {
        block = GetComponent<Block>();

        inputSocket.selectEntered.AddListener(interactor =>
        {
            neighborBlock = block.getSocketBlock(inputSocket);
            EditBlock();
        });

        inputSocket.selectExited.AddListener(interactor =>
        {
            ResetBlock();
        });
    }

    

    /*
     * Modifies the state of the block and its sockets if necessary.
     * This method is used when the block enters the socket.
     */
    public void EditBlock()
    {
        if (neighborBlock is WithBottomSocket)
        {
            XRSocketInteractor bottomSocket = ((WithBottomSocket)neighborBlock).getBottomSocket();

            if (desactivateBottomSocket)
            {
                bottomSocket.gameObject.SetActive(false);
            } else if (bottomSocketNewLayers.Length > 0) {

                bottomSocketDefaultLayers = bottomSocket.interactionLayers;
                bottomSocket.interactionLayers = InteractionLayerMask.GetMask(bottomSocketNewLayers);

            }

            

        }

        if (neighborBlock is WithRightSocket)
        {
            XRSocketInteractor rightSocket = ((WithRightSocket)neighborBlock).getRightSocket();

            if (desactivateRightSocket)
            {
                rightSocket.gameObject.SetActive(false);
            } else if (rightSocketNewLayers.Length > 0)
            {

                rightSocketDefaultLayers = rightSocket.interactionLayers;
                rightSocket.interactionLayers = InteractionLayerMask.GetMask(rightSocketNewLayers);

            }

            
        }

    }

    /*
     * Resets the state of the block and its sockets if necessary.
     * This method is used when the block quits the socket.
     */
    public void ResetBlock()
    {
        if (neighborBlock is WithBottomSocket)
        {
            XRSocketInteractor bottomSocket = ((WithBottomSocket)neighborBlock).getBottomSocket();

            if (desactivateBottomSocket)
            {
                bottomSocket.gameObject.SetActive(true);
            }
            else if (bottomSocketNewLayers.Length > 0)
            {

                bottomSocket.interactionLayers = bottomSocketDefaultLayers;
            }

        }

        if (neighborBlock is WithRightSocket)
        {
            XRSocketInteractor rightSocket = ((WithRightSocket)neighborBlock).getRightSocket();

            if (desactivateRightSocket)
            {
                rightSocket.gameObject.SetActive(true);
            } else if(rightSocketNewLayers.Length > 0)
            {
                rightSocket.interactionLayers = rightSocketDefaultLayers;
            }
        }
    }
}
