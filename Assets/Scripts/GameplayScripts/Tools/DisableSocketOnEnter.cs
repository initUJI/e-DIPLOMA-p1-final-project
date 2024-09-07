using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DisableSocketOnEnter : MonoBehaviour
{
    [SerializeField] string socketToDisable;
    private GameObject go;

    private void OnCollisionEnter(Collision collision)
    {
        (go = Tool.FindChildWithTag(collision.gameObject, socketToDisable)).SetActive(false);
    }

    private void OnCollisionExit(Collision collision)
    {
        go.SetActive(true);
    }
}