using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public const float TILE_DISTANCE = 0.3f;
    public const float moveDuration = 0.3f;
    public LayerMask obstacleLayer;         // Capa que define qu� objetos son considerados obst�culos

    private bool isMoving = false;

    // TESTING INPUT
    public void Update()
    {
        if (!isMoving)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                MoveForward();
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                TurnRight();
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                TurnLeft();
            }
        }
    }

    public void ProcesarSecuences(List<BlockObject> blockSecuence)
    {
        if( CheckIfSecuenceIsPosible(blockSecuence))
        {
            Debug.Log("El movimiento est� bien");
        }else
        {
            Debug.LogError("El coche se la ha pegado :(");
        }

        // Si el movimiento est� bien, ejecutar la secuencia
    }

    public void MoveForward()
    {
        // 1� Realizar un check con un raycast desde el GameObject hacia adelante
        Vector3 forward = transform.TransformDirection(Vector3.forward) * TILE_DISTANCE;

        if (IsObstacleInFront())
        {
            // Si el raycast golpea un obst�culo, lanzar un error y no ejecutar el movimiento
            Debug.LogError("Obstacle detected! Cannot move forward.");
        }
        else
        {
            isMoving = true;
            // Si no hay obst�culo, mover el coche hacia adelante con un movimiento suave utilizando LeanTween
            LeanTween.move(gameObject, transform.position + forward, moveDuration).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() => isMoving = false);
        }
    }

    // Girar el coche 90 grados hacia la derecha
    public void TurnRight()
    {
        isMoving = true;
        Quaternion targetRotation = transform.rotation * Quaternion.Euler(0, 90, 0);

        LeanTween.rotate(gameObject, targetRotation.eulerAngles, moveDuration).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() => isMoving = false);
    }

    // Girar el coche 90 grados hacia la izquierda
    public void TurnLeft()
    {
        isMoving = true;
        Quaternion targetRotation = transform.rotation * Quaternion.Euler(0, -90, 0);

        LeanTween.rotate(gameObject, targetRotation.eulerAngles, moveDuration).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() => isMoving = false);
    }

    public bool IsObstacleInFront()
    {
        RaycastHit hit;
        Vector3 forward = transform.TransformDirection(Vector3.forward) * TILE_DISTANCE;

        if (Physics.Raycast(transform.position, forward, out hit, TILE_DISTANCE, obstacleLayer))
        {
            // Si el raycast golpea un obst�culo, lanzar un error y no ejecutar el movimiento
            Debug.LogError("Obstacle detected! Cannot move forward.");
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckIfSecuenceIsPosible(List<BlockObject> blockList)
    {
        // Crear una copia virtual de la posici�n y rotaci�n actuales del coche
        Vector3 virtualPosition = transform.position;
        Quaternion virtualRotation = transform.rotation;

        List<string> blockStringList = Utilities.BlockListToStringList(blockList);

        // Recorrer cada movimiento en la secuencia
        foreach (string movement in blockStringList)
        {
            // Simular el movimiento basado en el tipo de instrucci�n
            if (movement == "MoveForward")
            {
                Vector3 forward = virtualRotation * Vector3.forward * TILE_DISTANCE;
                RaycastHit hit;

                // Comprobar si hay un obst�culo en la direcci�n del movimiento
                if (Physics.Raycast(virtualPosition, forward, out hit, TILE_DISTANCE, obstacleLayer))
                {
                    Debug.LogError("Obstacle detected in simulated move. Sequence is invalid.");
                    return false; // Secuencia no v�lida
                }
                else
                {
                    // Actualizar la posici�n virtual si no hay obst�culos
                    virtualPosition += forward;
                }
            }
            else if (movement == "Right")
            {
                // Simular una rotaci�n de 90 grados hacia la derecha
                virtualRotation *= Quaternion.Euler(0, 90, 0);
            }
            else if (movement == "Left")
            {
                // Simular una rotaci�n de 90 grados hacia la izquierda
                virtualRotation *= Quaternion.Euler(0, -90, 0);
            }
            else
            {
                Debug.LogError("Unknown movement command: " + movement);
                return false; // Secuencia no v�lida debido a un comando desconocido
            }
        }

        // Si la secuencia completa se simul� sin problemas, es v�lida
        Debug.Log("Movement sequence is valid.");
        return true;
    }
}
