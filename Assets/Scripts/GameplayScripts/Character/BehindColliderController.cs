using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehindColliderController : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TREE") || other.CompareTag("ROCK") || other.CompareTag("FLAG") || other.CompareTag("OBSTACLE"))
        {
            GameManager.objectInBehind = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == GameManager.objectInBehind)
            GameManager.objectInBehind = null;
    }
}
