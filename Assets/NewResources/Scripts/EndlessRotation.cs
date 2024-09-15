using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessRotation : MonoBehaviour
{
    // Velocidad de rotación en grados por segundo
    public float rotationSpeed = 100f;

    // Eje sobre el que se quiere girar (por defecto el eje Y)
    public Vector3 rotationAxis = Vector3.up;

    // Update se llama una vez por frame
    void FixedUpdate()
    {
        // Rotar el objeto en función de la velocidad y el tiempo transcurrido entre frames
        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);
    }
}
