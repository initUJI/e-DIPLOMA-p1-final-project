using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorObject : MonoBehaviour
{
    public GameObject objectInFront = null;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TREE") || other.CompareTag("ROCK") || other.CompareTag("FLAG") || other.CompareTag("OBSTACLE"))
            objectInFront = other.gameObject;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == objectInFront)
            objectInFront = null;
    }
}
