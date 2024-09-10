using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBlock_Controller : MonoBehaviour
{
    [SerializeField] Transform bottomAttach;
    public GameObject bottomObject;

    [SerializeField] GameObject bottomLight;

    public bool isBottomFilled = false;


    /*private void OnTriggerEnter(Collider other)
    {
        if(!isBottomFilled)
        {
            if (other.tag == "Block")
            {
                other.GetComponent<BlockController>().updateBlockToAttach(bottomAttach);
                bottomLight.SetActive(true);
                isBottomFilled = true;
                bottomObject = other.gameObject;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == bottomObject)
        {
            other.GetComponent<BlockController>().updateBlockToAttach(null);
            bottomLight.SetActive(false);
            isBottomFilled = false;
            bottomObject = null;
        }
    }*/
}
