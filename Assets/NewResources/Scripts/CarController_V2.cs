using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;

public class CarController_V2 : MonoBehaviour
{
    public const float TILE_DISTANCE = 0.3f;
    public const float moveDuration = 0.3f;
    public LayerMask obstacleLayer;         // Capa que define qu� objetos son considerados obst�culos
    public GameManager_V2 gameManager;

    private bool isMoving = false;
    private List<string> procesedBlockList = new List<string>();
    private TextMeshProUGUI playerMainBlockText;


    public void Awake()
    {
        gameManager = FindObjectOfType<GameManager_V2>();
    }

    // TESTING INPUT
    public void Update()
    {
        /*if (!isMoving)
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
        }*/
    }

    public void setPlayerText(TextMeshProUGUI playerText)
    {
        playerMainBlockText = playerText;
    }

    public IEnumerator ProcesarSecuences(List<BlockObject> blockSecuence, bool isPartnerSecuence = false)
    {
        procesedBlockList = new List<string>();
        CheckIfSecuenceIsPosible(blockSecuence, 1);

        //Debug.Log("Secuencia completa: " + string.Join(", ", blockSecuence));

        Vector3 initPosition = this.transform.position;
        Quaternion initRotation = this.transform.rotation;

        for (int i = 0; i < procesedBlockList.Count; i++)
        { 
            switch (procesedBlockList[i])
            {
                case "MoveForward":
                    bool canMove = MoveForward();
                    if (!canMove)
                    {
                        //Debug.LogError("El coche se ha chocado");
                        if (playerMainBlockText != null) playerMainBlockText.text = "Car crashed, reseting position...";
                        this.transform.position = initPosition;
                        this.transform.rotation = initRotation;

                        if (this == gameManager.localCarController)
                        {
                            if (playerMainBlockText != null)  playerMainBlockText.text = "Your code";  //Habrá que limpiar el mainBlock cara a hacer el nuevo turno
                            gameManager.setXRInteractionNewState(true);
                        }
                        else
                        {
                            gameManager.InitializeNextTurn();
                        }


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
                    if (!IsObstacleInFront(true))
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


        if(this == gameManager.localCarController)
        {
            if (playerMainBlockText != null) playerMainBlockText.text = "Your code";  //Habrá que limpiar el mainBlock cara a hacer el nuevo turno
            gameManager.setXRInteractionNewState(true);
        }else
        {
            //Debug.Log("Se va a iniciar el siguiente turno");
            gameManager.InitializeNextTurn();
        }
    }

    IEnumerator WaitBeforeContinuing()
    {
        //Debug.Log("Antes de esperar.");

        // Espera durante 0.5 segundos
        yield return new WaitForSeconds(1f);

        // Después de esperar 0.5 segundos, continúa aquí
        //Debug.Log("Después de esperar medio segundo.");
    }

    public bool MoveForward()
    {
        // 1� Realizar un check con un raycast desde el GameObject hacia adelante
        //Debug.Log("Partner moviendose");
        Vector3 forward = transform.TransformDirection(Vector3.forward) * TILE_DISTANCE;

        if (IsObstacleInFront())
        {
            // Si el raycast golpea un obst�culo, lanzar un error y no ejecutar el movimiento
            //Debug.LogError("Obstacle detected! Cannot move forward.");
            return false;
        }
        else
        {
            //Debug.Log("Moving forward...");
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

    public bool IsObstacleInFront(bool isConditional = false)
    {
        if(!isConditional)
        {
            RaycastHit hit;
            Vector3 forward = transform.TransformDirection(Vector3.forward) * TILE_DISTANCE;

            // Define las capas que quieres revisar (puedes especificar varias capas con una combinación de bits)
            int layersToCheck = LayerMask.GetMask("ObstacleObject", "OutOfBoundsLayer");

            if (Physics.Raycast(transform.position, forward, out hit, TILE_DISTANCE, layersToCheck))
            {
                // Si el raycast golpea un obstáculo en alguna de las capas especificadas, lanzar un error y no ejecutar el movimiento
                // Debug.LogError("Obstacle detected in one of the layers! Cannot move forward.");
                return true;
            }
            else
            {
                return false;
            }
        }
        //Este bucle lo utiliza un condicional
        else
        {
            RaycastHit hit;
            Vector3 forward = transform.TransformDirection(Vector3.forward) * TILE_DISTANCE;

            if (Physics.Raycast(transform.position, forward, out hit, TILE_DISTANCE, obstacleLayer))
            {
                // Si el raycast golpea un obst�culo, lanzar un error y no ejecutar el movimiento
                //Debug.LogError("Obstacle detected! Cannot move forward.");
                return true;
            }
            else
            {
                return false;
            }
        }
        
    }

    public bool CheckIfSecuenceIsPosible(List<BlockObject> blockList, int step)
    {
        if(blockList.Count == 0)
        {
            if (playerMainBlockText != null) playerMainBlockText.text = "ERROR: Empty sentence!";
            return false;
        }

        // Crear una copia virtual de la posici�n y rotaci�n actuales del coche
        List<string> blockStringList = Utilities.BlockListToStringList(blockList);
        List<string> auxProcessedBlockList = new List<string>();
        //procesedBlockList = new List<string>();

        for (int i = 0; i < blockStringList.Count; i++)
        {
            if (blockStringList[i] == "For")
            {
                int j = 1;
                if (i+j >= blockStringList.Count)
                {
                    //Debug.LogError("For before block not closed");
                    if (playerMainBlockText != null) playerMainBlockText.text = "ERROR: For block not closed!";
                    return false;
                }
                string block_repeats = blockStringList[i + j];
                if (Regex.IsMatch(block_repeats, @"^n\d+$"))
                {
                    j++;
                    List<string> repeatedBlockList = new List<string>();
                    List<BlockObject> blocksList = new List<BlockObject>();
                    while (blockStringList[i + j] != "EndFor")
                    {
                        repeatedBlockList.Add(blockStringList[i + j]);
                        blocksList.Add(blockList[i + j]);
                        Debug.Log(blockList[i + j]);
                        j++;

                        if (i + j >= blockStringList.Count)
                        {
                            //Debug.LogError("For analized block not closed");
                            if (playerMainBlockText != null) playerMainBlockText.text = "ERROR: For block not closed!";
                            return false;
                        }
                    }
                    if (!CheckIfSecuenceIsPosible(blocksList, step + 1))
                    {
                        return false;
                    }
                    int n = int.Parse(block_repeats.Substring(1));
                    for (int k = 0; k < n; k++)
                    {
                        auxProcessedBlockList.AddRange(repeatedBlockList);
                    }
                    i = i + j;
                }
                else
                {
                    //Debug.LogError("Invalid For block format" );
                    if (playerMainBlockText != null) playerMainBlockText.text = "ERROR: Number Block on repeat missing";
                    return false;
                }
            }
            else if (blockStringList[i] == "If")
            {
                int j = 1;
                if (i + j >= blockStringList.Count)
                {
                    if (playerMainBlockText != null) playerMainBlockText.text = "ERROR: Missing end-if block!";
                    return false;
                }
                while (blockStringList[i + j] != "EndIf")
                {
                    j++;
                    if (i + j >= blockStringList.Count)
                    {
                        //Debug.LogError("If block not closed");
                        if (playerMainBlockText != null) playerMainBlockText.text = "ERROR: Missing end-if block";
                        return false;
                    }
                }
                auxProcessedBlockList.Add(blockStringList[i]);
            }
            else if (blockStringList[i] == "Turn")
            {
                if (i + 1 >= blockStringList.Count || (blockStringList[i + 1] != "Right" && blockStringList[i + 1] != "Left"))
                {
                    //Debug.LogError("Invalid Turn block format");
                    if (playerMainBlockText != null) playerMainBlockText.text = "ERROR: Direction block missing";
                    return false;
                }
            }
            else
            {
                auxProcessedBlockList.Add(blockStringList[i]);
            }
        }
        if(playerMainBlockText != null) playerMainBlockText.text = "Code is OK. Wait for partner code.";
        //Debug.Log("Movement sequence is valid.");
        Debug.Log("Secuencia completa: " + string.Join(", ", procesedBlockList));
        if (step == 1)
        {
            procesedBlockList.AddRange(auxProcessedBlockList);
        }
        return true;
    }

    public void setCustomPlayerLogText(string newText)
    {
        playerMainBlockText.text = newText;
    }
}
