using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontColliderController : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.tag);
        if (other.CompareTag("TREE") || other.CompareTag("ROCK") || other.CompareTag("FLAG") || other.CompareTag("OBSTACLE"))
        {
            GameManager.objectInFront = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == GameManager.objectInFront)
            GameManager.objectInFront = null;
    }
}
