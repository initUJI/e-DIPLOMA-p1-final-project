using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class CarController_V2 : MonoBehaviour
{
    public const float TILE_DISTANCE = 0.3f;
    public const float moveDuration = 0.3f;
    public LayerMask obstacleLayer;         // Capa que define qu� objetos son considerados obst�culos

    private bool isMoving = false;
    private List<string> procesedBlockList = new List<string>();

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

    public IEnumerator ProcesarSecuences(List<BlockObject> blockSecuence)
    {
        if (!CheckIfSecuenceIsPosible(blockSecuence))
        {
            Debug.LogError("La secuencia es incorrecta");
            yield break;
        }
        Debug.Log(procesedBlockList.Count);

        Vector3 initPosition = this.transform.position;
        Quaternion initRotation = this.transform.rotation;

        for (int i = 0; i < procesedBlockList.Count; i++)
        { 
            Debug.Log(procesedBlockList[i]);
            switch (procesedBlockList[i])
            {
                case "MoveForward":
                    bool canMove = MoveForward();
                    if (!canMove)
                    {
                        Debug.LogError("El coche se ha chocado");
                        this.transform.position = initPosition;
                        this.transform.rotation = initRotation;
                        yield break;
                    }
                    else {
                        yield return new WaitForSeconds(1f);
                    }
                    break;
                case "Right":
                    TurnRight();
                    yield return new WaitForSeconds(1f);
                    break;
                case "Left":
                    TurnLeft();
                    yield return new WaitForSeconds(1f);
                    break;
                case "If":
                    if (!IsObstacleInFront())
                    {
                        for (int j = i + 1; j < procesedBlockList.Count; j++)
                        {
                            if (procesedBlockList[j] == "EndIf")
                            {
                                i = j;
                                break;
                            }
                        }
                    }
                    yield return new WaitForSeconds(0.1f);
                    break;
                default:
                    break;
            }
        }
    }

    IEnumerator WaitBeforeContinuing()
    {
        Debug.Log("Antes de esperar.");

        // Espera durante 0.5 segundos
        yield return new WaitForSeconds(1f);

        // Después de esperar 0.5 segundos, continúa aquí
        Debug.Log("Después de esperar medio segundo.");
    }

    public bool MoveForward()
    {
        // 1� Realizar un check con un raycast desde el GameObject hacia adelante
        Vector3 forward = transform.TransformDirection(Vector3.forward) * TILE_DISTANCE;

        if (IsObstacleInFront())
        {
            // Si el raycast golpea un obst�culo, lanzar un error y no ejecutar el movimiento
            Debug.LogError("Obstacle detected! Cannot move forward.");
            return false;
        }
        else
        {
            Debug.Log("Moving forward...");
            isMoving = true;
            // Si no hay obst�culo, mover el coche hacia adelante con un movimiento suave utilizando LeanTween
            LeanTween.move(gameObject, transform.position + forward, moveDuration).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() => isMoving = false);
            return true;
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
        List<string> blockStringList = Utilities.BlockListToStringList(blockList);
        procesedBlockList = new List<string>();

        for (int i = 0; i < blockStringList.Count; i++)
        {
            if (blockStringList[i] == "For")
            {
                int j = 1;
                string block_repeats = blockStringList[i + j];
                if (Regex.IsMatch(block_repeats, @"^n\d+$"))
                {
                    j++;
                    List<string> repeatedBlockList = new List<string>();
                    while (blockStringList[i + j] != "EndFor")
                    {
                        repeatedBlockList.Add(blockStringList[i + j]);
                        j++;
                        if (i + j >= blockStringList.Count)
                        {
                            Debug.LogError("For block not closed");
                            return false;
                        }
                    }

                    int n = int.Parse(block_repeats.Substring(1));
                    for (int k = 0; k < n; k++)
                    {
                        procesedBlockList.AddRange(repeatedBlockList);
                    }
                    i = i + j;
                }
                else
                {
                    Debug.LogError("Invalid For block format" );
                    return false;
                }
            }
            else if (blockStringList[i] == "If")
            {
                int j = 1;
                while (blockStringList[i + j] != "EndIf")
                {
                    j++;
                    if (i + j >= blockStringList.Count)
                    {
                        Debug.LogError("If block not closed");
                        return false;
                    }
                }
                procesedBlockList.Add(blockStringList[i]);
            }
            else if (blockStringList[i] == "Turn")
            {
                if (i + 1 >= blockStringList.Count || (blockStringList[i + 1] != "Right" && blockStringList[i + 1] != "Left"))
                {
                    Debug.LogError("Invalid Turn block format");
                    return false;
                }
            }
            else
            {
                procesedBlockList.Add(blockStringList[i]);
            }
        }
        Debug.Log("Movement sequence is valid.");
        return true;
    }
}
