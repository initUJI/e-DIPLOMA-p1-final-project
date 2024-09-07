using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
//using System.Collections.Generic;
//using System.Collections;
//using Unity.VisualScripting;
//using UnityEditor.Experimental.GraphView;
//using UnityEngine.XR.Interaction.Toolkit;

public class ShelfController : MonoBehaviour
{
    public GameObject blockPrefab;
    [SerializeField] GameObject attach;

    public GameObject currentBlock;
    [SerializeField] public int numberForNumberBlock;

    public LevelManager levelManager;

    protected virtual void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }

    public void startCreatingBlocks()
    {
        if (levelManager.returnNumberOfBlocks(blockPrefab) > 0)
        {
            GetComponentInChildren<TextMeshProUGUI>().text = levelManager.returnNumberOfBlocks(blockPrefab).ToString();
            StartCoroutine(Tool.c_InvokeAfterWait(0.1f, CreateNewBlock));
            levelManager.substractNumberOfBlocks(blockPrefab, numberForNumberBlock);
            
        }
        else
        {
            GetComponentInChildren<TextMeshProUGUI>().text = "";
        }
    }

    protected virtual void CreateNewBlock()
    {
        currentBlock = Instantiate(blockPrefab);
        currentBlock.transform.parent = attach.transform;

        if (currentBlock.name.Contains("NumberBlock"))
        {
            currentBlock.GetComponent<ObjectBlock>().setAssociatedString(numberForNumberBlock.ToString());
        }

        float xPos;
        float yPos;
        if (blockPrefab.name == "TurnLeftBlock" || blockPrefab.name == "TurnRightBlock" || blockPrefab.name == "ObjectBlock"
            || blockPrefab.name == "NumberBlock")
        {
            xPos = 0;
            yPos = 0;
        }
        else
        {
            if (currentBlock.GetComponent<BoxCollider>() != null)
            {
                xPos = currentBlock.GetComponent<BoxCollider>().size.z * 4;
            }
            else
            {
                xPos = currentBlock.transform.GetChild(0).GetComponent<BoxCollider>().size.z * 4;
            }
            yPos = 0.2f;
        }
        
        currentBlock.transform.localPosition = new Vector3(xPos, yPos, 0);
        currentBlock.transform.localRotation = blockPrefab.transform.rotation;

        EventsManager eventsManager;
        if (FindObjectOfType<EventsManager>() != null)
        {
            eventsManager = FindObjectOfType<EventsManager>();
            eventsManager.subscribeGrabEvents(currentBlock.GetComponent<XRGrabInteractable>());

            if (currentBlock.GetComponent<SocketsControl>() != null)
            {
                XRSocketInteractor[] xRSockets = currentBlock.GetComponent<SocketsControl>().getSockets();

                foreach (XRSocketInteractor socket in xRSockets)
                {
                    if (socket != null) eventsManager.subscribeSocketsEvents(socket);
                }
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject == currentBlock || (other.gameObject.transform.parent != null && other.gameObject.transform.parent.gameObject == currentBlock)){

            if (blockPrefab.name.Contains("Number"))
            {
                if (levelManager.returnNumberOfBlocks(blockPrefab, numberForNumberBlock) > 0)
                {
                    CreateNewBlock();
                    Debug.Log(numberForNumberBlock);
                    levelManager.substractNumberOfBlocks(blockPrefab, numberForNumberBlock);
                    GetComponentInChildren<TextMeshProUGUI>().text = levelManager.returnNumberOfBlocks(blockPrefab, numberForNumberBlock).ToString();
                }
                else
                {
                    GetComponentInChildren<TextMeshProUGUI>().text = "";
                }
            }
            else if (levelManager.returnNumberOfBlocks(blockPrefab) > 0)
            {
                GetComponentInChildren<TextMeshProUGUI>().text = levelManager.returnNumberOfBlocks(blockPrefab, numberForNumberBlock).ToString();
                CreateNewBlock();
                levelManager.substractNumberOfBlocks(blockPrefab, numberForNumberBlock);
            }
            else
            {
                GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }
    }

    public void actualiceText()
    {
        transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (levelManager.returnNumberOfBlocks(blockPrefab, numberForNumberBlock) + 1).ToString();
    }
}