using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public const float TILE_DISTANCE = 0.3f;
    public const float moveDuration = 0.3f;
    public LayerMask obstacleLayer;         // Capa que define qué objetos son considerados obstáculos

    public void Update()
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

    public void MoveForward()
    {
        // 1º Realizar un check con un raycast desde el GameObject hacia adelante
        Vector3 forward = transform.TransformDirection(Vector3.forward) * TILE_DISTANCE;

        if (IsObstacleInFront())
        {
            // Si el raycast golpea un obstáculo, lanzar un error y no ejecutar el movimiento
            Debug.LogError("Obstacle detected! Cannot move forward.");
        }
        else
        {
            // Si no hay obstáculo, mover el coche hacia adelante con un movimiento suave utilizando LeanTween
            LeanTween.move(gameObject, transform.position + forward, moveDuration).setEase(LeanTweenType.easeInOutQuad);
        }
    }

    public bool IsObstacleInFront()
    {
        RaycastHit hit;
        Vector3 forward = transform.TransformDirection(Vector3.forward) * TILE_DISTANCE;

        if (Physics.Raycast(transform.position, forward, out hit, TILE_DISTANCE, obstacleLayer))
        {
            // Si el raycast golpea un obstáculo, lanzar un error y no ejecutar el movimiento
            Debug.LogError("Obstacle detected! Cannot move forward.");
            return true;
        }
        else
        {
            return false;
        }
    }

        // Girar el coche 90 grados hacia la derecha
        public void TurnRight()
    {
        Quaternion targetRotation = transform.rotation * Quaternion.Euler(0, 90, 0);

        LeanTween.rotate(gameObject, targetRotation.eulerAngles, moveDuration).setEase(LeanTweenType.easeInOutQuad);
    }

    // Girar el coche 90 grados hacia la izquierda
    public void TurnLeft()
    {
        Quaternion targetRotation = transform.rotation * Quaternion.Euler(0, -90, 0);

        LeanTween.rotate(gameObject, targetRotation.eulerAngles, moveDuration).setEase(LeanTweenType.easeInOutQuad);
    }
}
