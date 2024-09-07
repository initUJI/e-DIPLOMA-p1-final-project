using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platformController : MonoBehaviour
{
    [HideInInspector] public bool cubeColliding = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("Cube"))
        {
            cubeColliding = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name.Contains("Cube"))
        {
            //cubeColliding = false;
        }
    }
}
