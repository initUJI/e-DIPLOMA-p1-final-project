using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class VerticalRestabilization : MonoBehaviour
{
    public void Restabilize()
    {
        Vector3 currentRotation = transform.eulerAngles;
        currentRotation.z = 0f;
        currentRotation.x = 0f;
        transform.eulerAngles = currentRotation;
    }
}
